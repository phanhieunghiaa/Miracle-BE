using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.Entity
{
    public class Payment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Method { get; set; }
        public float Amount { get; set; }
        public string OrderId { get; set; }
        public int Status { get; set; }

        public Order Order { get; set; }
    }
}
