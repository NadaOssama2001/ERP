using Application.Contracts;
using AutoMapper;
using DTOs;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            // Fetch all products using repository
            var products = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _repository.GetProductsByCategoryIdAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            // Fetch product by ID using repository
            var product = await _repository.GetByIdAsync(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task AddProductAsync(ProductDTO productDto)
        {
            // Map DTO to Product model
            var product = _mapper.Map<Product>(productDto);
            product.CreatedAt = DateTime.Now; // Add timestamp
            await _repository.AddAsync(product);
        }

        public async Task UpdateProductAsync(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _repository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<PaginatedList<ProductDTO>> GetProductsPagedAsync(int pageNumber, int pageSize)
        {
            var paginatedProducts = await _repository.GetProductsPagedAsync(pageNumber, pageSize);

            return new PaginatedList<ProductDTO>(
                _mapper.Map<List<ProductDTO>>(paginatedProducts.Items),
                paginatedProducts.TotalCount,
                paginatedProducts.PageNumber,
                paginatedProducts.PageSize,
                paginatedProducts.TotalPages
            );
        }
        
    }
}
