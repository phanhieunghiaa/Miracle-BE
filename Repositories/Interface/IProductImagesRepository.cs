using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IProductImagesRepository
    {
        Task<List<ProductImage>> GetAllImagesAsync();
        Task<ProductImage> GetByIdAsync(string productimageId);
        Task<List<ProductImage>> GetAllProductbySubCate(string subcategoryId);
        Task<bool> AddProductWithImages(ProductImage image);
    }
}
