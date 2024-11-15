using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.Entity
{
    public class Image
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Url { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }
    }
}
