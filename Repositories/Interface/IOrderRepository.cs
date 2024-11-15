using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrder();
        Task<Order> GetOrderById(string orderId);
        Task<Order> GetOrderByUserId(string userId);
        Task<bool> AddOrder(Order order);
        Task<bool> UpdateOrder(Order order);
        Task DeleteOrder(string orderId);
        Task<Order> GetOrderByUserIdAsync(string userId);
        Task<List<Order>> GetListOrderByUserId(string userId);

    }
}
