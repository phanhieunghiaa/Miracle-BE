using Repositories.Data.Entity;
using Repositories.Enums;

namespace Micracle.DTOs
{
    public class ProductDTO
    {      
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string SubCategoryId { get; set; }
    }
}
