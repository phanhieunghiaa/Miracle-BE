using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.Entity
{
    public class OrderProduct
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Sử dụng Guid cho key
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }

}
