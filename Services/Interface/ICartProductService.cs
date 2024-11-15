using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ICartProductService
    {
        Task<CartProduct> AddCartProductAsync(string userId, AddCartProductDTO addCartProductDTO);
        Task<bool> RemoveCartProductAsync(string userId, string productId);
    }

}
