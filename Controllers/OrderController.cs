using Feipder.Entities;
using Feipder.Entities.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Feipder.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Feipder.Entities.ResponseModels;
using Feipder.Entities.ResponseModels.Orders;
using Swashbuckle.AspNetCore.Annotations;

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public OrderController(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Создание нового заказа",
            Description = "DeliveryType = Delivery или Self. Если Delivery, то нужно обязательно заполнить Address. " +
            "Если же Self, то обязательно прислать id выбранной точки самовывоза. В ответ тут получаем ID созданного заказа." +
            "После создания заказа товары из корзины убираются")]
        public async Task<ActionResult<PostOrderResponse>> PostOrder(PostOrderRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(request.DeliveryType == DeliveryType.Delivery && request.Address == null)
            {
                return BadRequest("Адрес доставки не установлен");
            }

            if(request.DeliveryType == DeliveryType.Self && request.PickupPointId == 0)
            {
                return BadRequest("Точка доставки не установлена");
            }
            else if(request.DeliveryType == DeliveryType.Self)
            {
                var pickupPoint = _context.PickupPoints.Where( x => x.Id == request.PickupPointId)
                    .Include(x => x.Address)
                    .FirstOrDefault();

                if(pickupPoint == null)
                {
                    return BadRequest("Доставка в эту точку вывоза недоступна");
                }

                request.Address = pickupPoint.Address;
            }

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

            if(!user.Basket.Items.Any())
            {
                return BadRequest("В корзине ничего нет");
            }

            var items = user.Basket.Items.ToList().Select(x => {

                var color = _context.Colors.Find(x.ColorId);
                var size = _context.Sizes.Find(x.SizeId);

                /// получаем доступное количество таких размеров для этого товара
                var itemInStorage = _context.Storage.Where(s => s.SizeId == size.Id && s.ProductId == x.ProductId).First();

                /// если размеров недостаточно
                if(itemInStorage.TotalCount < x.Count)
                {
                    ModelState.AddModelError("product", $"Недостаточно товара id = {x.ProductId} на складе ");
                }

                /// если товара достаточно, то отнимаем количество заказанных товаров
                itemInStorage.TotalCount -= x.Count;

                _context.Storage.Update(itemInStorage);

                var orderItem = new OrderItem()
                {
                    Color = color,
                    Count = x.Count,
                    Product = x.Product,
                    ProductId = x.Id,
                    Size = size
                };
                return orderItem;
                }
            ).ToList();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _context.Orders.AddAsync(new Order()
            {
                Address = request.Address,
                Comment = request.Comment,
                DateCreated = DateTimeOffset.UtcNow,
                DateModified = DateTimeOffset.UtcNow,
                DeliveryType = request.DeliveryType,
                Email = request.Email,
                Name = request.Name,
                Phone = request.Phone,
                OrderStatus = OrderStatus.Created,
                User = user,
                Items = items
            });

            /// удаляем из корзины те товары, на которые сделали заказ
            //user.Basket.Items.ToList().RemoveAll(x => x.Count != 0);
            var userBasket = user.Basket;
            userBasket.Items.Clear();

            _context.Baskets.Update(userBasket);
            await _context.SaveChangesAsync();

            return Created(nameof(PostOrder), new PostOrderResponse() { Id = result.Entity.Id});
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Получение всех заказов текущего пользователя",
            Description = "Не уверен, нужно ли это вам, поскольку в тз не нашёл наличие личного кабинета, но пусть эта фигня тут будет")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders()
        {
            try
            {

                var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone);
                var user = await _context.Users.Where(x => x.PhoneNumber.Equals(userPhone))
                    .Include(x => x.Orders)
                        .ThenInclude(x => x.Address)
                    .Include(x => x.Orders)
                        .ThenInclude(x => x.Items)
                            .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync();

                var orders = user.Orders.Select(order =>
                {
                    var items = order.Items.Select(item =>
                    {
                        var color = _context.Colors.Find(item.ColorId);
                        var size = _context.Sizes.Find(item.SizeId);

                        return new OrderItemResponse()
                        {
                            Color = color.Value,
                            Name = item.Product.Name,
                            ProductId = item.ProductId,
                            Count = item.Count,
                            Size = size.Value,
                            Price = item.Product.Price
                        };
                    }).ToList();

                    return new OrderResponse()
                    {
                        Id = order.Id,
                        Comment = order.Comment,
                        Email = order.Email,
                        Address = order.Address,
                        ItemsCount = order.Items.Count,
                        Name = order.Name,
                        DeliveryType = order.DeliveryType,
                        OrderStatus = order.OrderStatus,
                        Phone = order.Phone,
                        TotalPrice = order.Items.Sum(x => x.Count * x.Product.Price),
                        Items = items
                    };
                }).ToList();

                return Ok(orders);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
          
        }
    }
}
