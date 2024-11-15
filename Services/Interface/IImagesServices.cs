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
    public interface IImagesServices
    {
        Task<List<Image>> GetAllImages();
        Task<Image> GetImagesById(string id);
        Task<ServicesResponse<ImageResponseDto>> AddImages(string ImageUrl);
    }
}
