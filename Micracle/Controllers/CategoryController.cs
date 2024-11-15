using Micracle.DTOs;
using Microsoft.AspNetCore.Mvc;
using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using Services.Interface;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _services;
        public CategoryController(ICategoryServices services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _services.GetAllCategories();
                if (categories == null)
                {
                    return NotFound();
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoriesById(string categoryId)
        {
            try
            {
                var category = await _services.GetCategoriesById(categoryId);

                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            if (category == null)
            {
                return BadRequest("Category data is null");
            }

            try
            {
                await _services.AddCategory(category);
                return Ok("New category has been added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(string categoryId, Category category)
        {
            if (categoryId == null)
            {
                return BadRequest("You need enter Id of category");
            }
            try
            {
                var result = await _services.Update(categoryId, category);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
