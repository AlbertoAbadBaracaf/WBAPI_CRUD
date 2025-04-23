using System.ComponentModel.DataAnnotations;

namespace WBAPI_CRUD.Models.Dtos.catUserApp
{
    public class catUserApplicationsRegistroDto
    {
        [Required(ErrorMessage ="El userName es obligatorio")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El Name es obligatorio")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El Password es obligatorio")]
        public string Password { get; set; }
    }
}
