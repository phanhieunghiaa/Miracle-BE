using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.DTOs
{
    public class AddCartProductDTO
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
