using AutoFixture;
using Feipder.Data.Repository;
using Feipder.Entities;
using Feipder.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Xml.Serialization;
using static System.Net.WebRequestMethods;

namespace Feipder.Data
{
    public static class Seeder
    {
        private static string RandomImageRef(string name, int width, int height)
        {
            return $"https://loremflickr.com/{width}/{height}/{name}";
        }

        public static void Seed(this DataContext dataContext, IConfiguration config)
        {
            if (!dataContext.Categories.Any())
            {
                var categories = new List<Category>()
                {
                    new Category()
                    {
                        Id= 1,
                        Name = "Clothes",
                        Alias = "Одежда",
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 2,
                        Name = "Shoes",
                        Alias = "Обувь",
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 3,
                        Name = "Accessories",
                        Alias = "Аксессуары",
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 4,
                        Name = "Sneakers",
                        Alias = "Кроссовки",
                        ParentId = 2,
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 5,
                        Name = "Gumshoes",
                        Alias = "Кеды",
                        ParentId = 2,
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 6,
                        Name = "Jeans",
                        Alias = "Джинсы",
                        ParentId = 1,
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 7,
                        Name = "TShirts",
                        Alias = "Футболки",
                        ParentId = 1,
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 8,
                        Name = "Hats",
                        Alias = "Головные уборы",
                        ParentId = 3,
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 9,
                        Name = "Balaclavas",
                        Alias = "Балаклавы",
                        ParentId = 8,
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 10,
                        Name = "BaseballCaps",
                        Alias = "Бейсболки",
                        ParentId = 8,
                        IsVisible = true
                    },
                    new Category()
                    {
                        Id= 11,
                        Name = "Caps",
                        Alias = "Кепки",
                        ParentId = 8,
                        IsVisible = true
                    }
                };

                var configImageSection = config.GetSection("ImageGeneration:Categories");

                var width = Convert.ToInt32(configImageSection["Width"]);
                var height = Convert.ToInt32(configImageSection["Height"]);

                foreach (var category in categories)
                {
                    category.Image = new CategoryImage()
                    {
                        Name = "RandomImage",
                        Url = RandomImageRef(category.Name, width, height)
                    };
                }

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

                var shoesCategory = dataContext.Categories.FirstOrDefault((c) => c.Name.Equals("Shoes"));

                shoesSizes.ForEach(size => shoesCategory?.Sizes.Add(size));

                var shirtCategory = dataContext.Categories.FirstOrDefault((c) => c.Name.Equals("TShirts"));

                shirtSizes.ForEach(size => shirtCategory?.Sizes.Add(size));

                var clothCategory = dataContext.Categories.FirstOrDefault((c) => c.Name.Equals("Clothes"));
                shoesSizes.ForEach(size => clothCategory?.Sizes.Add(size));
                
                var accessoriesCategory = dataContext.Categories.FirstOrDefault((c) => c.Name.Equals("Accessories"));
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
                                                            .Without(x => x.PreviewImage)
                                                            .Without(x => x.Brand)
                                                            .Without(x => x.CreatedDate)
                                                            .Without(x => x.Category));

                var products = fixture.CreateMany<Product>(200).ToList();

                var previewSettings = config.GetSection("ImageGeneration:ProductPreview");
                var productImageSttings = config.GetSection("ImageGeneration:ProductImage");

                var previewW = Convert.ToInt32(previewSettings["Width"]);
                var previewH = Convert.ToInt32(previewSettings["Height"]);
                
                var productW = Convert.ToInt32(productImageSttings["Width"]);
                var productH = Convert.ToInt32(productImageSttings["Height"]);

                foreach(var p in products)
                {
                    p.Color = colors[random.Next(0, colors.Count)];
                    p.Brand = brands[random.Next(0, brands.Count)];
                    p.Category = lastCategories[random.Next(0, lastCategories.Count)];
                    p.Price = random.NextDouble() * 1000 + 100;
                    p.IsNew = random.Next(100) < 20;

                    var imageCount = random.Next(1, 7);
                    var productImages = new List<ProductImage>(imageCount);

                    for (var i = 0; i < imageCount; i++)
                    {
                        var productImage = new ProductImage() { Name = "Random product image", Url = RandomImageRef($"{p.Category.Name}", productW, productH) };
                        productImages.Add(productImage);
                    }

                    var previewImage = new ProductPreviewImage() { Name = "Random preview image", Url = RandomImageRef($"{p.Category.Name}", previewW, previewH) };
                   
                    p.PreviewImage = previewImage;
                    p.ProductImages = productImages;
                }

                dataContext.AddRange(products);
                dataContext.SaveChanges();
            }

            if (!dataContext.Storage.Any())
            {
                var random = new Random();
                var storageRowCount = 100;
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
