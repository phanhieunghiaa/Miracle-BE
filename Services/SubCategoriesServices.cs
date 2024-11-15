using Micracle.DTOs;
using Repositories.Data.DTOs;
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
    public class SubCategoriesServices: ISubCategoryServices
    {
        private readonly ISubCategoriesRepository _subCategoriesRepository;
        private readonly ICategoryRepositories _categoryRepositories;

        public SubCategoriesServices(ISubCategoriesRepository subCategoriesRepository, ICategoryRepositories categoryRepositories)
        {
            _subCategoriesRepository = subCategoriesRepository;
            _categoryRepositories = categoryRepositories;
        }

        public async Task<List<SubCategory>> GetAllSubCategory()
        {
            return await _subCategoriesRepository.GetAllSubCategory();
        }

        public async Task<SubCategory> GetSubCategoryById(string SubCategoryId)
        {
            return await _subCategoriesRepository.GetSubCategoryById(SubCategoryId);
        }

        public async Task<string> AddSubCategory(AddSubCategoryDto subCategoryDto)
        {
           if (subCategoryDto == null)
            {
                return "Subcategory not found";
            }
            if (string.IsNullOrWhiteSpace(subCategoryDto.Type) || subCategoryDto.Type == "string")
            {
                throw new Exception("Products must be required");
            }
            SubCategory newSubCategory = new SubCategory()
            {
                Id = Guid.NewGuid().ToString(),
                Type = subCategoryDto.Type,
                CategoryId = subCategoryDto.CategoryId,
            };
            var result = await _subCategoriesRepository.AddSubCategory(newSubCategory);
            return result ? "Add Successful" : "Add failed";
        }

        public async Task<string> UpdateSubCategory(string  subCategoryId, UpdateSubCategoryDTO subCategoryDto)
        {
           var newSubCategory = await _subCategoriesRepository.GetSubCategoryById(subCategoryId);
            if(newSubCategory == null)
            {
                return "Sub category not found";
            }
            else
            {
                newSubCategory.Type = subCategoryDto.Type;
                var result = await _subCategoriesRepository.UpdateSubCategory(newSubCategory);
                return result ? "Update Successful" : "Update failed";
            }
        }

        public async Task<string> DeleteSubCategory(string subCategoryId)
        {
            SubCategory subCategory = await _subCategoriesRepository.GetSubCategoryById(subCategoryId);
            if(subCategory == null)
            {
                return "SubCategory not found";
            }
            await _subCategoriesRepository.DeleteSubCategory(subCategoryId);
            return "Detele Success";
        }
    }
}
