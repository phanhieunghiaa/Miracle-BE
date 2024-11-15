using Microsoft.EntityFrameworkCore;
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
    public class CartProductService : ICartProductService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartProductRepository _cartProductRepository;
        private readonly ICardRepositories _cardRepositories;


        public CartProductService(ICartRepository cartRepository, ICartProductRepository cartProductRepository, ICardRepositories cardRepositories)
        {
            _cartRepository = cartRepository;
            _cartProductRepository = cartProductRepository;
            _cardRepositories = cardRepositories;
        }

        public async Task<CartProduct> AddCartProductAsync(string userId, AddCartProductDTO addCartProductDTO)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            // If the cart doesn't exist, create a new one
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartProducts = new List<CartProduct>()
                };
                await _cartRepository.AddCartAsync(cart);
            }
            // Tìm CartProduct tồn tại trong cart
            var existingCartProduct = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == addCartProductDTO.ProductId);
            Product findProductByProductId = await _cardRepositories.GetProductsById(addCartProductDTO.ProductId);
            if (existingCartProduct != null)
            {
                existingCartProduct.Quantity += addCartProductDTO.Quantity;
                await _cartProductRepository.UpdateCartProductAsync(existingCartProduct); // Cập nhật ghi đè
                return existingCartProduct; // Trả về sản phẩm đã cập nhật
            }

            // Nếu sản phẩm chưa tồn tại, map AddCartProductDTO sang CartProduct
            var cartProduct = new CartProduct
            {
                CartId = cart.Id,
                ProductId = addCartProductDTO.ProductId,
                Quantity = addCartProductDTO.Quantity,
                Price = findProductByProductId.Price * addCartProductDTO.Quantity
            };

            // Thêm sản phẩm vào giỏ hàng
            return await _cartProductRepository.AddCartProductAsync(cartProduct);
            
        }

        public async Task<bool> RemoveCartProductAsync(string userId, string productId)
        {
            
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            var existcard = await _cardRepositories.GetProductsById(productId);
            
            if (cart == null)
            {
                throw new Exception("Cart not found for the specified user.");
            }

            // Tìm sản phẩm trong giỏ hàng
            var cartProduct = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);

            
            if (cartProduct == null)
            {
                throw new Exception("Product not found in the cart.");
            }

            // Gọi repository để xóa sản phẩm khỏi giỏ hàng
            return await _cartProductRepository.RemoveCartProductAsync(cartProduct);
        }

    }

}
