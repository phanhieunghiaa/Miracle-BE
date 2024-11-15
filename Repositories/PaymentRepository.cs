using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Data.Entity;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PaymentRepository: IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> GetPaymentById(string paymnetId)
        {
            return await _context.Payments.FirstOrDefaultAsync(sc => sc.Id.Equals(paymnetId));  
        } 
        
        public async Task<List<Payment>> GetPaymentByUserId(string UserId)
        {
            return await _context.Payments.Where(sc => sc.Order.UserId.Equals(UserId)).ToListAsync();
        }





        public async Task<List<Payment>> GetAllPayment(string searchterm)
        {
            if (searchterm != null)
            {
                return await _context.Payments.Where(sc => sc.Method.Contains(searchterm)).ToListAsync();
            }
            else
            {
                return await _context.Payments.ToListAsync();
            }
        }

        public async Task<bool> AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
