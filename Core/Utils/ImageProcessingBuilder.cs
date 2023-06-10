using System.Globalization;
using Core.Constants;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Core.Utils
{
   public interface IImageProcessingBuilder
   {
      ImageProcessingBuilder LoadImage(IFormFile imageFile);
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

      public ImageProcessingBuilder LoadImage(IFormFile imageFile)
      {
         var streamContent = imageFile.OpenReadStream();
         _processedImage = Image.Load(streamContent);
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
}