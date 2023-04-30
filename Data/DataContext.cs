using Feipder.Migrations;
using Feipder.Models;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Data
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionString"));
            // options.UseNpgsql(Configuration.GetConnectionString("Postgresql"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasOne(s => s.Parent)
                .WithMany(m => m.Children)
                .HasForeignKey(e => e.ParentId);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Brand> Brands { get; set; }
    }
}
