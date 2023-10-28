using Core.Models.Image;
using ImageEntity = Core.Domain.Entities.Image;

namespace Core.Domain.RepositoryInterface
{
   public interface IImageRepository
   {
      Task<string> UploadImageFileToBlobStorageAsync(Stream streamContent, string filename, string blobContainerName);
      Task<string> DeleteImageFileFromBlobStorageAsync(string? imageUrl);
      Task<ImageEntity> CreateImageAsync(UploadImageRequest request, UploadImageResponse urlList, CancellationToken cancellationToken);
      Task<ImageEntity> GetImageByIdAsync(Guid id, CancellationToken cancellationToken);
      Task<ImageEntity> DeleteImageByIdAsync(Guid id, CancellationToken cancellationToken);
      Task<ImageEntity> UpdateImageByIdAsync(Guid id, UpdateImageRequest request, UploadImageResponse? urlList, CancellationToken cancellationToken);
   }
}