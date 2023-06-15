﻿using AutoFixture;
using Feipder.Data.Repository;
using Feipder.Entities;
using Feipder.Entities.Admin.Products;
using Feipder.Entities.Models;
using Feipder.Entities.ResponseModels;
using Feipder.Entities.ResponseModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Feipder.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Администрирование товаров")]
    [Authorize(Roles = "admin")]
    public class ProductController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _accessor;

        public ProductController(IRepositoryWrapper repository, DataContext context, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _context = context;
            _env = env;
            _accessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<AdmProductsList>> GetProducts([FromQuery] string filterName = "",
            [FromQuery] string categoryName = "", [FromQuery] int offset = 0, [FromQuery] int limit = 20)
        {
            try
            {
                var results = new List<AdmProductPreview>();

                var products = _repository.Products.FindByCondition(p => p.Name.Contains(filterName, StringComparison.OrdinalIgnoreCase)
                        && p.Category.Name.Contains(categoryName, StringComparison.OrdinalIgnoreCase))
                    .Skip(offset)
                    .Take(limit)
                    .Select(p => new AdmProductShortPreview()
                    {
                        Name = p.Name,
                        Article = p.Article,
                        Category = new CategoryResponse(p.Category),
                        IsVisible = p.IsVisible,
                        Id = p.Id
                    }).ToList();

                var response = new AdmProductsList()
                {
                    Products = products,
                    Count = products.Count,
                    Limit = limit,
                    Offset = offset
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }


        [HttpGet("{productId}")]
        public async Task<ActionResult<AdmProductPreview>> GetProduct(int productId)
        {
            try
            {
                var product = _repository.Products.FindByCondition(x => x.Id == productId).FirstOrDefault();

                if (product == null)
                {
                    return NotFound();
                }

                var sizes = _repository.Sizes.FindByProduct(product, true);

                return Ok(new AdmProductPreview(product)
                {
                    Sizes = sizes
                });

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutProduct(AdmProductPutRequest request)
        {
            try
            {
                var product = _repository.Products.FindByCondition((p) => p.Id == request.ProductId).FirstOrDefault();

                if(product == null)
                {
                    return NotFound();
                }

                var category = _repository.Categories.FindByCondition(c => c.Id == request.CategoryId).FirstOrDefault();

                product.Category = category ?? product.Category;
                product.Price = request.Price;
                product.Article = request.Article ?? product.Article;
                product.Description = request.Description ?? product.Description;
                product.Title = request.Name ?? product.Title;
                product.Name = request.Alias ?? product.Name;
                product.IsVisible = request.IsVisible;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                var sizes = _repository.Sizes.FindByProduct(product, true);

                return Ok(new AdmProductPreview(product)
                {
                    Sizes = sizes
                });

            }
            catch(Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("images")]
        public async Task<ActionResult<ProductImage>> UpdateProductImage([FromForm]ImageUpdateRequest request)
        {
            try
            {
                var product = _repository.Products.FindByCondition(x => x.Id == request.ProductId)
                    .FirstOrDefault();

                if (product == null)
                {
                    return NotFound();
                }

                var productImage = product.ProductImages.Where(x => x.Id == request.ProductImageId).FirstOrDefault();

                if(productImage == null)
                {
                    return NotFound();
                }

                var imagePath = await _repository.Images.UploadImage("Products", request.UploadedFile, _env);

                var req = _accessor.HttpContext?.Request;
                var imageRef = $"{req.Scheme}://{req.Host.Value}/{imagePath}";

                productImage.Url = imageRef;
                productImage.Name = request.ImageName ?? productImage.Name;

                _context.ProductImages.Update(productImage);
                await _context.SaveChangesAsync();

                return Ok(productImage);

            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("images")]
        public async Task<ActionResult> PostProductImage()
        {
            try
            {
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{productId}/images")]
        public async Task<ActionResult<IEnumerable<ProductImage>>> GetProductImages(int productId)
        {
            try
            {
                var product = _repository.Products.FindByCondition(x => x.Id == productId)
                    .FirstOrDefault();

                if(product == null)
                {
                    return NotFound();
                }

                return product.ProductImages.ToList();

            }catch(Exception e)
            {
                return StatusCode(500);
            }
        }

    }
}
