using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Data.DTOs;
using Services.Interface;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartProductController : ControllerBase
    {
        private readonly ICartProductService _cartProductService;

        public CartProductController(ICartProductService cartProductService)
        {
            _cartProductService = cartProductService;
        }

        [HttpPost("user/{userId}")]
        public async Task<ActionResult> AddCartProduct(string userId, [FromBody] AddCartProductDTO addCartProductDTO)
        {
            try
            {
                await _cartProductService.AddCartProductAsync(userId, addCartProductDTO);
                return Ok("Product added to cart successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
