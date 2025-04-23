using Microsoft.EntityFrameworkCore;
using WBAPI_CRUD.Models;

namespace WBAPI_CRUD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<catUserApplications> catUserApplications { get; set; }

        public DbSet<logWebAppWBAPI_CRUD> logWebAppWBAPI_CRUD { get; set; }
    }
}
