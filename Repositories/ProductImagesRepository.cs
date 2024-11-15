using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Data.Entity;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProductImagesRepository: IProductImagesRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductImagesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProductWithImages(ProductImage image)
        {
            _context.ProductImages.Add(image);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ProductImage>> GetAllImagesAsync()
        {
           return await _context.ProductImages.ToListAsync();
        }
        public async Task<List<ProductImage>> GetAllProductbySubCate(string subcategoryId)
        {
            return await _context.ProductImages.Where(sc => sc.Product.SubCategoryId.Equals(subcategoryId)).ToListAsync();
        }

        public async Task<ProductImage> GetByIdAsync(string productimageId)
        {
            return await _context.ProductImages.FirstOrDefaultAsync(sc => sc.Id.Equals(productimageId));
        }
    }
}
