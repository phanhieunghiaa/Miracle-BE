using Microsoft.AspNetCore.Http;
using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IPaymentServices
    {
        Task<Payment> GetPaymentById(string paymnetId);
        Task<List<Payment>> GetAllPayment(string searchterm);
        Task<string> AddPayment(PaymentResponseDto paymentResponseDto);
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model, string userid);
        VnPaymentResponseModel PaymentExecute(Dictionary<string, string> url);
        public string GetUserId(string orderInfo);
        public string GetOrderId(string orderInfo);
        Task<List<Payment>> GetPaymentByUserId(string UserId);
        Task<string> AddPaymentPayOs(long method, string orderId);
    }
}
