using Core.Models.Image;

namespace Core.Domain.RepositoryInterface
{
   public interface IImageRepository
   {
      Task<string> UploadImageFileToBlobStorageAsync(Stream streamContent, string filename);
      Task<string> DeleteImageFileFromBlobStorageAsync(string? imageUrl);
      Task<CreateImageResponse> CreateImageAsync(UploadImageRequest request, UploadImageResponse urlList, CancellationToken cancellationToken);
      Task<CreateImageResponse> GetImageByIdAsync(Guid id, CancellationToken cancellationToken);
      Task<CreateImageResponse> DeleteImageByIdAsync(Guid id, CancellationToken cancellationToken);
      Task<CreateImageResponse> UpdateImageByIdAsync(Guid id, UpdateImageRequest request, UploadImageResponse? urlList, CancellationToken cancellationToken);
   }
}