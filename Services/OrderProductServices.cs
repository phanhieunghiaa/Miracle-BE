using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using Repositories.Interface;
using Services.Helpers;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderProductServices : IOrderProductServices
    {
        private readonly IOrderProductRepository _repository;
        private readonly ICardRepositories _cardRepositories;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;


        public OrderProductServices(IOrderProductRepository repository, ICardRepositories cardRepositories, IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _repository = repository;
            _cardRepositories = cardRepositories;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        public async Task<ServicesResponse<OrderProductReponse>> AddOrderProduct(string userId, string orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (order == null)
            {
                // tạo order khi không có order
                order = new Order()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    Status = 0, // order mới tạo
                    OrderProducts = new List<OrderProduct>(),
                    TotalPrice = 0,
                };
                await _orderRepository.AddOrder(order);
            }
            // Get product details using product IDs from the cart
            #region test
            //var productIds = cart.CartProducts.Select(cp => cp.ProductId).ToList();
            //var ListProducts = await _cardRepositories.GetListProductsById(productIds);
            #endregion
            // tạo order product dựa trên cartProduct
            var orderProduct = cart.CartProducts.Select(cartProduct => new OrderProduct
            {
           
                OrderId = order.Id,
                ProductId = cartProduct.ProductId,
                Quantity = cartProduct.Quantity,
                Price = cartProduct.Price,
            }).ToList();
            await _repository.AddListOrderProduct(orderProduct);

            var existingOrder = await _orderRepository.GetOrderById(order.Id);

            // kiểm tra Order có OrderProduct hay không
            if(orderProduct == null || !orderProduct.Any())
            {
                await _orderRepository.DeleteOrder(existingOrder.Id);
                return ServicesResponse<OrderProductReponse>.ErrorResponse("Cart dont have any product. Please add some Products");
            }

            // cập nhật tổng giá tiền khi có order Product
            existingOrder.TotalPrice = order.OrderProducts.Sum(product => product.Price);
            await _orderRepository.UpdateOrder(existingOrder);  
            
            // Xóa Cart product khi người dùng nhập order thành công
            await _cartRepository.RemoveCartProducts(cart.Id);
            #region test
            //foreach (var item in orderProduct)
            //{
            //    var product = ListProducts.FirstOrDefault(p => p.Id == item.ProductId);
            //    if (product != null)
            //    {
            //        product.Quantity -= item.Quantity;// Reduce the inventory quantity based on the order
            //        await _cardRepositories.UpdateProducts(product); // Update the product in the database
            //    }
            //}
            #endregion
            return ServicesResponse<OrderProductReponse>.SuccessResponse(new OrderProductReponse { OrderId = order.Id}); 

        }

        public async Task DeleteOrderProduct(string orderProductId)
        {
            await _repository.DeleteOrderProductById(orderProductId);
        }

        public async Task<List<OrderProduct>> GetAllOrderProducts()
        {
            return await _repository.GetAllOrderProduct();
        }

        public async Task<List<OrderProduct>> GetListOrderProductByOrderId(string orderId)
        {
            return await _repository.GetAllOrderProductByOrderId(orderId);
        }

        public async Task<OrderProduct> GetByOrderProductById(string orderProductId)
        {
            return await _repository.GetOrderProductById(orderProductId);
        }

        public async Task<string> UpdateOrderProduct(string orderproductId, OrderProductRequestDtos orderProductRequest)
        {
            var existingOrderProduct = await GetByOrderProductById(orderproductId);
            var card = await _cardRepositories.GetProductsById(existingOrderProduct.ProductId);
            if (existingOrderProduct != null)
            {
                return "order product not found";
            }
            if (orderProductRequest.Quantity > card.Quantity)
            {
                return "Not have enough quantity for this card";
            }
            if (card.Status == "Disable")
            {
                return "This product is not existed";
            }
            existingOrderProduct.Quantity = orderProductRequest.Quantity;
            existingOrderProduct.Price = card.Price * orderProductRequest.Quantity;

            var results = await _repository.UpdateOrderProduct(existingOrderProduct);
            return results ? "Update Success" : "Update failed";
        }
    }
}
