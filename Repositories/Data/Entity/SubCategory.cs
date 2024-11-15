using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.Entity
{
    public class SubCategory
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string CategoryId { get; set; }

        public Category Category { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
