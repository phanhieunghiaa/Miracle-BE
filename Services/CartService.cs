using Repositories.Data.DTOs;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<List<CartProductDTO>> GetCartProductsByUserIdAsync(string userId)
        {
            // Lấy giỏ hàng của user dựa trên userId
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            // Nếu không có giỏ hàng thì trả về rỗng
            if (cart == null)
            {
                throw new Exception("Cart not found for the specified user.");
            }

            // Lấy danh sách các sản phẩm từ giỏ hàng
            var cartProducts = cart.CartProducts.Select(cp => new CartProductDTO
            {
                ProductId = cp.ProductId,
                Quantity = cp.Quantity,
                Price = cp.Price
            }).ToList();

            return cartProducts;
        }
    }
}
