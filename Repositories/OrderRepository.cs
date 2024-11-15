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
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrder()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderById(string orderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(sc => sc.Id.Equals(orderId));
        }
        public async Task<Order> GetOrderByUserId(string userId)
        {
            return await _context.Orders.FirstOrDefaultAsync(sc => sc.UserId.Equals(userId));
        }
        public async Task<List<Order>> GetListOrderByUserId(string userId)
        {
            return await _context.Orders.Where(sc => sc.UserId.Equals(userId)).ToListAsync();
        }

        public async Task<Order> GetOrderByUserIdAsync(string userId)
        {
            return await _context.Orders.Where(order => order.UserId == userId)
                .OrderByDescending(order => order.OrderDate)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> AddOrder (Order order)
        {
            _context.Orders.Add(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
           return await _context.SaveChangesAsync() > 0;
        }

        public async Task DeleteOrder(string orderId)
        {
            Order order = await GetOrderById(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
