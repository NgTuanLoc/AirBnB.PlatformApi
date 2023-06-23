using Core.Domain.RepositoryInterface;
using Core.Utils;
using Core.Models.Image;
using Microsoft.AspNetCore.Http;
using Core.Exceptions;
using ImageEntity = Core.Domain.Entities.Image;

namespace Core.Services
{
   public interface IImageService
   {
      Task<UploadImageResponse> UploadImageService(IFormFile? file);
      Task<CreateImageResponse> GetImageByIdService(Guid id, CancellationToken cancellationToken);
      Task<CreateImageResponse> DeleteImageByIdService(Guid id, CancellationToken cancellationToken);
      Task<CreateImageResponse> CreateImageService(UploadImageRequest request, CancellationToken cancellationToken);
      Task<CreateImageResponse> UpdateImageByIdService(Guid id, UpdateImageRequest request, CancellationToken cancellationToken);
   }
   public class ImageService : IImageService
   {
      private readonly IImageRepository _imageRepository;
      public ImageService(IImageRepository imageRepository)
      {
         _imageRepository = imageRepository;
      }
      public async Task<UploadImageResponse> UploadImageService(IFormFile? file)
      {
         if (file == null)
         {
            throw new ValidationException("File can not be null");
         }
         // Save Original Image
         var fileName = $"{DateHelper.GetDateTimeNowString()}_high_quality{file.FileName.Replace(" ", "")}";
         var streamContent = file.OpenReadStream();
         var highQualityUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(streamContent, fileName);

         // Save Medium Size Image
         var processedMediumQualityImageStream = ProcessedImageFactory.TransformToMediumQualityImageFromFile(file);
         var processedMediumQualityFileName = $"{DateHelper.GetDateTimeNowString()}_medium_quality_{file.FileName.Replace(" ", "")}";
         var mediumQualityUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(processedMediumQualityImageStream, processedMediumQualityFileName);

         // Save Small Size Image
         var processedImageStream = ProcessedImageFactory.TransformToLowQualityImageFromFile(file);
         var processedFileName = $"{DateHelper.GetDateTimeNowString()}_low_quality_{file.FileName.Replace(" ", "")}";
         var lowQualityUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(processedImageStream, processedFileName);

         return new UploadImageResponse()
         {
            highQualityUrl = highQualityUrl,
            mediumQualityUrl = mediumQualityUrl,
            lowQualityUrl = lowQualityUrl
         };
      }

      public async Task<CreateImageResponse> CreateImageService(UploadImageRequest request, CancellationToken cancellationToken)
      {
         var urlList = await UploadImageService(request.File);
         var result = await _imageRepository.CreateImageAsync(request, urlList, cancellationToken);
         return ConvertEntityIntoResponse.GetImageResponse(result);
      }

      public async Task<CreateImageResponse> UpdateImageByIdService(Guid id, UpdateImageRequest request, CancellationToken cancellationToken)
      {
         ImageEntity? result = null;
         if (request.File != null)
         {
            var urlList = await UploadImageService(request.File);
            result = await _imageRepository.UpdateImageByIdAsync(id, request, urlList, cancellationToken);
            return ConvertEntityIntoResponse.GetImageResponse(result);
         }
         result = await _imageRepository.UpdateImageByIdAsync(id, request, null, cancellationToken);
         return ConvertEntityIntoResponse.GetImageResponse(result);
      }

      public async Task<CreateImageResponse> DeleteImageByIdService(Guid id, CancellationToken cancellationToken)
      {
         var result = await _imageRepository.DeleteImageByIdAsync(id, cancellationToken);
         return ConvertEntityIntoResponse.GetImageResponse(result);
      }

      public async Task<CreateImageResponse> GetImageByIdService(Guid id, CancellationToken cancellationToken)
      {
         var result = await _imageRepository.GetImageByIdAsync(id, cancellationToken);
         return ConvertEntityIntoResponse.GetImageResponse(result);
      }
   }
}