using Feipder.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Entities
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

            modelBuilder.Entity<Product>().Property(x => x.CreatedDate).HasDefaultValueSql("now()");
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Color> Colors { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Size> Sizes { get; set; } = null!;
        public DbSet<ProductStorage> Storage { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;
    }
}
