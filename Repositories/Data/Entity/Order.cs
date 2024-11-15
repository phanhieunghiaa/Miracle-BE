using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.Entity
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public float TotalPrice { get; set; }

        public User User { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
