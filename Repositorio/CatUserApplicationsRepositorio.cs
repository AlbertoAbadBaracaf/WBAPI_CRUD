using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WBAPI_CRUD.Data;
using WBAPI_CRUD.Models;
using WBAPI_CRUD.Models.Dtos.catUserApp;
using WBAPI_CRUD.Repositorio.IRepositorio;
using XAct.Library.Settings;
using XSystem.Security.Cryptography;

namespace WBAPI_CRUD.Repositorio
{
    public class CatUserApplicationsRepositorio : ICatUserApplicationsRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private string claveSecreta;

        public CatUserApplicationsRepositorio(ApplicationDbContext bd, IConfiguration config)
        {
            _bd = bd;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
        }

        public catUserApplications GetCatUserApplications(int userId)
        {
            return _bd.catUserApplications.FirstOrDefault(c => c.Id == userId);
        }

        public ICollection<catUserApplications> GetAllCatUserApplications()
        {
            return _bd.catUserApplications.OrderBy(c => c.UserName).ToList();
        }

        public bool IsUniqueUser(string userName)
        {
            var usuarioBd = _bd.catUserApplications.FirstOrDefault(u => u.UserName == userName);
            if (usuarioBd == null)
            {
                return true;
            }
            return false;
        }

        public async Task<catUserApplicationsRespuestaDto> Login(catUserApplicationsLoginDto usuarioLoginDto)
        {
            var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);

            var usuario = _bd.catUserApplications.FirstOrDefault(
                u => u.UserName.ToLower() == usuarioLoginDto.UserName.ToLower()
                && u.Password == passwordEncriptado
                && u.UserStatus == true
                );

            //Validamos si el usuario no existe con la combinación de usuario y contraseña correcta
            if (usuario == null)
            {
                return new catUserApplicationsRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            //Aquí existe el usuario entonces podemos procesar el login
            var manejadoToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadoToken.CreateToken(tokenDescriptor);

            catUserApplicationsRespuestaDto usuarioLoginRespuestaDto = new catUserApplicationsRespuestaDto()
            {
                Token = manejadoToken.WriteToken(token),
                Usuario = usuario
            };

            return usuarioLoginRespuestaDto;
        }

        public async Task<catUserApplications> Registro(catUserApplicationsRegistroDto usuarioRegistroDto)
        {
            var passwordEncriptado = obtenermd5(usuarioRegistroDto.Password);

            catUserApplications usuario = new catUserApplications()
            {
                UserName = usuarioRegistroDto.UserName,
                Password = passwordEncriptado,
                Name = usuarioRegistroDto.Name,
                UserStatus = true
            };

            _bd.catUserApplications.Add(usuario);
            await _bd.SaveChangesAsync();
            usuario.Password = passwordEncriptado;
            return usuario;
        }

        public async Task<catUserApplications> UpdateR(catUserApplicationsUpdateDto usuarioUpdateDto)
        {
            var passwordEncriptado = obtenermd5(usuarioUpdateDto.Password);

            var result = _bd.catUserApplications.SingleOrDefault(b => b.Id == usuarioUpdateDto.Id);
            if (result != null)
            {
                result.Name = usuarioUpdateDto.Name;
                result.Password = passwordEncriptado;
                _bd.SaveChanges();
            }
            return result;
        }
        
        public bool DeleteR(int id)
        {
            var result = _bd.catUserApplications.SingleOrDefault(b => b.Id == id);
            if (result != null)
            {
                _bd.catUserApplications.Remove(result);
                _bd.SaveChanges();
            }
            else
            {
                return false;
            }
            return true;
        }

        //Método para encriptar contraseña con MD5 se usa tanto en el Acceso como en el Registro
        public static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }
    }
}
