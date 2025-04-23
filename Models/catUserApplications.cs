using System.ComponentModel.DataAnnotations;

namespace WBAPI_CRUD.Models
{
    public class catUserApplications
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool UserStatus { get; set; }
    }
}
