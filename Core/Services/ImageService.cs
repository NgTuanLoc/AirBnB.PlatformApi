using Core.Domain.RepositoryInterface;
using Core.Utils;
using Core.Models.Image;
using Microsoft.AspNetCore.Http;
using Core.Exceptions;
using ImageEntity = Core.Domain.Entities.Image;
using AutoMapper;
using Core.Constants;

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
      private readonly IMapper _mapper;
      public ImageService(IImageRepository imageRepository, IMapper mapper)
      {
         _imageRepository = imageRepository;
         _mapper = mapper;
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
         var highQualityUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(streamContent, fileName, ConfigConstants.BLOB_CONTAINER);

         // Save Medium Size Image
         var processedMediumQualityImageStream = ProcessedImageFactory.TransformToMediumQualityImageFromFile(file);
         var processedMediumQualityFileName = $"{DateHelper.GetDateTimeNowString()}_medium_quality_{file.FileName.Replace(" ", "")}";
         var mediumQualityUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(processedMediumQualityImageStream, processedMediumQualityFileName, ConfigConstants.BLOB_CONTAINER);

         // Save Small Size Image
         var processedImageStream = ProcessedImageFactory.TransformToLowQualityImageFromFile(file);
         var processedFileName = $"{DateHelper.GetDateTimeNowString()}_low_quality_{file.FileName.Replace(" ", "")}";
         var lowQualityUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(processedImageStream, processedFileName, ConfigConstants.BLOB_CONTAINER);

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
         var createdImage = await _imageRepository.CreateImageAsync(request, urlList, cancellationToken);
         var response = _mapper.Map<ImageEntity, CreateImageResponse>(createdImage);

         return response;
      }

      public async Task<CreateImageResponse> UpdateImageByIdService(Guid id, UpdateImageRequest request, CancellationToken cancellationToken)
      {
         ImageEntity? updatedImage = null;
         if (request.File != null)
         {
            var urlList = await UploadImageService(request.File);
            updatedImage = await _imageRepository.UpdateImageByIdAsync(id, request, urlList, cancellationToken);
            var imageResponse = _mapper.Map<ImageEntity, CreateImageResponse>(updatedImage);
            return imageResponse;
         }
         updatedImage = await _imageRepository.UpdateImageByIdAsync(id, request, null, cancellationToken);
         var response = _mapper.Map<ImageEntity, CreateImageResponse>(updatedImage);
         return response;
      }

      public async Task<CreateImageResponse> DeleteImageByIdService(Guid id, CancellationToken cancellationToken)
      {
         var deletedImage = await _imageRepository.DeleteImageByIdAsync(id, cancellationToken);
         var response = _mapper.Map<ImageEntity, CreateImageResponse>(deletedImage);
         return response;
      }

      public async Task<CreateImageResponse> GetImageByIdService(Guid id, CancellationToken cancellationToken)
      {
         var image = await _imageRepository.GetImageByIdAsync(id, cancellationToken);
         var response = _mapper.Map<ImageEntity, CreateImageResponse>(image);
         return response;
      }
   }
}