using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.Entity
{
    public class CartProduct
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CartId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }

        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
