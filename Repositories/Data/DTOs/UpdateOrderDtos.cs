using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.DTOs
{
    public class UpdateOrderDtos
    {
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public float TotalPrice { get; set; }
    }
}
