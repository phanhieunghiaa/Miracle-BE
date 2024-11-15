using Micracle.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using Services.Interface;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardControllers : ControllerBase
    {
        private readonly ICardServices _services;
        public CardControllers(ICardServices services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var product = await _services.GetAllProduct();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost("id")]
        public async Task<IActionResult> GetProductById([FromQuery]List<string> productId)
        {
            try
            {
                var products = await _services.GetProductsByIdsAsync(productId);

                if (products == null)
                {
                    return NotFound();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddProducts(string UserId, ProductDTO productdto)
        {
            if (productdto == null)
            {
                return BadRequest("Product data is null");
            }
            try
            {
                var result = await _services.AddProduct(UserId, productdto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //Trả về lỗi nếu có exception
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(string productIds, string UserId, ProductRequestDtos requestDtos)
        {
            if (productIds == null)
            {
                return BadRequest("You need enter Id of product");
            }
            try
            {
                var result = await _services.Update(productIds, UserId, requestDtos);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(string productIds)
        {
            try
            {

                if (!string.IsNullOrEmpty(productIds))
                {
                    var result = await _services.Delete(productIds);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest("Product not found");

        }



    }
}
