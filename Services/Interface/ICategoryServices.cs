using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ICategoryServices
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategoriesById(string categoryId);
        Task AddCategory(Category category);
        Task<string> Update(string categoryId, Category category);
        Task DeleteCategory(string categoryId);
    }
}
