using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.Entity
{
    public class ProductImage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProductId { get; set; }
        public string ImageId { get; set; }

        public Product Product { get; set; }
        public Image Image { get; set; }
    }
}
