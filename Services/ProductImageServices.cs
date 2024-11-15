using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = Repositories.Data.Entity.Image;

namespace Services
{
    public class ProductImageServices : IProductImagesServices
    {
        private readonly IProductImagesRepository _productsImagesRepository;
        private readonly ICardServices _cardsRepository;
        private readonly IImagesRepository _imagesRepository;
        private readonly ISubCategoriesRepository _subCategoriesRepository;
        private readonly ICategoryRepositories _categoryRepositories;
        private readonly ICartProductRepository _cartProductRepository;

        public ProductImageServices(IProductImagesRepository productsRepository, ICardServices cardServices, IImagesRepository imagesRepository, ISubCategoriesRepository subCategoriesRepository, ICategoryRepositories categoryRepositories, ICartProductRepository cartProductRepository)
        {
            _productsImagesRepository = productsRepository;
            _cardsRepository = cardServices;
            _imagesRepository = imagesRepository;
            _subCategoriesRepository = subCategoriesRepository;
            _categoryRepositories = categoryRepositories;
            _cartProductRepository = cartProductRepository;
        }

        public async Task<string> AddProductImages(string productId, string imageId)
        {
            var product = await _cardsRepository.GetProductById(productId);
            var images = await _imagesRepository.GetImageByid(imageId);
            if (product == null)
            {
                return "Product not found ";
            }
            if (images == null)
            {
                return "Images not found";
            }
            ProductImage productImage = new ProductImage()
            {
                Id = Guid.NewGuid().ToString(),
                ImageId = imageId,
                ProductId = productId,
            };
            var result = await _productsImagesRepository.AddProductWithImages(productImage);
            return result ? "Add Successful" : "Add failed";
        }

        public async Task<List<ProductImagesResponse>> GetAllProductImages()
        {
            List<ProductImagesResponse> productImagesResponses = new List<ProductImagesResponse>();
            var ProductImages = await _productsImagesRepository.GetAllImagesAsync();
            foreach (var item in ProductImages)
            {
                var product = await _cardsRepository.GetProductById(item.ProductId);
                var image = await _imagesRepository.GetImageByid(item.ImageId);
                var subcategory = await _subCategoriesRepository.GetSubCategoryById(product.SubCategoryId);
                var category = await _categoryRepositories.GetCategoriesById(subcategory.CategoryId);
                if (product.Quantity > 0)
                {
                    var newProductImages = new ProductImagesResponse()
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ImageId = item.ImageId,
                        ProductName = product.ProductName,
                        Description = product.Description,
                        Quantity = product.Quantity,
                        Price = product.Price,
                        Status = product.Status,
                        CreatedDate = product.CreatedDate,
                        UpdatedDate = product.UpdatedDate,
                        CreatedBy = product.CreatedBy,
                        UpdatedBy = product.UpdatedBy,
                        Url = image.Url,
                        Type = subcategory.Type,
                        Brand = category.Brand,
                    };
                    productImagesResponses.Add(newProductImages);
                }
            }
            return productImagesResponses;
        }

        public async Task<List<ProductImagesResponse>> GetAllProductbySubCate(string subcategoryId)
        {
            List<ProductImagesResponse> productImagesResponses = new List<ProductImagesResponse>();
            var ProductImages = await _productsImagesRepository.GetAllProductbySubCate(subcategoryId);
            {
                foreach (var item in ProductImages)
                {
                    var product = await _cardsRepository.GetProductById(item.ProductId);
                    var image = await _imagesRepository.GetImageByid(item.ImageId);
                    var subcategory = await _subCategoriesRepository.GetSubCategoryById(product.SubCategoryId);
                    var category = await _categoryRepositories.GetCategoriesById(subcategory.CategoryId);
                    var newProductImages = new ProductImagesResponse()
                    {

                        Id = item.Id,
                        ProductId = item.ProductId,
                        ImageId = item.ImageId,
                        ProductName = product.ProductName,
                        Description = product.Description,
                        Quantity = product.Quantity,
                        Price = product.Price,
                        Status = product.Status,
                        CreatedDate = product.CreatedDate,
                        UpdatedDate = product.UpdatedDate,
                        CreatedBy = product.CreatedBy,
                        UpdatedBy = product.UpdatedBy,
                        Url = image.Url,
                        Type = subcategory.Type,
                        Brand = category.Brand,
                    };
                    productImagesResponses.Add(newProductImages);
                }
                return productImagesResponses;
            }
        }

        public async Task<ProductImagesResponse> GetProductImages(string productImageId)
        {
            var productImage = await _productsImagesRepository.GetByIdAsync(productImageId);
            Product product = await _cardsRepository.GetProductById(productImage.ProductId);
            Image image = await _imagesRepository.GetImageByid(productImage.ImageId);
            SubCategory subCategory = await _subCategoriesRepository.GetSubCategoryById(product.SubCategoryId);
            Category category = await _categoryRepositories.GetCategoriesById(subCategory.CategoryId);

            if (productImage == null)
            {
                throw new Exception("Product with images not found");
            }
            if (image == null)
            {
                throw new Exception("Imgaes not found");
            }
            if (product == null)
            {
                throw new Exception("product not found");
            }
            if (subCategory == null)
            {
                throw new Exception("Subcategory not found");
            }
            var newPeoductImage = new ProductImagesResponse()
            {
                Id = productImage.Id,
                ProductId = productImage.ProductId,
                ImageId = productImage.ImageId,
                ProductName = product.ProductName,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price,
                Status = product.Status,
                CreatedDate = product.CreatedDate,
                UpdatedDate = product.UpdatedDate,
                CreatedBy = product.CreatedBy,
                UpdatedBy = product.UpdatedBy,
                Url = image.Url,
                Type = subCategory.Type,
                Brand = category.Brand,
            };
            return newPeoductImage;

        }
    }
}
