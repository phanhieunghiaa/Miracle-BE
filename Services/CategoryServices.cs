using Repositories.Data.Entity;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepositories _repository;

        public CategoryServices(ICategoryRepositories repository)
        {
            _repository = repository;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _repository.GetAllCategories();
        }

        public async Task<Category> GetCategoriesById(string categoryId)
        {
            return await _repository.GetCategoriesById(categoryId);
        }

        public async Task AddCategory(Category category)
        {
            await _repository.AddCategory(category);
        }
        public async Task<string> Update(string categoryId, Category category)
        {
            var existingCategory = await _repository.GetCategoriesById(categoryId);
            if (existingCategory == null)
            {
                return "Category not found";
            }
            else
            {
                var result = await _repository.UpdateCategory(existingCategory);
                return result ? "Update Successful" : "Update failed";
            }
        }
        public async Task DeleteCategory(string categoryId)
        {
            await _repository.DeleteCategory(categoryId);
        }
    }
}
