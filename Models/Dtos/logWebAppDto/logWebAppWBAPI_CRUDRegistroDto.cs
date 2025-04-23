using System.ComponentModel.DataAnnotations;

namespace WBAPI_CRUD.Models.Dtos.logWebAppDto
{
    public class logWebAppWBAPI_CRUDRegistroDto
    {
        public DateTime fecha { get; set; }
        public string request { get; set; }
        public string Response { get; set; }
        public string UIDd { get; set; }
        public string error { get; set; }

    }
}
