using AutoFixture;
using Feipder.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Feipder.Tools
{
    public static class Seeder
    {
        public static void Seed(this DataContext dataContext)
        {
            if (!dataContext.Categories.Any())
            {
                var categories = new List<Category>()
                {
                    new Category()
                    {
                        Id= 1,
                        Name = "Одежда",
                        Alias = "Одежда",
                        Image = "https://otkritkis.com/wp-content/uploads/2022/02/pravila-kot-11.jpg",
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 2,
                        Name = "Обувь",
                        Alias = "Обувь",
                        Image = "https://otkritkis.com/wp-content/uploads/2022/02/pravila-kot-11.jpg",
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 3,
                        Name = "Аксессуары",
                        Alias = "Аксессуары",
                        Image = "https://otkritkis.com/wp-content/uploads/2022/02/pravila-kot-11.jpg",
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 4,
                        Name = "Кроссовки",
                        Alias = "Кроссовки",
                        ParentId = 2,
                        Image = "https://otkritkis.com/wp-content/uploads/2022/02/pravila-kot-11.jpg",
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 5,
                        Name = "Кеды",
                        Alias = "Кеды",
                        ParentId = 2,
                        Image = "https://otkritkis.com/wp-content/uploads/2022/02/pravila-kot-11.jpg",
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 6,
                        Name = "Джинсы",
                        Alias = "Джинсы",
                        ParentId = 1,
                        Image = "https://otkritkis.com/wp-content/uploads/2022/02/pravila-kot-11.jpg",
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 7,
                        Name = "Брюки",
                        Alias = "Брюки",
                        ParentId = 1,
                        Image = "https://otkritkis.com/wp-content/uploads/2022/02/pravila-kot-11.jpg",
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 8,
                        Name = "Головные уборы",
                        Alias = "Головные уборы",
                        ParentId = 3,
                        Image = "https://otkritkis.com/wp-content/uploads/2022/02/pravila-kot-11.jpg",
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 9,
                        Name = "Балаклавы",
                        Alias = "Балаклавы",
                        ParentId = 8,
                        Image = "https://otkritkis.com/wp-content/uploads/2022/02/pravila-kot-11.jpg",
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 10,
                        Name = "Бейсболки",
                        Alias = "Бейсболки",
                        ParentId = 8,
                        Image = "https://otkritkis.com/wp-content/uploads/2022/02/pravila-kot-11.jpg",
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 11,
                        Name = "Кепки",
                        Alias = "Кепки",
                        ParentId = 8,
                        Image = "https://otkritkis.com/wp-content/uploads/2022/02/pravila-kot-11.jpg",
                        IsVisible = true
                    }
                };

                dataContext.AddRange(categories);
                dataContext.SaveChanges();
            }

            if (!dataContext.Products.Any())
            {
                var lastCategories = dataContext.Categories.Include(x => x.Children).Where(x => !x.Children.Any()).ToList();

                var random = new Random();
                var fixture = new Fixture();

                fixture.Customize<Product>(product => product.Without(x => x.Id)
                                                            .Without(x => x.Brand)
                                                            .Without(x => x.Baskets)
                                                            .Without(x => x.Color)
                                                            .Without(x => x.Discounts).Without(x => x.FavoriteProducts)
                                                            .Without(x => x.SubscribeProducts)
                                                            .Without(x => x.ProductImages)
                                                            .Without(x => x.OrdersProducts)
                                                            .Without(x => x.ProductSizeAvailables)
                                                            .With(x => x.Category,
                                                                lastCategories[random.Next(0, lastCategories.Count)]
                                                            ));

                var products = fixture.CreateMany<Product>(10).ToList();
                dataContext.AddRange(products);
                dataContext.SaveChanges();
            }
        }
    }
}
