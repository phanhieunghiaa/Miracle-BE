using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Data.DTOs;
using Services;
using Services.Interface;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICartProductService _cartProductService;


        public CartController(ICartService cartService, ICartProductService cartProductService)
        {
            _cartService = cartService;
            _cartProductService = cartProductService;
        }

        // GET api/cart/user/{userId}/cart
        [HttpGet("user/{userId}/cart")]
        public async Task<ActionResult<List<AddCartProductDTO>>> GetCartProductsByUserId(string userId)
        {
            try
            {
                // Gọi tới service để lấy các sản phẩm trong giỏ hàng
                var cartProducts = await _cartService.GetCartProductsByUserIdAsync(userId);
                return Ok(cartProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("user/{userId}/cart/product/{productId}")]
        public async Task<ActionResult> RemoveCartProduct(string userId, string productId)
        {
            try
            {
                await _cartProductService.RemoveCartProductAsync(userId, productId);
                return NoContent(); // Trả về 204 No Content nếu xóa thành công
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

    }
}

