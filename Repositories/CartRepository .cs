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
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartProducts)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartProducts(string cartId)
        {
            // Lấy danh sách CartProduct theo CartId
            var cartProducts = await _context.CartProducts.Where(cp => cp.CartId == cartId).ToListAsync();

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
