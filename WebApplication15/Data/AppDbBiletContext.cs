using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WebApplication15.Data
{
    public class AppDbBiletContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbBiletContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("BiletApiDatabase"));
        }
        public DbSet<Ticket> Tickets { get; set; }

    }
}
