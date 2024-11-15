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
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _repository;
        private readonly IOrderProductRepository _orderProductRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        public OrderServices(IOrderRepository repository, ICartRepository cartRepository , IOrderProductRepository orderProductRepository, IUserRepository userRepository)
        {
            _repository = repository;
            _orderProductRepository = orderProductRepository;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
        }

        public async Task<string> AddOrder(OrderDto orderDto)
        {
            if (orderDto == null)
            {
                return "Data null";
            }
            Order order = new Order()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = orderDto.UserId,
                OrderDate = DateTime.Now,
                Status = orderDto.Status,
                TotalPrice = 0,
            };
            var orderProduct = await _orderProductRepository.GetAllOrderProductByOrderId(order.Id);
            order.TotalPrice = orderProduct.Sum(op => op.Price * op.Quantity);
            var result = await _repository.AddOrder(order);
            return result ? "Add Suucess" : "Add failed";
        }      

        public async Task<List<OrderProductDtos>> GetAllOrderProductBuUserId(string userId)
        {
            var order = await  _repository.GetOrderByUserId(userId);
            if(order == null)
            {
                throw new Exception(" order not found");
            }
            var orderProduct = order.OrderProducts.Select(p => new OrderProductDtos()
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                Price = p.Price,
            }).ToList();

            return orderProduct;
        }
        public async Task DeleteOrder(string orderId)
        {
            await _repository.DeleteOrder(orderId);
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _repository.GetAllOrder();
        }
        public async Task<Order> GetOrderById(string orderId)
        {
            return await _repository.GetOrderById(orderId);
        }

        public async Task<string> Update(string orderId, UpdateOrderDtos orderDto)
        {
            var existingOrder = await _repository.GetOrderById(orderId);
            var listOrderProduct = await _orderProductRepository.GetAllOrderProductByOrderId(orderId);
            
            if (existingOrder == null)
            {
                return "Order not found";
            }
            existingOrder.OrderDate = DateTime.Now;
            existingOrder.Status = orderDto.Status;
            existingOrder.TotalPrice = listOrderProduct.Sum(sc => sc.Price * sc.Quantity);

            var result = await _repository.UpdateOrder(existingOrder);
            return result ? "Update Success" : "update failed";
        }

        public async Task UpdateStatus(string orderId, int newStatus)
        {
            var existingOrder = await _repository.GetOrderById(orderId);
            if (existingOrder == null)
            {
                throw new Exception( "Order not found");
            }
            switch (existingOrder.Status)
            {
                case 0:
                    if (newStatus == 1 || newStatus == 2)
                    {
                        existingOrder.Status = newStatus; // Cho phép chuyển từ 0 sang 1 hoặc 2
                                                          // chuyển từ chờ thanh toán sang thàng công hay thất bại
                    }
                    else
                    {
                        throw new Exception("Invalid status transition.");
                    }
                    break;
                case 1:
                    if (newStatus == 2)
                    {
                        existingOrder.Status = newStatus; // Chuyển từ 1 sang 2
                    }
                    else
                    {
                        throw new Exception("Invalid status transition.");
                    }
                    break;
                case 2:
                    throw new Exception("Order is already completed. No further updates allowed.");
                default:
                    throw new Exception("Invalid current status.");
            }
            await _repository.UpdateOrder(existingOrder);

        }

        public async Task<Order> GetOrderByUserIdAsync(string userId)
        {
            return await _repository.GetOrderByUserIdAsync(userId);
        }

        public async Task<List<Order>> GetListOrderByUserId(string userId)
        {
            return await _repository.GetListOrderByUserId(userId);
        }
    }
}
