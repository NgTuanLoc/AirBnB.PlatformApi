using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Core.Constants;
using Core.Domain.RepositoryInterface;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Repositories
{
   public class ImageRepository : IImageRepository
   {
      private readonly BlobServiceClient _blobServiceClient;
      public ImageRepository(BlobServiceClient blobServiceClient)
      {
         _blobServiceClient = blobServiceClient;
      }
      public async Task UploadImageFileToBlobStorageAsync(IFormFile file, string filename)
      {
         var blobStorageContainer = _blobServiceClient.GetBlobContainerClient(ConfigConstants.BlobContainer);
         var blobStorageClient = blobStorageContainer.GetBlobClient(filename);
         var streamContent = file.OpenReadStream();
         await blobStorageClient.UploadAsync(streamContent);
      }
   }
}