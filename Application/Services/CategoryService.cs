using Application.Contracts;
using AutoMapper;
using DTOs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;


        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;

        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            //return await _categoryRepository.GetAllCategoriesAsync();
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        public async Task<bool> CategoryNameExists(string categoryName)
        {
            return await _categoryRepository.CategoryNameExists(categoryName);
        }
    }
}
