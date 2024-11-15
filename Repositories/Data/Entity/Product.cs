using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.Entity
{
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
    }
}
