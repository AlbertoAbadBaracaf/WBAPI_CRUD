using System.ComponentModel.DataAnnotations;

namespace WBAPI_CRUD.Models.Dtos.catUserApp
{
    public class catUserApplicationsUpdateDto
    {
        [Required(ErrorMessage = "El Id es obligatorio")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Name es obligatorio")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El Password es obligatorio")]
        public string Password { get; set; }
    }
}
