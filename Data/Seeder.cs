using AutoFixture;
using Feipder.Data.Repository;
using Feipder.Entities;
using Feipder.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Xml.Serialization;

namespace Feipder.Data
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
                        Name = "Футболки",
                        Alias = "Футболки",
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

            if (!dataContext.Sizes.Any())
            {
                var shoesSizes = new List<Size>();

                /// размеры обуви
                for(var i = 31; i<=46; i++)
                {
                    shoesSizes.Add(new Size() { Id = i - 30, Value = $"{i}" });
                }

                var shirtSizes = new List<Size>() {
                    new Size() { Id = 17, Value = "S" },
                    new Size() { Id = 18, Value = "M" },
                    new Size() { Id = 19, Value = "L" },
                    new Size() { Id = 20, Value = "S" },
                    new Size() { Id = 21, Value = "XL" },
                    new Size() { Id = 22, Value = "XXL" }
                };


                dataContext.Sizes.AddRange(shoesSizes);
                dataContext.Sizes.AddRange(shirtSizes);

                dataContext.SaveChanges();

                var shoesCategory = dataContext.Categories.FirstOrDefault((c) => c.Name.Equals("Обувь"));

                shoesSizes.ForEach(size => shoesCategory?.Sizes.Add(size));

                var shirtCategory = dataContext.Categories.FirstOrDefault((c) => c.Name.Equals("Футболки"));

                shirtSizes.ForEach(size => shirtCategory?.Sizes.Add(size));

                var clothCategory = dataContext.Categories.FirstOrDefault((c) => c.Name.Equals("Одежда"));
                shoesSizes.ForEach(size => clothCategory?.Sizes.Add(size));
                
                var accessoriesCategory = dataContext.Categories.FirstOrDefault((c) => c.Name.Equals("Аксессуары"));
                shoesSizes.ForEach(size => accessoriesCategory?.Sizes.Add(size));

                dataContext.SaveChanges();
            }

            if (!dataContext.Products.Any())
            {
                var lastCategories = dataContext.Categories.Include(x => x.Children).Where(x => !x.Children.Any()).ToList();
                var brands = dataContext.Brands.ToList();
                var colors = dataContext.Colors.ToList();

                var random = new Random();
                var fixture = new Fixture();

                fixture.Customize<Product>(product => product.Without(x => x.Id)
                                                            .Without(x => x.Color)
                                                            .Without(x => x.Discount)
                                                            .Without(x => x.Price)
                                                            .Without(x => x.ProductImages)
                                                            .Without(x => x.Brand)
                                                            .Without(x => x.CreatedDate)
                                                            .Without(x => x.Category));

                var products = fixture.CreateMany<Product>(100).ToList();
                products.ForEach(p =>
                {
                    p.Color = colors[random.Next(0, colors.Count)];
                    p.Brand = brands[random.Next(0, brands.Count)];
                    p.Category = lastCategories[random.Next(0, lastCategories.Count)];
                    p.Price = random.NextDouble() * 1000 + 100;
                });
                dataContext.AddRange(products);
                dataContext.SaveChanges();
            }

            if (!dataContext.Storage.Any())
            {
                var random = new Random();
                var storageRowCount = 50;
                var repository = new RepositoryWrapper(dataContext);

                var products = dataContext.Products.Include(x => x.Category).ThenInclude(x => x.Sizes).Include(x => x.Category!.Parent).ToList();

                for(var i = 0; i < storageRowCount; i++)
                {
                    var productIndex = random.Next(0, products.Count);
                    var randomProduct = products[productIndex];

                    products.Remove(randomProduct);

                    var sizes = repository.Sizes.FindByCategory(randomProduct.Category).ToList();

                    var sizesCount = random.Next(1, 4);
                    for(var j = 0; j < sizesCount; j++)
                    {
                        var randomSize = sizes[random.Next(0, sizes.Count)];
                        sizes.Remove(randomSize);

                        var productStorage = new ProductStorage()
                        {
                            TotalCount = random.Next(1, 10),
                            Product = randomProduct,
                            Size = randomSize
                        };

                        dataContext.Storage.Add(productStorage);
                        //repository.Storage.Create(productStorage);
                    }

                }

                dataContext.SaveChanges();
            }
        }
    }
}
