using Application.Contracts;
using Application.Services;
using Context;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ProductRepository : GenericRepository<Product, int>, IProductRepository
    {
        private readonly ProductDbcontext _context;

        public ProductRepository(ProductDbcontext context) : base(context)
        {
            _context = context;
        }

        public object Products => throw new NotImplementedException();

        public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
        }
        public async Task<PaginatedList<Product>> GetProductsPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Products.AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PaginatedList<Product>(items, totalCount, pageNumber, pageSize, totalPages);
        }



    }
}
