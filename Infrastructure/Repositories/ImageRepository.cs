using Azure.Storage.Blobs;
using Core.Constants;
using Core.Domain.Entities;
using Core.Domain.RepositoryInterface;
using Core.Models.Image;
using Infrastructure.DbContext;
namespace Infrastructure.Repositories
{
   public class ImageRepository : IImageRepository
   {
      private readonly BlobServiceClient _blobServiceClient;
      private readonly ApplicationDbContext _context;
      public ImageRepository(BlobServiceClient blobServiceClient, ApplicationDbContext context)
      {
         _blobServiceClient = blobServiceClient;
         _context = context;
      }

      public async Task<CreateImageResponse> CreateImageAsync(UploadImageRequest request, UploadImageResponse urlList, CancellationToken cancellationToken)
      {
         var image = new Image()
         {
            Title = request.Title,
            Description = request.Description,
            HighQualityUrl = urlList.highQualityUrl,
            MediumQualityUrl = urlList.mediumQualityUrl,
            LowQualityUrl = urlList.lowQualityUrl,
            Room = null
         };

         var result = _context.Image.Add(image);

         await _context.SaveChangesAsync(cancellationToken);

         return new CreateImageResponse()
         {
            Title = image.Title,
            Description = image.Description,
            HighQualityUrl = image.HighQualityUrl,
            MediumQualityUrl = image.MediumQualityUrl,
            LowQualityUrl = image.LowQualityUrl,
         };
      }

      public async Task<string> UploadImageFileToBlobStorageAsync(Stream streamContent, string filename)
      {
         var blobStorageContainer = _blobServiceClient.GetBlobContainerClient(ConfigConstants.BlobContainer);
         var blobStorageClient = blobStorageContainer.GetBlobClient(filename);
         streamContent.Position = 0;
         var result = await blobStorageClient.UploadAsync(streamContent);
         return blobStorageClient.Uri.AbsoluteUri;
      }
   }
}