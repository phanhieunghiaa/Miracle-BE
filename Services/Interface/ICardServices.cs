using Micracle.DTOs;
using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ICardServices
    {
        Task<string> Update(string productId, string UserId, ProductRequestDtos product);
        Task<ServicesResponse<AddProductResponseDTO>> AddProduct(string UserId, ProductDTO productdto);
        Task<Product> GetProductById(string ProductsId);
        Task<List<Product>> GetAllProductSearch(string? searchterm);
        Task<string> Delete(string productId);
        Task<List<ProductDtos>> GetAllProduct();
        Task<List<ProductDtos>> GetProductsByIdsAsync(List<string> productIds);
        Task<List<Product>> GetListProductsById(List<string> ProductId);
        Task<string> UpdateQuantityProduct(string productId, Product productReponse);
    }
}
