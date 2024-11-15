using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.DTOs
{
    public class PaymentResponseDto
    {
        public string Id { get; set; }
        public string Method { get; set; }
        public float Amount { get; set; }
        public string OrderId { get; set; }
        public int Status { get; set; }
    }
}
