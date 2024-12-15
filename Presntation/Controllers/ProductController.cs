using Application.Services;
using AutoMapper;
using Context;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Linq;

namespace Presntation.Controllers
{
[Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;
        private readonly ProductDbcontext _context;


        public ProductController(IProductService service, IMapper mapper, ProductDbcontext context)
        {
            _context = context;

            _service = service;
            _mapper = mapper;
        }
        //GetAll
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
            => Ok(await _service.GetAllProductsAsync());

        //GetProductById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }


        //Create Product
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromForm] ProductInputModel model)
        {
            if (model.Image == null || model.Image.Length > 1_000_000)
            {
                return BadRequest("Image is required and must be less than 1MB.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(model.Image.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Invalid image format. Only JPG, JPEG, and PNG are allowed.");
            }
            var imagePath = Path.Combine("wwwroot/images", Guid.NewGuid().ToString() + extension);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

            var productDTO = new ProductDTO
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                CategoryId = model.CategoryId,
                ImagePath = imagePath
            };

            await _service.AddProductAsync(productDTO);
            return CreatedAtAction(nameof(GetProductById), new { id = productDTO.Id }, productDTO);
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveProducts()
        {
            var currentTime = DateTime.UtcNow;
            var products = await _service.GetAllProductsAsync();
            var activeProducts = products.Where(p => p.StartDate <= currentTime && currentTime <= p.StartDate.AddDays(p.Duration));
            return Ok(activeProducts);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllProductsForAdmin()
        {
            return Ok(await _service.GetAllProductsAsync());
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductInputModel model)
        {
            var existingProduct = await _service.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            string imagePath = existingProduct.ImagePath;

            if (model.Image != null)
            {
                if (model.Image.Length > 1_000_000)
                {
                    return BadRequest("Image must be less than 1MB.");
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(model.Image.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest("Invalid image format. Only JPG, JPEG, and PNG are allowed.");
                }

                if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                imagePath = Path.Combine("wwwroot/images", Guid.NewGuid().ToString() + extension);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
            }

            existingProduct.Name = model.Name;
            existingProduct.Price = model.Price;
            existingProduct.Description = model.Description;
            existingProduct.CategoryId = model.CategoryId;
            existingProduct.ImagePath = imagePath;

            var productDTO = _mapper.Map<ProductDTO>(existingProduct);

            await _service.UpdateProductAsync(productDTO);
            return NoContent();
        }


        //Delete
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _service.DeleteProductAsync(id);
            return NoContent();
        }

        //Filter
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _service.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var paginatedProducts = await _service.GetProductsPagedAsync(pageNumber, pageSize);

            return Ok(new
            {
                paginatedProducts.Items, 
                paginatedProducts.TotalCount,  
                paginatedProducts.PageSize, 
                paginatedProducts.TotalPages 
            });
        }

    }
}
