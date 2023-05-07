using Feipder.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Data
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
                new Product { Id = 1, Article = "Куртка", Description = "КУртка на голову", Price = 40, IsVisible = true, PreviewImage = "https://ru.freepik.com/free-photo/portrait-young-handsome-male_12976276.htm#query=%D0%BA%D1%83%D1%80%D1%82%D0%BA%D0%B0%20%D0%B1%D0%BE%D0%BC%D0%B6%D0%B0&position=29&from_view=search&track=ais" },
                new Product { Id = 2, Article = "Шапка", Description = "Шапка на голову", Price = 50, IsVisible = true, PreviewImage = "https://ru.freepik.com/free-photo/portrait-young-handsome-male_12976276.htm#query=%D0%BA%D1%83%D1%80%D1%82%D0%BA%D0%B0%20%D0%B1%D0%BE%D0%BC%D0%B6%D0%B0&position=29&from_view=search&track=ais" },
                new Product { Id = 3, Article = "Штаны", Description = "Штаны на ноги", Price = 60, IsVisible = true, PreviewImage = "https://ru.freepik.com/free-photo/portrait-young-handsome-male_12976276.htm#query=%D0%BA%D1%83%D1%80%D1%82%D0%BA%D0%B0%20%D0%B1%D0%BE%D0%BC%D0%B6%D0%B0&position=29&from_view=search&track=ais" },
                new Product { Id = 4, Article = "Перчатки", Description = "Перчатки на руки", Price = 30, IsVisible = true, PreviewImage = "https://ru.freepik.com/free-photo/portrait-young-handsome-male_12976276.htm#query=%D0%BA%D1%83%D1%80%D1%82%D0%BA%D0%B0%20%D0%B1%D0%BE%D0%BC%D0%B6%D0%B0&position=29&from_view=search&track=ais" },
                new Product { Id = 5, Article = "Удавка", Description = "Удавка на шею", Price = 30, IsVisible = true, PreviewImage = "https://ru.freepik.com/free-photo/portrait-young-handsome-male_12976276.htm#query=%D0%BA%D1%83%D1%80%D1%82%D0%BA%D0%B0%20%D0%B1%D0%BE%D0%BC%D0%B6%D0%B0&position=29&from_view=search&track=ais" }
            );
        }
    }
}
