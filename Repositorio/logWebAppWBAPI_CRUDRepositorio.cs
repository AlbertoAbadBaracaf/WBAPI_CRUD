using WBAPI_CRUD.Data;
using WBAPI_CRUD.Models;
using WBAPI_CRUD.Models.Dtos.logWebAppDto;
using WBAPI_CRUD.Repositorio.IRepositorio;

namespace WBAPI_CRUD.Repositorio
{
    public class logWebAppWBAPI_CRUDRepositorio : IlogWebAppWBAPI_CRUDRepositorio
    {
        private readonly ApplicationDbContext _bd;

        public logWebAppWBAPI_CRUDRepositorio(ApplicationDbContext bd)
        {
            _bd = bd;
        }

        public async Task<logWebAppWBAPI_CRUD> Log(logWebAppWBAPI_CRUDRegistroDto LogWebAppRegistroDto)
        {
            logWebAppWBAPI_CRUD Log = new logWebAppWBAPI_CRUD()
            {
                fecha = LogWebAppRegistroDto.fecha,
                request = LogWebAppRegistroDto.request,
                Response = LogWebAppRegistroDto.Response,
                UIDd = LogWebAppRegistroDto.UIDd,
                error = LogWebAppRegistroDto.error
            };

            _bd.logWebAppWBAPI_CRUD.Add(Log);

            await _bd.SaveChangesAsync();
            //usuario.Password = passwordEncriptado;
            return Log;
        }
    }
}
