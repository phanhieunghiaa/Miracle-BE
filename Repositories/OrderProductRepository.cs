using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Data.Entity;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class OrderProductRepository: IOrderProductRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddOrderProduct(OrderProduct orderProduct)
        {
            _context.OrderProducts.Add(orderProduct);
           return  await _context.SaveChangesAsync() > 0;
        }

        public async Task AddListOrderProduct(IEnumerable<OrderProduct> orderProducts)
        {
            if(orderProducts == null)
            {
                throw new ArgumentException("OrderProducts list cannot be null or empty");
            }
             _context.OrderProducts.AddRange(orderProducts);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderProductById(string orderId)
        {
           var orderProduct = await GetOrderProductById(orderId);
            if(orderProduct != null)
            {
                _context.OrderProducts.Remove(orderProduct);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<OrderProduct>> GetAllOrderProductByOrderId(string orderId)
        {
            return await _context.OrderProducts.Where(sc => sc.OrderId.Equals(orderId)).ToListAsync();
        }

        public async Task<List<OrderProduct>> GetAllOrderProduct()
        {
           return await _context.OrderProducts.ToListAsync();
        }

        public async Task<OrderProduct> GetOrderProductById(string orderId)
        {
            return await _context.OrderProducts.FirstOrDefaultAsync(sc => sc.Id.Equals(orderId));
        }

        public async Task<bool> UpdateOrderProduct(OrderProduct orderProduct)
        {
           _context.OrderProducts.Update(orderProduct);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
