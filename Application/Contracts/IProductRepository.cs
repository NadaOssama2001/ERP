using Application.Contracts;
using Application.Services;
using Models;

public interface IProductRepository : IGenericRepository<Product, int>
{
    Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
    Task<PaginatedList<Product>> GetProductsPagedAsync(int pageNumber, int pageSize);
}
