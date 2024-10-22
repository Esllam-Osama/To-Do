using Microsoft.EntityFrameworkCore;
using To_Do.Models;
namespace To_Do.data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Mission> missions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ToDo; Integrated Security=True;TrustServerCertificate=True");
        }
    }
}
