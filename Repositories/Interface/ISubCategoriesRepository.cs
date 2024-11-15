using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface ISubCategoriesRepository
    {
        Task<List<SubCategory>> GetAllSubCategory();
        Task<SubCategory> GetSubCategoryById(string SubCategoryId);
        Task<bool> AddSubCategory(SubCategory subCategory);
        Task<bool> UpdateSubCategory(SubCategory subCategory);
        Task DeleteSubCategory(string SubCategoryId);

    }
}
