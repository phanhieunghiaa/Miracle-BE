using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Repositories.Data.DTOs;
using Services.Interface;
using Repositories.Data.Entity;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly string _projectId = "miracles-ef238";
        private readonly string _bucketName = "miracles-ef238.appspot.com";
        private readonly ICardServices _cardServices;
        private readonly IImagesServices _imagesServices;
        private readonly IProductImagesServices _productImagesServices;

        public ImagesController(ICardServices cardServices, IImagesServices imagesServices, IProductImagesServices productImagesServices)
        {
            _cardServices = cardServices;
            _imagesServices = imagesServices;
            _productImagesServices = productImagesServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            try
            {
                var result = await _imagesServices.GetAllImages();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost("uploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadViewModel request)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                {
                    return BadRequest("File không hợp lệ");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await request.File.CopyToAsync(memoryStream);
                    var bytes = memoryStream.ToArray();

                    // Initialize Firebase Admin SDK
                    var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile("miracles-ef238-firebase-adminsdk-mm7s5-0c76f3bec8.json");
                    //tạm thời hardcode, thay sau FromFile thành đường dẫn file net1701-jewelry...
                    var storage = StorageClient.Create(credential);

                    // Construct the object name (path) in Firebase Storage
                    var objectName = $"images/{DateTime.Now.Ticks}_{request.File.FileName}";

                    // Upload the file to Firebase Storage
                    var response = await storage.UploadObjectAsync(
                        bucket: _bucketName,
                        objectName: objectName,
                        contentType: request.File.ContentType,
                        source: new MemoryStream(bytes)
                    );
                    // Tải file lên Firebase Storage
                    //var storageObject = await storage.GetObjectAsync(_bucketName, objectName);
                    var downloadUrl = /*storageObject.MediaLink*/ $"https://storage.googleapis.com/{_bucketName}/{objectName}";

                    var result = await _imagesServices.AddImages(downloadUrl);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}");
            }
        }
    }
}


