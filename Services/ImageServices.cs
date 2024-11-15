using Repositories.Data.DTOs;
using Repositories.Data.Entity;
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
    public class ImageServices: IImagesServices
    {
        private readonly IImagesRepository _imagesRepository;

        public ImageServices(IImagesRepository imagesRepository)
        {
            _imagesRepository = imagesRepository;
        }

        public async Task<ServicesResponse<ImageResponseDto>> AddImages(string ImageUrl)
        {
            try
            {
                Image newImages = new Image()
                {
                    Id = Guid.NewGuid().ToString(),
                    Url = ImageUrl,
                };
                await _imagesRepository.AddImages(newImages);
                return ServicesResponse<ImageResponseDto>.SuccessResponse(new ImageResponseDto { ImageId = newImages.Id }); ;
            }catch(Exception ex)
            {
                return ServicesResponse<ImageResponseDto>.ErrorResponse($"An error occurred while adding the product: {ex.Message}");
            }
        }

        public async Task<List<Image>> GetAllImages()
        {
            return await _imagesRepository.GetAllImages();
        }

        public async Task<Image> GetImagesById(string id)
        {
            return await _imagesRepository.GetImageByid(id);
        }
    }
}
