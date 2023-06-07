using Feipder.Entities.Models;
using Feipder.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Entities
{
    public class DataContext : IdentityDbContext<User>
    {
        private readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionString"));
            // options.UseNpgsql(Configuration.GetConnectionString("Postgresql"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>()
                .HasOne(s => s.Parent)
                .WithMany(m => m.Children)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Product>().Property(x => x.CreatedDate).HasDefaultValueSql("now()");
            modelBuilder.Entity<TempUser>().Property(x => x.CreatedDate).HasDefaultValueSql("now()");
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Color> Colors { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Size> Sizes { get; set; } = null!;
        public DbSet<ProductStorage> Storage { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;
        public DbSet<ProductPreviewImage> ProductsPreviewImages { get; set; } = null!;
        public DbSet<TempUser> TempUsers { get; set; } = null!;
        public DbSet<Basket> Baskets { get; set; } = null!;

        public DbSet<FavoriteProduct> FavoriteProducts { get; set; } = null!;

        public DbSet<BasketItem> BasketItems { get; set; } = null!;
        public DbSet<Feature> Features { get; set; } = null!;

        public DbSet<WorkHour> WorkHours { get; set; } = null!;
        public DbSet<PickupPoint> PickupPoints { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;


    }
}
