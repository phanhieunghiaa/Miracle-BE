using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Data;
using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using Repositories.Interface;
using Services;
using Services.Interface;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;
        private readonly IUserServices _userService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IOrderServices _orderServices;
        private readonly IOrderProductServices _orderProductServices;
        private readonly ICardServices _cardServices;


        public PaymentController(IPaymentServices paymentServices, IUserServices userService, ApplicationDbContext dbContext, IOrderServices orderServices, IOrderProductServices orderProductServices, ICardServices cardServices)
        {
            _paymentServices = paymentServices;
            _userService = userService;
            _dbContext = dbContext;
            _orderServices = orderServices;
            _orderProductServices = orderProductServices;
            _cardServices = cardServices;
        }

        [HttpGet("userId")]
        public async Task<IActionResult> GetPaymentsByUserId(string userId)
        {
            var payments = await _paymentServices.GetPaymentByUserId(userId);
            if (payments == null || !payments.Any())
            {
                return NotFound("No payments found for the user.");
            }
            return Ok(payments);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayemnt(string? searchterm)
        {
            try
            {
                var results = await _paymentServices.GetAllPayment(searchterm);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("id")]
        public async Task<IActionResult> GetPaymentById(string paymentId)
        {
            try
            {
                var result = await _paymentServices.GetPaymentById(paymentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("payment/vnpay")]
        public async Task<IActionResult> AddPayment(string orderId, string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            var order = await _orderServices.GetOrderById(orderId);
            try
            {               
                    var vnPayModel = new VnPaymentRequestModel()
                    {
                        Amount = order.TotalPrice,
                        CreatedDate = DateTime.Now,
                        Description = "thanh toán VnPay",
                        OrderId = order.Id,
                        FullName = user.FullName,                      
                    };
                    if (vnPayModel.Amount < 0)
                    {
                        return BadRequest("The amount entered cannot be less than 0. Please try again");
                    }
                    var paymentUrl = _paymentServices.CreatePaymentUrl(HttpContext, vnPayModel, userId);
                    return Ok(new { url = paymentUrl });
                    //return Redirect(_vpnPayServices.CreatePaymentUrl(HttpContext, vnPayModel, userId));
                    //return new JsonResult(_vpnPayServices.CreatePaymentUrl(HttpContext, vnPayModel, userId));               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PaymentBack")]
        public async Task<IActionResult> PaymenCalltBack()
        {
            var queryParameters = HttpContext.Request.Query;
            // Kiểm tra và lấy giá trị 'vnp_OrderInfo' từ Query
            string orderInfo = queryParameters["vnp_OrderInfo"];
            string userId = _paymentServices.GetUserId(orderInfo);
            string orderId = _paymentServices.GetOrderId(orderInfo);
            double amount = double.Parse(queryParameters["vnp_Amount"]);
            if (string.IsNullOrEmpty(orderInfo))
            {
                return BadRequest("Thông tin đơn hàng không tồn tại.");
            }
            // Phân tích chuỗi 'orderInfo' để lấy các thông tin cần thiết
            var orderInfoDict = new Dictionary<string, string>();
            string[] pairs = orderInfo.Split(',');
            foreach (var pair in pairs)
            {
                string[] keyValue = pair.Split(':');
                if (keyValue.Length == 2)
                {
                    orderInfoDict[keyValue[0].Trim()] = keyValue[1].Trim();
                }
            }

            //Tạo và lưu trữ thông tin giao dịch
        var paymentDto = new PaymentResponseDto()
        {
            Id = Guid.NewGuid().ToString(),
            Status = 1,
            Amount = (float)amount / 100,  // Chia cho 100 nếu giá trị 'amount' là theo đơn vị nhỏ nhất của tiền tệ
            Method = "VnPay",
            OrderId = orderId
        };
            var result = await _paymentServices.AddPayment(paymentDto);         
            var listOrderProduct = await _orderProductServices.GetListOrderProductByOrderId(orderId);         
            foreach (var item in listOrderProduct)
            {
                var product = await _cardServices.GetProductById(item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;// Reduce the inventory quantity based on the order
                    await _cardServices.UpdateQuantityProduct(product.Id,product); // Update the product in the database
                }
            }

            if (result == "AddSuccessful")
            {                 
                return Redirect("http://localhost:5000/" /*+ userId*/); // thay đổi đường link
            }
            return BadRequest("Invalid transaction data.");
        }
    }
}
