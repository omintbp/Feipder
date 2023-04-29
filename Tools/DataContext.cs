using Feipder.Migrations;
using Feipder.Models;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Tools
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Database.EnsureCreated();
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           // new DbInitializer(modelBuilder).Seed();
        }

        public DbSet<Product> Products { get; set; }
    }
}
