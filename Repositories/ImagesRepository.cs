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
    public class ImagesRepository : IImagesRepository
    {
        private readonly ApplicationDbContext _context;

        public ImagesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Image>> GetAllImages()
        {
            return await _context.Images.ToListAsync();
        }

        public async Task<Image> GetImageByid(string imagesId)
        {
            return await _context.Images.FirstOrDefaultAsync(sc => sc.Id.Equals(imagesId));
        }

        public async Task AddImages(Image image)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }
    }
}
