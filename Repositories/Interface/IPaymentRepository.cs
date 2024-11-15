using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IPaymentRepository
    {
        Task<Payment> GetPaymentById(string paymnetId);
        Task<List<Payment>> GetAllPayment(string searchterm);
        Task<bool> AddPayment(Payment payment);
        Task<List<Payment>> GetPaymentByUserId(string UserId);
    }
}
