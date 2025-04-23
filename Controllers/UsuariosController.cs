using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WBAPI_CRUD.Models;
using WBAPI_CRUD.Models.Dtos.catUserApp;
using WBAPI_CRUD.Models.Dtos.logWebAppDto;
using WBAPI_CRUD.Repositorio.IRepositorio;

namespace WBAPI_CRUD.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ICatUserApplicationsRepositorio _usRepo;
        protected RespuestaAPI _respuestaApi;
        private readonly IMapper _mapper;
        private readonly IlogWebAppWBAPI_CRUDRepositorio _logRepo;
        protected logWebAppWBAPI_CRUDRegistroDto _logDto;

        public UsuariosController(ICatUserApplicationsRepositorio usRepo, IMapper mapper, IlogWebAppWBAPI_CRUDRepositorio logRepo)
        {
            _usRepo = usRepo;
            _mapper = mapper;
            this._respuestaApi = new();
            this._logDto = new();
            _logRepo = logRepo;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _usRepo.GetAllCatUserApplications();

            var listaUsuariosDto = new List<catUserApplicationsDto>();

            foreach (var lista in listaUsuarios)
            {
                listaUsuariosDto.Add(_mapper.Map<catUserApplicationsDto>(lista));
            }
            return Ok(listaUsuariosDto);
        }


        [HttpGet("{userId:int}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUsuario(int userId)
        {
            var itemUsuario = _usRepo.GetCatUserApplications(userId);

            if (itemUsuario == null)
            {
                return NotFound();
            }

            var itemUsuarioDto = _mapper.Map<catUserApplicationsDto>(itemUsuario);

            return Ok(itemUsuarioDto);
        }
        
        [HttpDelete("{userId:int}", Name = "deleteUsuario")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUsuario(int userId)
        {
            var itemUsuario = _usRepo.DeleteR(userId);

            if (itemUsuario == false)
            {
                return NotFound();
            }
                return Ok();
        }


        [HttpPost("registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] catUserApplicationsRegistroDto usuarioRegistroDto)
        {
            Guid UIDd = Guid.NewGuid();
            string body = string.Empty;
            string? response = string.Empty;
            _logDto.fecha = DateTime.Now;
            _logDto.UIDd = UIDd.ToString();

            try
            {
                body = System.Text.Json.JsonSerializer.Serialize(usuarioRegistroDto);
                _logDto.request = body;
                bool validarNombreUsuarioUnico = _usRepo.IsUniqueUser(usuarioRegistroDto.UserName);
                if (!validarNombreUsuarioUnico)
                {
                    _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.ErrorMessages.Add("El nombre de usuario ya existe");
                    response = System.Text.Json.JsonSerializer.Serialize(_respuestaApi);
                    _logDto.Response = response;
                    await _logRepo.Log(_logDto);
                    return BadRequest(_respuestaApi);
                }

                var usuario = await _usRepo.Registro(usuarioRegistroDto);
                if (usuario == null)
                {
                    _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.ErrorMessages.Add("Error en el registro");
                    response = System.Text.Json.JsonSerializer.Serialize(_respuestaApi);
                    _logDto.Response = response;
                    await _logRepo.Log(_logDto);
                    return BadRequest(_respuestaApi);
                }

                _respuestaApi.StatusCode = HttpStatusCode.OK;
                _respuestaApi.IsSuccess = true;
                return Ok(_respuestaApi);
            }
            catch (Exception ex)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("Error inesperado, contacte al administrador.");

                response = System.Text.Json.JsonSerializer.Serialize(_respuestaApi);

                _logDto.Response = response;
                _logDto.error = $"error: {ex.Message}, stack: {ex.StackTrace}";
                await _logRepo.Log(_logDto);

                return StatusCode(500, _respuestaApi);
            }
        }
       
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] catUserApplicationsUpdateDto usuarioUpdateDto)
        {
            Guid UIDd = Guid.NewGuid();
            string body = string.Empty;
            string? response = string.Empty;
            _logDto.fecha = DateTime.Now;
            _logDto.UIDd = UIDd.ToString();

            try
            {
                body = System.Text.Json.JsonSerializer.Serialize(usuarioUpdateDto);
                _logDto.request = body;

                var usuario = await _usRepo.UpdateR(usuarioUpdateDto);
                if (usuario == null)
                {
                    _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.ErrorMessages.Add("Error en el Update");
                    response = System.Text.Json.JsonSerializer.Serialize(_respuestaApi);
                    _logDto.Response = response;
                    await _logRepo.Log(_logDto);
                    return BadRequest(_respuestaApi);
                }

                _respuestaApi.StatusCode = HttpStatusCode.OK;
                _respuestaApi.IsSuccess = true;
                return Ok(_respuestaApi);
            }
            catch (Exception ex)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("Error inesperado, contacte al administrador.");

                response = System.Text.Json.JsonSerializer.Serialize(_respuestaApi);

                _logDto.Response = response;
                _logDto.error = $"error: {ex.Message}, stack: {ex.StackTrace}";
                await _logRepo.Log(_logDto);

                return StatusCode(500, _respuestaApi);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] catUserApplicationsLoginDto usuarioLoginDto)
        {
            Guid UIDd = Guid.NewGuid();
            string body = System.Text.Json.JsonSerializer.Serialize(usuarioLoginDto);
            string? response = string.Empty;
            _logDto.request = body;
            _logDto.fecha = DateTime.Now;
            _logDto.UIDd = UIDd.ToString();
            try
            {
                var respuestaLogin = await _usRepo.Login(usuarioLoginDto);

                if (respuestaLogin.Usuario == null || string.IsNullOrEmpty(respuestaLogin.Token))
                {
                    _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.ErrorMessages.Add("El nombre de usuario o password son incorrectos");
                    response = System.Text.Json.JsonSerializer.Serialize(_respuestaApi);
                    _logDto.Response = response;
                    await _logRepo.Log(_logDto);
                    return BadRequest(_respuestaApi);
                }
                _respuestaApi.StatusCode = HttpStatusCode.OK;
                _respuestaApi.IsSuccess = true;
                _respuestaApi.Result = respuestaLogin;
                response = System.Text.Json.JsonSerializer.Serialize(_respuestaApi);
                _logDto.Response = response;
                await _logRepo.Log(_logDto);
                return Ok(_respuestaApi);
            }
            catch (Exception ex)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("Error inesperado, contacte al administrador.");

                response = System.Text.Json.JsonSerializer.Serialize(_respuestaApi);

                _logDto.Response = response;
                _logDto.error = $"error: {ex.Message}, stack: {ex.StackTrace}";
                await _logRepo.Log(_logDto);

                return StatusCode(500, _respuestaApi);
            }

        }
    }
}

