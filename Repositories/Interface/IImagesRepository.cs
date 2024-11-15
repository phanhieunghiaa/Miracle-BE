using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IImagesRepository
    {
        Task<List<Image>> GetAllImages();
        Task<Image> GetImageByid(string imagesId);
        Task AddImages(Image image);
    }
}
