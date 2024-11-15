using Microsoft.EntityFrameworkCore;
using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface ICartProductRepository
    {
        Task<CartProduct> AddCartProductAsync(CartProduct cartProduct);
        Task<CartProduct> UpdateCartProductAsync(CartProduct cartProduct);
        Task<bool> RemoveCartProductAsync(CartProduct cartProduct);

    }

}
