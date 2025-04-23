using System.ComponentModel.DataAnnotations;

namespace WBAPI_CRUD.Models.Dtos.catUserApp
{
    public class catUserApplicationsDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string UserStatus { get; set; }
    }
}
