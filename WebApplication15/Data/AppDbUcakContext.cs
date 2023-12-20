
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace WebApplication15.Data
{
    public class AppDbUcakContext:DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbUcakContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("UcakApiDatabase"));

        }
        public DbSet<FlyName> FlyNames { get; set; }
        public DbSet<Voyage>Voyages { get; set; }
    }
}
