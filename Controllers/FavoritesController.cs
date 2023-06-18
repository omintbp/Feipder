using Feipder.Data.Repository;
using Feipder.Entities;
using Feipder.Entities.Models;
using Feipder.Entities.RequestModels;
using Feipder.Entities.ResponseModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IRepositoryWrapper _repository;


        public FavoritesController(DataContext context, UserManager<User> userManager, IRepositoryWrapper repository)
        {
            _context = context;
            _repository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Получнеие списка избранных товаров текущего пользователя")]
        public async Task<ActionResult<IEnumerable<ProductPreview>>> GetFavorites([FromQuery]int offset = 0, [FromQuery]int limit = 20)
        {
            try
            {
                var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone);
                var user = await _context.Users.Where(x => x.PhoneNumber.Equals(userPhone))
                    .Include(x => x.Basket)
                    .ThenInclude(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync();

                var products = _context.FavoriteProducts
                    .Include(x => x.Product)
                    .Where(x => Guid.Parse(user.Id) == x.UserId)
                    .ToList()
                    .Select(x =>
                    {
                        var product = _repository.Products.FindByCondition(p => p.Id == x.ProductId).First();
                        var sizes = _repository.Sizes.FindByProduct(product, true);

                        return new ProductPreview(product)
                        {
                            Colors = product.Colors.Select(x => new ProductColor(x)).ToList(),
                            Sizes = sizes
                        };
                    })
                    .Skip(offset)
                    .Take(limit)
                    .ToList();

                return products;
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Получение количества избранных товаров")]
        public async Task<ActionResult<int>> GetFavoritesCount()
        {
            try
            {
                var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone);
                var user = await _context.Users.Where(x => x.PhoneNumber.Equals(userPhone))
                    .Include(x => x.Basket)
                    .ThenInclude(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync();

                var userFavorites = await _context.FavoriteProducts.Where(x => Guid.Parse(user.Id) == x.UserId).ToListAsync();

                return Ok(userFavorites.Count);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Добавито товар в избранные текущего пользователя")]
        public async Task<ActionResult> PostFavorite(PostFavoriteProductRequest request)
        {
            try
            {
                var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone);
                var user = await _context.Users.Where(x => x.PhoneNumber.Equals(userPhone))
                    .Include(x => x.Basket)
                    .ThenInclude(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync();

                var userFavorites = await _context.FavoriteProducts.Where(x => Guid.Parse(user.Id) == x.UserId).ToListAsync();
                var product = _repository.Products.FindByCondition(x => request.ProductId == x.Id).FirstOrDefault();

                if(product == null)
                {
                    return NotFound("Product not found");
                }

                if(userFavorites.Any(x => x.ProductId == request.ProductId))
                {
                    return BadRequest("Такой товар уже есть в избранном");
                }

                var favorite = new FavoriteProduct()
                {
                    ProductId = request.ProductId,
                    UserId = Guid.Parse(user.Id)
                };

                await _context.FavoriteProducts.AddAsync(favorite);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [HttpGet("recomendations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Получение списка избранных товаров для рекомендаций в корзине")]
        public async Task<ActionResult<IEnumerable<ProductPreview>>> GetRecomendations()
        {
            try
            {
                var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone);
                var user = await _context.Users.Where(x => x.PhoneNumber.Equals(userPhone))
                    .Include(x => x.Basket)
                    .ThenInclude(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync();

                var basketProductsIds = user.Basket.Items.Select(x => x.ProductId).ToList();

                var products = _context.FavoriteProducts
                    .Include(x => x.Product)
                    .Where(x => Guid.Parse(user.Id) == x.UserId && !basketProductsIds.Contains(x.ProductId))
                    .ToList()
                    .Select(x =>
                    {
                        var product = _repository.Products.FindByCondition(p => p.Id == x.ProductId).First();
                        var sizes = _repository.Sizes.FindByProduct(product, false);
                        return new ProductPreview(product)
                        {
                            Sizes = sizes
                        };
                    }).ToList();

                return products;
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
