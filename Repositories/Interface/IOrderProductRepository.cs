using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IOrderProductRepository
    {
        Task<List<OrderProduct>> GetAllOrderProduct();
        Task<OrderProduct> GetOrderProductById(string orderId);
        Task<bool> UpdateOrderProduct(OrderProduct orderProduct);
        Task DeleteOrderProductById(string orderId);
        Task<bool> AddOrderProduct(OrderProduct orderProduct);
        Task AddListOrderProduct(IEnumerable<OrderProduct> orderProducts);
        Task<List<OrderProduct>> GetAllOrderProductByOrderId(string orderId);
    }
}
