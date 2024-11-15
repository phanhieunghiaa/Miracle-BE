using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface ICategoryRepositories
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategoriesById(string categoryId);
        Task AddCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task DeleteCategory(string categoryId);

    }
}
