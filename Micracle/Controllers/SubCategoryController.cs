using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using Repositories.Interface;
using Services.Interface;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryServices _services;
        private readonly ICategoryServices _categoryServices;

        public SubCategoryController(ISubCategoryServices services, ICategoryServices categoryServices)
        {
            _services = services;
            _categoryServices = categoryServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubCategory()
        {
            var subcategory = await _services.GetAllSubCategory();
            if (subcategory == null)
            {
                return NotFound();
            }
            return Ok(subcategory);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetSubCategoryById(string SubCategoryId)
        {
            var subCategory = await _services.GetSubCategoryById(SubCategoryId);
            if (subCategory == null)
            {
                return NotFound();
            }
            return Ok(subCategory);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubCategory(AddSubCategoryDto subCategorydto)
        {
            if (subCategorydto == null)
            {
                return BadRequest("Data null");
            }
            else
            {
                var result = await _services.AddSubCategory(subCategorydto);
                return Ok(result);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSubCategory(string subCategoryId, UpdateSubCategoryDTO subCategorydto)
        {
            var subcategories = await _services.GetSubCategoryById(subCategoryId);
            if (subcategories == null)
            {
                return BadRequest("Dont have any subcategory");
            }
            else
            {
                var result = await _services.UpdateSubCategory(subCategoryId, subCategorydto);
                return Ok(result);
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSubCategory(string subCategoryId)
        {
            var subcategory = await _services.GetSubCategoryById(subCategoryId);
            if (subcategory == null)
            {
                return NotFound();
            }
            else
            {
                await _services.DeleteSubCategory(subCategoryId);
                return Ok("Delete Success");
            }
        }
    }
}
