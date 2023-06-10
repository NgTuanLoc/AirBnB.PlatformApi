using Core.Domain.RepositoryInterface;
using Core.Utils;
using Core.Constants;
using Core.Models.Image;

namespace Core.Services
{
   public interface IImageService
   {
      Task<UploadImageResponse> UploadImageAsync(UploadImageRequest request);
      Task<CreateImageResponse> CreateImageAsync(UploadImageRequest request, CancellationToken cancellationToken);
   }
   public class ImageService : IImageService
   {
      private readonly IImageRepository _imageRepository;
      public ImageService(IImageRepository imageRepository)
      {
         _imageRepository = imageRepository;
      }
      public async Task<UploadImageResponse> UploadImageAsync(UploadImageRequest request)
      {
         // Save Original Image
         var fileName = $"{DateHelper.GetDateTimeNowString()}_high_quality{request.File.FileName.Replace(" ", "")}";
         var streamContent = request.File.OpenReadStream();
         var highQualityUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(streamContent, fileName);

         // Save Medium Size Image
         var processedMediumQualityImageBuilder = new ImageProcessingBuilder();
         var processedMediumQualityImageStream = processedMediumQualityImageBuilder
                                 .LoadImage(request.File)
                                 .ResizeImage(ProcessingMediumQualityImageConstants.RESIZE_WIDTH, ProcessingMediumQualityImageConstants.RESIZE_HEIGHT)
                                 .BlurImage(ProcessingMediumQualityImageConstants.GAUSSIAN_BLUR_SIGMA)
                                 .GetImageStreamWithQuality(ProcessingMediumQualityImageConstants.IMAGE_QUALITY);
         var processedMediumQualityFileName = $"{DateHelper.GetDateTimeNowString()}_medium_quality_{request.File.FileName.Replace(" ", "")}";
         var mediumQualityUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(processedMediumQualityImageStream, processedMediumQualityFileName);

         // Save Small Size Image
         var processedImageBuilder = new ImageProcessingBuilder();
         var processedImageStream = processedImageBuilder
                                 .LoadImage(request.File)
                                 .ResizeImage(ProcessingLowQualityImageConstants.RESIZE_WIDTH, ProcessingLowQualityImageConstants.RESIZE_HEIGHT)
                                 .BlurImage(ProcessingLowQualityImageConstants.GAUSSIAN_BLUR_SIGMA)
                                 .GetImageStreamWithQuality(ProcessingLowQualityImageConstants.IMAGE_QUALITY);
         var processedFileName = $"{DateHelper.GetDateTimeNowString()}_low_quality_{request.File.FileName.Replace(" ", "")}";
         var lowQualityUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(processedImageStream, processedFileName);

         return new UploadImageResponse()
         {
            highQualityUrl = highQualityUrl,
            mediumQualityUrl = mediumQualityUrl,
            lowQualityUrl = lowQualityUrl
         };
      }

      public async Task<CreateImageResponse> CreateImageAsync(UploadImageRequest request, CancellationToken cancellationToken)
      {
         var urlList = await UploadImageAsync(request);
         var result = await _imageRepository.CreateImageAsync(request, urlList, cancellationToken);
         return result;
      }
   }
}