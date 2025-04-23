using WBAPI_CRUD.Models;
using WBAPI_CRUD.Models.Dtos.catUserApp;

namespace WBAPI_CRUD.Repositorio.IRepositorio
{
    public interface ICatUserApplicationsRepositorio
    {
        ICollection<catUserApplications> GetAllCatUserApplications();
        catUserApplications GetCatUserApplications(int userId);
        bool DeleteR(int id);
        bool IsUniqueUser(string userName);
        Task<catUserApplicationsRespuestaDto> Login(catUserApplicationsLoginDto usuarioLoginDto);
        Task<catUserApplications> Registro(catUserApplicationsRegistroDto userApplicationsRegistroDto);
        Task<catUserApplications> UpdateR(catUserApplicationsUpdateDto usuarioUpdateDto);

    }
}
