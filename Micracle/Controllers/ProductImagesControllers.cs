using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Data.Entity;
using Services.Interface;
using System.ComponentModel.DataAnnotations;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesControllers : ControllerBase
    {
        private readonly IProductImagesServices _services;
        private readonly ICardServices _cardServices;
        private readonly IImagesServices _imagesServices;

        public ProductImagesControllers(IProductImagesServices services, ICardServices cardServices, IImagesServices imagesServices)
        {
            _services = services;
            _cardServices = cardServices;
            _imagesServices = imagesServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductImages()
        {
            try
            {
                var results = await _services.GetAllProductImages();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SubcategoryId")]
        public async Task<IActionResult> GetAllProductImagesBySubCategory([Required]string SubcategoryId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SubcategoryId))
                {
                    return BadRequest("Please enter subcategoryId");
                }
                var results = await _services.GetAllProductbySubCate(SubcategoryId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("productImages")]
        public async Task<IActionResult> GetProductIamges( [Required] string id)
        {
            try
            {
                var result = await _services.GetProductImages(id);
                if (result == null)
                {
                    return NotFound("product not found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProductImages(string productId, string imagesId)
        {
            try
            {
                if (productId == null)
                {
                    return BadRequest("Please enter productId");
                }
                if (imagesId == null)
                {
                    return BadRequest("Please enter imageId");
                }
                var results = await _services.AddProductImages(productId, imagesId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
