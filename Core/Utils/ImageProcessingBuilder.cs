using Core.Constants;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Core.Utils
{
   public interface IImageProcessingBuilder
   {
      ImageProcessingBuilder LoadImageFromFile(IFormFile imageFile);
      ImageProcessingBuilder LoadImageFromStream(Stream imageStream);
      ImageProcessingBuilder ResizeImage(int width, int height);
      ImageProcessingBuilder BlurImage(float sigma);
      Stream GetImageStreamWithQuality(int quality);
   }
   public class ImageProcessingBuilder : IImageProcessingBuilder
   {
      private Image? _processedImage;
      public ImageProcessingBuilder()
      {
         _processedImage = null;
      }

      public ImageProcessingBuilder LoadImageFromFile(IFormFile imageFile)
      {
         var streamContent = imageFile.OpenReadStream();
         _processedImage = Image.Load(streamContent);
         return this;
      }

      public  ImageProcessingBuilder LoadImageFromStream(Stream imageStream)
      {
         _processedImage = Image.Load(imageStream);
         return this;
      }

      public ImageProcessingBuilder ResizeImage(int width, int height)
      {
         if (_processedImage == null)
         {
            throw new ValidationException("Fail to compress image !");
         }

         _processedImage.Mutate(item => item.Resize(
            new ResizeOptions
            {
               Mode = ResizeMode.Max,
               Size = new Size(width, height)
            }
            ));
         return this;
      }

      public ImageProcessingBuilder BlurImage(float sigma)
      {
         if (_processedImage == null)
         {
            throw new ValidationException("Fail to compress image !");
         }
         _processedImage.Mutate(item => item.GaussianBlur(sigma));
         return this;
      }

      public Stream GetImageStreamWithQuality(int quality)
      {
         Stream outputStream = new MemoryStream();

         if (_processedImage == null)
         {
            throw new ValidationException("Fail to compress image !");
         }
         var jpegEncoder = new JpegEncoder { Quality = quality };
         _processedImage.Save(outputStream, jpegEncoder);
         return outputStream;
      }
   }
   public static class ProcessedImageFactory
   {
      public static Stream TransformToMediumQualityImageFromFile(IFormFile file)
      {
         var processedImageBuilder = new ImageProcessingBuilder();
         var processedImageStream = processedImageBuilder
                                 .LoadImageFromFile(file)
                                 .ResizeImage(ProcessingMediumQualityImageConstants.RESIZE_WIDTH, ProcessingMediumQualityImageConstants.RESIZE_HEIGHT)
                                 .BlurImage(ProcessingMediumQualityImageConstants.GAUSSIAN_BLUR_SIGMA)
                                 .GetImageStreamWithQuality(ProcessingMediumQualityImageConstants.IMAGE_QUALITY);
         return processedImageStream;
      }
      public static Stream TransformToLowQualityImageFromFile(IFormFile file)
      {
         var processedImageBuilder = new ImageProcessingBuilder();
         var processedImageStream = processedImageBuilder
                                 .LoadImageFromFile(file)
                                 .ResizeImage(ProcessingLowQualityImageConstants.RESIZE_WIDTH, ProcessingLowQualityImageConstants.RESIZE_HEIGHT)
                                 .BlurImage(ProcessingLowQualityImageConstants.GAUSSIAN_BLUR_SIGMA)
                                 .GetImageStreamWithQuality(ProcessingLowQualityImageConstants.IMAGE_QUALITY);
         return processedImageStream;
      }
      public static Stream TransformToMediumQualityImageFromStream(Stream stream)
      {
         var processedImageBuilder = new ImageProcessingBuilder();
         var processedImageStream = processedImageBuilder
                                 .LoadImageFromStream(stream)
                                 .ResizeImage(ProcessingMediumQualityImageConstants.RESIZE_WIDTH, ProcessingMediumQualityImageConstants.RESIZE_HEIGHT)
                                 .BlurImage(ProcessingMediumQualityImageConstants.GAUSSIAN_BLUR_SIGMA)
                                 .GetImageStreamWithQuality(ProcessingMediumQualityImageConstants.IMAGE_QUALITY);
         return processedImageStream;
      }
      public static Stream TransformToLowQualityImageFromStream(Stream stream)
      {
         var processedImageBuilder = new ImageProcessingBuilder();
         var processedImageStream = processedImageBuilder
                                 .LoadImageFromStream(stream)
                                 .ResizeImage(ProcessingLowQualityImageConstants.RESIZE_WIDTH, ProcessingLowQualityImageConstants.RESIZE_HEIGHT)
                                 .BlurImage(ProcessingLowQualityImageConstants.GAUSSIAN_BLUR_SIGMA)
                                 .GetImageStreamWithQuality(ProcessingLowQualityImageConstants.IMAGE_QUALITY);
         return processedImageStream;
      }
   }
}