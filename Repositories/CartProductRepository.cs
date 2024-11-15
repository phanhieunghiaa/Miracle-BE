using Repositories.Data.Entity;
using Repositories.Data;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class CartProductRepository : ICartProductRepository
    {
        private readonly ApplicationDbContext _context;

        public CartProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<CartProduct> AddCartProductAsync(CartProduct cartProduct)
        {
            _context.CartProducts.Add(cartProduct);
            await _context.SaveChangesAsync();
            return cartProduct;
        }
        public async Task<CartProduct> UpdateCartProductAsync(CartProduct cartProduct)
        {
            _context.CartProducts.Update(cartProduct);
            await _context.SaveChangesAsync();
            return cartProduct;
        }
        public async Task<bool> RemoveCartProductAsync(CartProduct cartProduct)
        {
            _context.CartProducts.Remove(cartProduct);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task RemoveCartProducts(string cartId)
        {
            // Lấy danh sách CartProduct theo CartId
            var cartProducts = _context.CartProducts.Where(cp => cp.CartId == cartId).ToList();

            // Kiểm tra nếu không tìm thấy sản phẩm nào trong giỏ hàng
            if (cartProducts == null || !cartProducts.Any())
                return; // Không có sản phẩm nào để xóa

            // Xóa các sản phẩm trong giỏ hàng
            _context.CartProducts.RemoveRange(cartProducts);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }
    }

}
