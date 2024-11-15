using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IOrderServices
    {
        Task<List<Order>> GetAllOrders();
        Task<Order> GetOrderById(string orderId);
        Task<List<OrderProductDtos>> GetAllOrderProductBuUserId(string userId);
        Task<string> AddOrder(OrderDto orderDto);
        Task DeleteOrder(string orderId);
        Task<string> Update(string orderId, UpdateOrderDtos orderDto);
        Task UpdateStatus(string orderId, int newStatus);
        Task<Order> GetOrderByUserIdAsync(string userId);
        Task<List<Order>> GetListOrderByUserId(string userId);
    }
}
