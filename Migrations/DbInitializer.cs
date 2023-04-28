
using Feipder.Models;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Migrations
{
    public class DbInitializer
    {
        private readonly ModelBuilder _modelBuilder;

        public DbInitializer(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            _modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Article = "Куртка", Description = "Куртка зеленая"},
                new Product { Id = 2, Article = "Шапка", Description = "Шапка на голову"},
                new Product { Id = 3, Article = "Штаны", Description = "Штаны на ноги"},
                new Product { Id = 4, Article = "Перчатки", Description = "Перчатки на руки"},
                new Product { Id = 5, Article = "Удавка", Description = "Удавка на шею"}
            );
        }
    }
}
