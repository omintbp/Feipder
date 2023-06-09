﻿using Feipder.Entities;
using Feipder.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Feipder.Entities.RequestModels;
using Feipder.Entities.ResponseModels;
using Feipder.Data.Repository;
using Feipder.Entities.ResponseModels.Basket;
using System.Drawing;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

using Feipder.Tools;

using Feipder.Entities.Models.ResponseModels.Products;
using Feipder.Entities.ResponseModels.Products;
using System.Reflection.Metadata.Ecma335;

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly IRepositoryWrapper _repository;

        public BasketController(UserManager<User> userManager, DataContext context, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            _context = context;
            _repository = repository;
        }


        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Получение корзины авторизованного пользователя")]
        public async Task<ActionResult<BasketResponse>> GetBasket()
        {
            try
            {
                var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone);
                var user = await _context.Users.Where(x => x.PhoneNumber.Equals(userPhone))
                    .Include(x => x.Basket)
                    .ThenInclude(x => x.Items)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return BadRequest();
                }

                var items = new List<BasketItemResponse>();

                foreach (var item in user.Basket.Items)
                {
                    var product = _repository.Products.FindByCondition(x => x.Id == item.ProductId).FirstOrDefault();
                    var size = _repository.Sizes.FindByCondition(x => x.Id == item.SizeId).FirstOrDefault();
                    var color = _repository.Colors.FindByCondition(x => x.Id == item.ColorId).FirstOrDefault();

                    if(product == null)
                    {
                        return BadRequest();
                    }

                    var available = _repository.Sizes.FindByProduct(product, true)
                              .Where(s => s.Id == size.Id)
                              .First().Available;

                    var itemResponse = new BasketItemResponse()
                    {
                        Id = item.Id,
                        Article = product.Article,
                        Name = product.Name,
                        Price = product.Price,
                        Count = item.Count,
                        ProductId = product.Id,
                        ProductColor = new ProductColor(color),
                        ProductSize = new ProductSize(size)
                        {
                            Available = available
                        },
                        Available = available,
                        ProductImage = product.ProductImages.FirstOrDefault()
                    };

                    itemResponse.IsLast = itemResponse.ProductSize.Available == 1;
                    items.Add(itemResponse);
                }

                var basketResponse = new BasketResponse()
                {
                    Items = items,
                    DiscountPrice = 0,
                    ItemsCount = items.Count,
                    TotalPrice = items.Sum(x => x.Price * x.Count)
                };
                
                return Ok(basketResponse);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("basketStatus")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Получение краткой информации о корзине", 
            Description = "Можно использовать для отображения количества товаров в иконке корзины или же при создании заказа")]
        public async Task<ActionResult<BasketStatusResponse>> GetBasketStatus()
        {
            try
            {
                var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone);
                var user = await _context.Users.Where(x => x.PhoneNumber.Equals(userPhone))
                    .Include(x => x.Basket)
                    .ThenInclude(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync();

                return Ok(new BasketStatusResponse()
                {
                   TotalPrice = user.Basket.Items.Sum(x => x.Count * x.Product.Price),
                   PriceWithDiscount = user.Basket.Items.Sum(x => x.Count * x.Product.Price),
                   ProductsCount = user.Basket.Items.Count
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }
        }
        
        [HttpPost("basketInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Посчитать актуальную информацию о корзине неавторизованного пользователя",
            Description = "Посчитать итоговую информацию для корзины по переданным товарам")]
        public ActionResult<BasketResponse> CalculateBasket([FromBody]ItemsStatusRequest request)
        {
            try
            {
                var response = new BasketResponse();

                if (!request.Items.Any())
                {
                    return Ok(response);
                }
                var basketItems = new List<BasketItemResponse>();

                var index = 0;

                foreach (var item in request.Items)
                {
                    var product = _repository.Products.FindByCondition(x => x.Id == item.ProductId).FirstOrDefault();
                    var size = _repository.Sizes.FindByCondition(x => x.Id == item.SizeId).FirstOrDefault();
                    var color = _repository.Colors.FindByCondition(x => x.Id == item.ColorId).FirstOrDefault();

                    if (color == null)
                    {
                        return BadRequest($"Цвет с id = {item.SizeId} не найден");
                    }

                    if (size == null)
                    {
                        return BadRequest($"Размер с id = {item.SizeId} не найден");
                    }

                    if (product == null)
                    {
                        return BadRequest($"Продукт с id = {item.ProductId} не найден");
                    }

                    var sizes = _repository.Sizes.FindByProduct(product, true).ToList();

                    if (!sizes.Any(x => x.Id == size.Id))
                    {
                        return BadRequest($"У продукта id = {product.Id} не существует размера с id = {size.Id}");
                    }
                    
                    if(!product.Colors.Any(x => x.Id == color.Id))
                    {
                        return BadRequest($"У продукта id = {product.Id} не существует цвета с id = {color.Id}");
                    }

                    var available = _repository.Sizes.FindByProduct(product, true)
                                .Where(s => s.Id == size.Id)
                                .First().Available;

                    var itemResponse = new BasketItemResponse()
                    {
                        Id = index++,
                        Article = product.Article,
                        Name = product.Name,
                        Price = product.Price,
                        Count = item.Count,
                        Category = new CategoryResponse(product.Category),
                        Brand = new BrandResponse(product.Brand),
                        ProductId = product.Id,
                        ProductColor = new ProductColor(color),
                        ProductSize = new ProductSize(size)
                        {
                            Available = available
                        },
                        Available = available,
                        ProductImage = product.ProductImages.FirstOrDefault()
                    };

                    itemResponse.IsLast = itemResponse.Available == 1;
                    basketItems.Add(itemResponse);
                }

                response.Items = basketItems;
                response.DiscountPrice = 0;
                response.ItemsCount = basketItems.Count;
                response.TotalPrice = basketItems.Sum(x => x.Price * x.Count);

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Добавление товара в корзину", 
            Description = "Если товар в корзине уже есть, то их количество суммируется")]
        public async Task<ActionResult<PostItemResponse>> PostItem([FromBody]BasketPostItemRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone);
                var user = await _context.Users.Where(x => x.PhoneNumber.Equals(userPhone))
                    .Include(x => x.Basket)
                    .ThenInclude(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return BadRequest();
                }

                var product = _context.Products.Include(x => x.Colors).Where(x => x.Id == request.ProductId).FirstOrDefault();
                var size = _repository.Sizes.FindByCondition(x => x.Id == request.SizeId).FirstOrDefault();
                var color = _repository.Colors.FindByCondition(x => x.Id == request.ColorId).FirstOrDefault();
                
                if (product == null || size == null || color == null || !product.Colors.Contains(color))
                {
                    return BadRequest("Не удалось найти подходящий товар");
                }

                /// проверям, доступен ли у товара такой размер
                var isCurrectSize = _context.Storage.Where(x => x.ProductId == product.Id).Any(x => x.SizeId == size.Id);

                if (!isCurrectSize)
                {
                    return BadRequest($"У товара {product.Id} нет доступного размера {size.Id}");
                }

                var basketItem = await _context.BasketItems.Where(x => x.BasketId == user.BasketId 
                    && x.ProductId == product.Id && x.ColorId == color.Id && x.SizeId == size.Id)
                    .FirstOrDefaultAsync();

                if (basketItem != null)
                {
                    basketItem.Count += request.Count;

                    _context.BasketItems.Update(basketItem);
                }
                else
                {
                    user.Basket.Items.Add(new BasketItem()
                    {
                        Count = request.Count,
                        Color = color,
                        ColorId = color.Id,
                        ProductId = product.Id,
                        Product = product,
                        Size = size,
                        SizeId = size.Id
                    });
                }

                await _context.SaveChangesAsync();

                var response = new PostItemResponse()
                {
                    BasketTotalCount = user.Basket.Items.Count,
                    ProductId = product.Id,
                    ProductsInBasketCount = user.Basket.Items.Where(x => x.ProductId == product.Id).First().Count
                };

                return CreatedAtAction(nameof(PostItem), new { }, response);

            } catch(Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("itemUpdate")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Обновление количества товара в корзине",
            Description = "В корзине явно есть кнопочки увеличения/уменьшения количества товара. Сюда сразу шлем итоговое количество. " +
            "Если прислать count = 0, то товар удаляем. В ответ получаем обновленную информацию о корзине")]
        public async Task<ActionResult<ItemUpdateResponse>> ItemUpdate(ItemUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone);
                var user = await _context.Users.Where(x => x.PhoneNumber.Equals(userPhone))
                    .Include(x => x.Basket)
                    .ThenInclude(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return BadRequest();
                }

                /// получение объекта продукта, количество которого нужно обновить
                var product = await _context.Products
                    .Include(x => x.Colors)
                    .Where(x => x.Id == request.ProductId)
                    .FirstOrDefaultAsync();

                var color = await _context.Colors.FindAsync(request.ColorId);
                var size = await _context.Sizes.FindAsync(request.SizeId);

                if(product == null || color == null || size == null || !product.Colors.Contains(color))
                {
                    return BadRequest("Такого продукта найти не удалось");
                }

                /// получаем доступные размеры этого товара
                var sizes = _context.Storage.Where(p => p.ProductId == product.Id).Include(x => x.Size).Select(x => x.Size).ToList();

                /// товары данного размера закончились
                if (sizes.Find(s => s.Id == size.Id) == null)
                {
                    return BadRequest("Товары с таким размером закончились");
                }

                var basket = await _context.Baskets.Include(x => x.Items).Where(x => x.Id == user.BasketId).FirstOrDefaultAsync();
                var item = await _context.BasketItems.Where(x => x.BasketId == user.BasketId && 
                                                                 x.ColorId == color.Id &&
                                                                 x.SizeId == size.Id &&
                                                                 x.ProductId == product.Id)
                    .Include(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                    .Include(x => x.Color)
                    .Include(x => x.Size)
                    .FirstOrDefaultAsync();


                if(item == null)
                {
                    return BadRequest($"Не удалось найти товар с id = {request.ProductId} в корзине");
                }

                if(request.NewCount == 0)
                {
                    _context.BasketItems.Remove(item);
                }
                else
                {
                    item.Count = request.NewCount;
                   _context.BasketItems.Update(item);

                }

                var response = new ItemUpdateResponse()
                {
                    DiscountPrice = 0,
                    Item = new BasketItemResponse()
                    {
                        Id = item.Id,
                        Article = item.Product.Article,
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        Count = item.Count,
                        ProductId = item.Product.Id,
                        ProductColor = new ProductColor(item.Color),
                        ProductSize = new ProductSize(item.Size),
                        ProductImage = item.Product.ProductImages.FirstOrDefault()
                    },

                    /// количество элементов в обновленной корзине
                    ItemsCount = basket.Items.Count,

                    /// рассчет итоговой цены обновленной корзины.
                    TotalPrice = basket.Items.Sum(x => {
                        var product = _repository.Products.FindByCondition(p => p.Id == x.ProductId).First();
                        return x.Count * product.Price;
                        }
                    )
                };

                await _context.SaveChangesAsync();

                return Ok(response);

            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
