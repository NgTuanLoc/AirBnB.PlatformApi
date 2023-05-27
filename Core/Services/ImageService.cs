using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services
{
   public interface IImageService
   {
      Task CreateImageAsync();
   }
   public class ImageService : IImageService
   {
      // private readonly IImageRepository _imageRepository;
      public ImageService()
      {

      }
      public async Task CreateImageAsync()
      {

      }
   }
}