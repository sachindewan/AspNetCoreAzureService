using azure_app_trev_vs.Models;
using Microsoft.EntityFrameworkCore;

namespace azure_app_trev_vs.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions):base(dbContextOptions)
        {
            
        }
        public DbSet<Person> Persons { get; set; } 
    }
}
