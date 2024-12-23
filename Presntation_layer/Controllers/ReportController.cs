using Application.Services;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Presntation_layer.Models;

namespace Presntation_layer.Controllers
{
    public class ReportController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        public ReportController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }
        [HttpGet]

        public async Task<IActionResult> Index()
        
        {
            var viewModel = new CategoryProductReportViewModel
            {
                Categories = await _categoryService.GetAllCategoriesAsync(),
                Products = new List<ProductDTO>()
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(int categoryId)
        {
            var viewModel = new CategoryProductReportViewModel
            {
                Categories = await _categoryService.GetAllCategoriesAsync(),
                Products = (List<ProductDTO>)await _productService.GetProductsByCategoryAsync(categoryId)
            };
            return View(viewModel);
        }
    }

}

