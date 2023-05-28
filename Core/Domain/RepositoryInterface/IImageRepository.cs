using Microsoft.AspNetCore.Http;

namespace Core.Domain.RepositoryInterface
{
   public interface IImageRepository
   {
      Task UploadImageFileToBlobStorageAsync(IFormFile file, string filename);
   }
}