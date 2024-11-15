using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Data.DTOs;
using Services;
using Services.Interface;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderControllers : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrderControllers(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrder()
        {
            try
            {
                var order = await _orderServices.GetAllOrders();
                if (order == null)
                {
                    return NotFound("order not found");
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        
        
        [HttpGet("ListOrder")]
        public async Task<IActionResult> GetAllOrderByUserId(string userId)
        {
            try
            {
                var order = await _orderServices.GetListOrderByUserId(userId);
                if (order == null)
                {
                    return NotFound("order not found");
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetOrderById(string? orderId)
        {
            try
            {
                if (string.IsNullOrEmpty(orderId))
                {
                    return BadRequest("Please enter your orderId");
                }
                var order = await _orderServices.GetOrderById(orderId);
                if (order == null)
                {
                    return NotFound("Order do not existed");
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Test 
        //[HttpGet("userId")]
        //public async Task<IActionResult> GetOrderByUserId(string UserId)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(UserId))
        //        {
        //            return BadRequest("Please enter your orderId");
        //        }
        //        var order = await _orderServices.GetOrderByUserIdAsync(UserId);
        //        if (order == null)
        //        {
        //            return NotFound("Order do not existed");
        //        }
        //        return Ok(order);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"{ex.Message}");
        //    }
        //}
        #endregion

        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(string orderId)
        {
            try
            {
                if (string.IsNullOrEmpty(orderId))
                {
                    return NotFound("order not found");
                }
                await _orderServices.DeleteOrder(orderId);
                return Ok("Delete Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
