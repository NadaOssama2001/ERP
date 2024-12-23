using Application.Services;
using AutoMapper;
using Context;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System.Net.Http.Headers;

namespace PresentationLayer.Controllers
{
    //[Authorize]

    public class ProductController : Controller
    {
        private readonly IProductService _service;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductController(IProductService service, ICategoryService categoryService, IMapper mapper)
        {
            _service = service;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var paginatedProducts = await _service.GetProductsPagedAsync(pageNumber, pageSize);
            return View(paginatedProducts);
        }

        public async Task<IActionResult> AllProducts()
        {
            var products = await _service.GetAllProductsAsync();
            return View(products);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        //[Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create(ProductInputModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(model);
            }

            string imagePath = await SaveImageAsync(model.Image);
            if (imagePath == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid image format or size.");
                await LoadCategoriesAsync();
                return View(model);
            }

            var productDTO = new ProductDTO
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                CategoryId = model.CategoryId,
                ImagePath = imagePath,
                CreatedBy = User.Identity?.Name ?? "System"
            };

            await _service.AddProductAsync(productDTO);
            return RedirectToAction(nameof(AllProducts));
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ProductInputModel>(product);
            await LoadCategoriesAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, ProductInputModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(model);
            }

            var existingProduct = await _service.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            string imagePath = existingProduct.ImagePath;
            if (model.Image != null)
            {
                imagePath = await SaveImageAsync(model.Image, existingProduct.ImagePath);
                if (imagePath == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid image format or size.");
                    await LoadCategoriesAsync();
                    return View(model);
                }
            }

            existingProduct.Name = model.Name;
            existingProduct.Price = model.Price;
            existingProduct.Description = model.Description;
            existingProduct.CategoryId = model.CategoryId;
            existingProduct.ImagePath = imagePath;

            await _service.UpdateProductAsync(_mapper.Map<ProductDTO>(existingProduct));
            return RedirectToAction(nameof(AllProducts));
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product != null && !string.IsNullOrEmpty(product.ImagePath))
            {
                var fullImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(fullImagePath))
                {
                    System.IO.File.Delete(fullImagePath);
                }
            }

            await _service.DeleteProductAsync(id);
            return RedirectToAction(nameof(AllProducts));
        }

        [AllowAnonymous]
        public async Task<IActionResult> FilterByCategory(int categoryId)
        {
            var products = await _service.GetProductsByCategoryAsync(categoryId);
            return View("Index", products);
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }

        private async Task<string> SaveImageAsync(IFormFile image, string existingImagePath = null)
        {
            if (image == null || image.Length > 1_000_000)
            {
                return null;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(image.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                return null;
            }

            try
            {
                if (!string.IsNullOrEmpty(existingImagePath))
                {
                    var fullExistingPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(fullExistingPath))
                    {
                        System.IO.File.Delete(fullExistingPath);
                    }
                }

                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                var newImagePath = Path.Combine("images", Guid.NewGuid() + extension);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newImagePath);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return "/" + newImagePath;
            }
            catch
            {
                return null;
            }
        }
    }
}
