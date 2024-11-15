using Micracle.DTOs;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Data;
using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using Repositories.Enums;
using Repositories.Interface;
using Services.Helpers;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CardServices : ICardServices
    {
        private readonly ICardRepositories _repositories;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;

        public CardServices(ICardRepositories repositories, IUserRepository userRepository, ApplicationDbContext dbContext)
        {
            _repositories = repositories;
            _userRepository = userRepository;
            _context = dbContext;
        }

        public async Task<List<Product>> GetAllProductSearch(string? searchterm)
        {
            return await _repositories.GetAllProductsSearch(searchterm);
        }

        public async Task<Product> GetProductById(string ProductsId)
        {
            return await _repositories.GetProductsById(ProductsId);
        }

        public async Task<List<Product>> GetListProductsById(List<string> ProductId)
        {
            return await _repositories.GetListProductsById(ProductId);
        }

        public async Task<List<ProductDtos>> GetAllProduct()
        {
            var products = await _context.Products.Select(p => new ProductDtos
            {
                Id = p.Id,  
                ProductName = p.ProductName,
                Description = p.Description,
                Quantity = p.Quantity,
                Price = p.Price,
                Status = p.Status,
                CreatedDate = p.CreatedDate,
                UpdatedDate = p.UpdatedDate,
                CreatedBy = p.CreatedBy,
                UpdatedBy = p.UpdatedBy,
                // Giả sử lấy ảnh đầu tiên nếu có
                ImageId = p.ProductImages.FirstOrDefault() != null ? p.ProductImages.FirstOrDefault().Id : null,
                Url = p.ProductImages.FirstOrDefault() != null ? p.ProductImages.FirstOrDefault().Image.Url : null,
            }).ToListAsync();

            return products;
        }

        public async Task<List<ProductDtos>> GetProductsByIdsAsync(List<string> productIds)
        {
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new ProductDtos
                {
                    Id = p.Id,                  
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    Status = p.Status,
                    CreatedDate = p.CreatedDate,
                    UpdatedDate = p.UpdatedDate,
                    CreatedBy = p.CreatedBy,
                    UpdatedBy = p.UpdatedBy,
                    // Giả sử lấy ảnh đầu tiên nếu có
                    ImageId = p.ProductImages.FirstOrDefault() != null ? p.ProductImages.FirstOrDefault().Id : null,
                    Url = p.ProductImages.FirstOrDefault() != null ? p.ProductImages.FirstOrDefault().Image.Url : null,

                })
                .ToListAsync();

            return products;
        }

        public async Task<ServicesResponse<AddProductResponseDTO>> AddProduct(string UserId, ProductDTO productdto)
        {
            try
            {
                var user = await _userRepository.GetUserById(UserId);
                if (user == null)
                {
                    return ServicesResponse<AddProductResponseDTO>.ErrorResponse("User Not found");
                }
                if (productdto == null)
                {
                    return ServicesResponse<AddProductResponseDTO>.ErrorResponse("Products do not existed");
                }
                if (string.IsNullOrWhiteSpace(productdto.ProductName) || productdto.ProductName == "string")
                {
                    return ServicesResponse<AddProductResponseDTO>.ErrorResponse("Products must be required");
                }
                if (productdto.Quantity < 0)
                {
                    return ServicesResponse<AddProductResponseDTO>.ErrorResponse("Quantity cannot be less than 0");
                }
                Product newProduct = new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductName = productdto.ProductName,
                    Quantity = productdto.Quantity,
                    Price = productdto.Price,
                    Description = productdto.Description,
                    Status = ProductStatus.Active.ToString(),
                    CreatedBy = user.FullName,
                    UpdatedBy = null,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = null,
                    SubCategoryId = productdto.SubCategoryId,
                };
                await _repositories.AddProducts(newProduct);
                return ServicesResponse<AddProductResponseDTO>.SuccessResponse(new AddProductResponseDTO { ProductId = newProduct.Id });
            }
            catch (Exception ex)
            {
                return ServicesResponse<AddProductResponseDTO>.ErrorResponse($"An error occurred while adding the product: {ex.Message}");
            }
        }

        public async Task<string> Update(string productId, string UserId, ProductRequestDtos product)
        {
            var user = await _userRepository.GetUserById(UserId);
            var existingCard = await _repositories.GetProductsById(productId);
            if (user == null)
            {
                return "User Not found";
            }
            else if (existingCard == null)
            {
                return "Card not found";
            }
            else if (product.Quantity < 0)
            {
                return "Quantity cannot be less than 0";
            }
            else
            {
                existingCard.ProductName = product.ProductName;
                existingCard.Quantity = product.Quantity;
                existingCard.Price = product.Price;
                existingCard.Description = product.Description;
                existingCard.UpdatedDate = DateTime.Now;
                existingCard.UpdatedBy = user.FullName;
                if (product.Quantity == 0)
                {
                    existingCard.Status = ProductStatus.Disable.ToString();
                }
                else
                {
                    existingCard.Status = ProductStatus.Active.ToString();
                }
                var result = await _repositories.UpdateProducts(existingCard);
                return result ? "Update Successful" : "Update failed";
            }

        }

        public async Task<string> UpdateQuantityProduct(string productId, Product productReponse)
        {
            var products =await _repositories.GetProductsById(productId);
            if(products == null)
            {
                return "product not found";
            }
            products.Quantity = productReponse.Quantity;
            var result = await _repositories.UpdateProducts(productReponse);
            return result ? "Update Successful" : "Update failed";

        }
        public async Task<string> Delete(string productId)
        {
            Product card = await _repositories.GetProductsById(productId);
            if (card == null)
            {
                return "Card not found";
            }
            await _repositories.DeleteProducts(productId);
            return "Delete success";
        }

    }
}
