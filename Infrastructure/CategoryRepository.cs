using Application.Contracts;
using Context;
using DTOs;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class CategoryRepository: GenericRepository<Category, int>, ICategoryRepository
    {
        private readonly ProductDbcontext _context;

        public CategoryRepository(ProductDbcontext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
            
                })
                .ToListAsync();
        }

        public async Task<bool> CategoryNameExists(string categoryName)
        {
            return await _context.Categories
                .AnyAsync(category => category.Name == categoryName);
        }

    }
}

