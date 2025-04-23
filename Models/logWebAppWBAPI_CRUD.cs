using System.ComponentModel.DataAnnotations;

namespace WBAPI_CRUD.Models
{
    public class logWebAppWBAPI_CRUD
    {
        [Key]
        public int idLog { get; set; }
        public DateTime fecha { get; set; }
        public string request { get; set; }
        public string Response { get; set; }
        public string UIDd { get; set; }
        public string error { get; set; }
    }
}
