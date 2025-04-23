using AutoMapper;
using WBAPI_CRUD.Models.Dtos.catUserApp;
using WBAPI_CRUD.Models;
using WBAPI_CRUD.Models.Dtos.logWebAppDto;

namespace WBAPI_CRUD.WBAPI_CRUD_Mappers
{
    public class WBAPI_CRUD_Mappers : Profile
    {
        public WBAPI_CRUD_Mappers()
        {
            CreateMap<catUserApplications, catUserApplicationsDto>().ReverseMap();
            CreateMap<catUserApplications, catUserApplicationsRegistroDto>().ReverseMap();
            CreateMap<logWebAppWBAPI_CRUD, logWebAppWBAPI_CRUDRegistroDto>().ReverseMap();
        }
    }
}
