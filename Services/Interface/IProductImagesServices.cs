using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IProductImagesServices
    {
        Task<List<ProductImagesResponse>> GetAllProductImages();
        Task<List<ProductImagesResponse>> GetAllProductbySubCate(string subcategoryId);
        //Task<ProductImage> GetProductImageById(string id);
        Task<string> AddProductImages(string productId, string imageId);
        Task<ProductImagesResponse> GetProductImages(string productImageId);
       
    }
}
