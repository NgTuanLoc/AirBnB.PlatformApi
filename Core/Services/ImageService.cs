using Core.Domain.RepositoryInterface;
using Common.Models.Image;
using Core.Utils;

namespace Core.Services
{
   public interface IImageService
   {
      Task CreateImageAsync(UploadImageRequest request);
   }
   public class ImageService : IImageService
   {
      private readonly IImageRepository _imageRepository;
      public ImageService(IImageRepository imageRepository)
      {
         _imageRepository = imageRepository;
      }
      public async Task CreateImageAsync(UploadImageRequest request)
      {
         var fileName = $"{DateHelper.GetDateTimeNowString()}_{request.File.FileName.Replace(" ", "")}";
         await _imageRepository.UploadImageFileToBlobStorageAsync(request.File, fileName);
      }
   }
}