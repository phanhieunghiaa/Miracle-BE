using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IOrderProductServices
    {
        Task<List<OrderProduct>> GetAllOrderProducts();
        Task<OrderProduct> GetByOrderProductById(string orderProductId);
        Task<ServicesResponse<OrderProductReponse>> AddOrderProduct(string userId, string orderId);
        Task DeleteOrderProduct(string orderProductId);
        Task<string> UpdateOrderProduct(string orderproductId, OrderProductRequestDtos orderProductRequest);
        Task<List<OrderProduct>> GetListOrderProductByOrderId(string orderId);
    }
}
