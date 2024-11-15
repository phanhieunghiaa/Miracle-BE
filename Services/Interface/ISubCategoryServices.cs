using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ISubCategoryServices
    {
        Task<List<SubCategory>> GetAllSubCategory();
        Task<SubCategory> GetSubCategoryById(string SubCategoryId);
        Task<string> AddSubCategory(AddSubCategoryDto subCategoryDto);
        Task<string> UpdateSubCategory(string subCategoryId, UpdateSubCategoryDTO subCategoryDto);
        Task<string> DeleteSubCategory(string subCategoryId);
    }
}
