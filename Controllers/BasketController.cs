using Feipder.Entities;
using Feipder.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Feipder.Entities.RequestModels;

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;

        public BasketController(UserManager<User> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBasket()
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);


                var managedUser = await _userManager.FindByEmailAsync(email);
                var user = _context.Users.Where(x => x.Email.Equals(email)).Include(x => x.Basket).FirstOrDefault();

                if (user == null)
                {
                    return BadRequest();
                }

                var basket = user.Basket;

                return Ok(basket.Items);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("itemsCount")]
        [Authorize]
        public async Task<IActionResult> GetProductsCount()
        {
            try
            {
                var user = _context.Users.Where(x => x.Email.Equals(User.FindFirstValue(ClaimTypes.Email)))
                    .Include(x => x.Basket).FirstOrDefault();

                return Ok(user.Basket.Items.Count);
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("itemsStatus")]
        public async Task<IActionResult> GetProductsStatus()
        {
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostItem([FromBody]BasketPostItemRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _context.Users.Where(x => x.Email.Equals(User.FindFirstValue(ClaimTypes.Email)))
                   .Include(x => x.Basket).FirstOrDefault();

                if(user == null)
                {
                    return BadRequest();
                }

                var product = await _context.Products.FindAsync(request.ProductId);
                
                if(product == null)
                {
                    return BadRequest("Такого id не существует");
                }

                var size = await _context.Sizes.FindAsync(request.SizeId);

                if(size == null)
                {
                    return BadRequest("Такого размера не существует");
                }

                /// проверям, доступен ли у товара такой размер

                var isCurrectSize = _context.Storage.Where(x => x.ProductId == product.Id).Any(x => x.SizeId == size.Id);

                if (!isCurrectSize)
                {
                    return BadRequest($"У товара {product.Id} нет доступного размера {size.Id}");
                }

                user.Basket.Items.Add(new BasketItem()
                {
                    Count = request.Count,
                    Product = product,
                    Size = size
                });

                await _context.SaveChangesAsync();

            } catch(Exception e)
            {
                return StatusCode(500);
            }
            return BadRequest();
        }

        [HttpPut("itemUpdate")]
        [Authorize]
        public async Task<IActionResult> ItemUpdate(ItemUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _context.Users.Where(x => x.Email.Equals(User.FindFirstValue(ClaimTypes.Email)))
                   .Include(x => x.Basket).FirstOrDefault();

                if (user == null)
                {
                    return BadRequest();
                }

                var item = user.Basket.Items.Where(x => x.Id == request.ItemId).FirstOrDefault();

                if(item == null)
                {
                    return BadRequest($"Элемента с id = {request.ItemId} в корзине нет");
                }

                item.Count = request.Count;

                _context.BasketItems.Update(item);
                await _context.SaveChangesAsync();

                return Ok();

            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("item/{itemId}")]
        [Authorize]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            try
            {
                var user = _context.Users.Where(x => x.Email.Equals(User.FindFirstValue(ClaimTypes.Email)))
                   .Include(x => x.Basket).FirstOrDefault();

                if (user == null)
                {
                    return BadRequest();
                }

                var item = user.Basket.Items.Where(x => x.Id == itemId).FirstOrDefault();

                if(item == null)
                {
                    return BadRequest("Такого элемента в корзине не нашлось");
                }

                user.Basket.Items.Remove(item);
                _context.BasketItems.Remove(item);
                await _context.SaveChangesAsync();

                return Ok();

            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
