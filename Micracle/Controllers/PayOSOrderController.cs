using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using Repositories.Data.DTOs.PayOSDTO;
using Response = Repositories.Data.DTOs.PayOSDTO.Response;
using Services.Interface;
using Repositories.Data.DTOs;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayOSOrderController : ControllerBase
    {
        private readonly PayOS _payOS;
        private readonly IPaymentServices _paymentServices;
        private readonly IOrderServices _orderServices;
        private readonly IUserServices _userServices;
        public PayOSOrderController(PayOS payOS, IPaymentServices paymentServices, IOrderServices orderServices, IUserServices userServices)
        {
            _payOS = payOS;
            _paymentServices = paymentServices;
            _orderServices = orderServices;
            _userServices = userServices;
        }

        [HttpPost("create")]
        //public async Task<IActionResult> CreatePaymentLink(CreatePaymentLinkRequest body)
        public async Task<IActionResult> CreatePaymentLink([FromQuery] string orderId, [FromQuery] string userId, PayOSUrl body)
        {
            try
            {

                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                var user = await _userServices.GetUserByIdAsync(userId);
                var order = await _orderServices.GetOrderById(orderId);
                ItemData item = new ItemData("OrderId" + order.Id, 1, (int)Math.Ceiling(order.TotalPrice));
                List<ItemData> items = new List<ItemData>();
                items.Add(item);
                PaymentData paymentData = new PaymentData(orderCode, (int)Math.Ceiling(order.TotalPrice), "Miracle", items, body.CancelUrl, body.ReturnUrl);

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                await _paymentServices.AddPaymentPayOs(createPayment.orderCode, order.Id);
                return Ok(new Response(0, "success", createPayment));
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder([FromRoute] int orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
                return Ok(new Response(0, "Ok", paymentLinkInformation));
            }
            catch (System.Exception exception)
            {

                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }

        }
        [HttpPut("{orderId}")]
        public async Task<IActionResult> CancelOrder([FromRoute] int orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.cancelPaymentLink(orderId);
                return Ok(new Response(0, "Ok", paymentLinkInformation));
            }
            catch (System.Exception exception)
            {

                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }

        }
        [HttpPost("confirm-webhook")]
        public async Task<IActionResult> ConfirmWebhook(ConfirmWebhook body)
        {
            try
            {
                await _payOS.confirmWebhook(body.webhook_url);
                return Ok(new Response(0, "Ok", null));
            }
            catch (System.Exception exception)
            {

                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }

        }
    }
}
