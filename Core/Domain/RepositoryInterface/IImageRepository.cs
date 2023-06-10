using Core.Models.Image;

namespace Core.Domain.RepositoryInterface
{
   public interface IImageRepository
   {
      Task<string> UploadImageFileToBlobStorageAsync(Stream streamContent, string filename);
      Task<CreateImageResponse> CreateImageAsync(UploadImageRequest request, UploadImageResponse urlList, CancellationToken cancellationToken);
   }
}