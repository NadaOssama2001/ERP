using DTOs;
using Models;

namespace Application.Services
{
    public interface IProductService
    {

        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task AddProductAsync(ProductDTO productDto);
        Task UpdateProductAsync(ProductDTO productDto);
        Task DeleteProductAsync(int id);
        Task<PaginatedList<ProductDTO>> GetProductsPagedAsync(int pageNumber, int pageSize);


    }
}
