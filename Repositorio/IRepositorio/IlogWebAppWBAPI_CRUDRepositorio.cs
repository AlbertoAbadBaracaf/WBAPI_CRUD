using WBAPI_CRUD.Models;
using WBAPI_CRUD.Models.Dtos.logWebAppDto;

namespace WBAPI_CRUD.Repositorio.IRepositorio
{
    public interface IlogWebAppWBAPI_CRUDRepositorio
    {
        Task<logWebAppWBAPI_CRUD> Log(logWebAppWBAPI_CRUDRegistroDto userApplicationsRegistroDto);
    }
}
