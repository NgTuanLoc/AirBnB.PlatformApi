using Azure.Storage.Blobs;
using Core.Constants;
using Core.Domain.Entities;
using Core.Domain.RepositoryInterface;
using Core.Exceptions;
using Core.Models.Image;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using ImageEntity = Core.Domain.Entities.Image;

namespace Infrastructure.Repositories
{
   public class ImageRepository : IImageRepository
   {
      private readonly BlobServiceClient _blobServiceClient;
      private readonly ApplicationDbContext _context;
      private readonly IUserRepository _userRepository;
      public ImageRepository(BlobServiceClient blobServiceClient, ApplicationDbContext context, IUserRepository userRepository)
      {
         _blobServiceClient = blobServiceClient;
         _context = context;
         _userRepository = userRepository;
      }

      public async Task<string> UploadImageFileToBlobStorageAsync(Stream streamContent, string filename)
      {
         var blobStorageContainer = _blobServiceClient.GetBlobContainerClient(ConfigConstants.BlobContainer);
         var blobStorageClient = blobStorageContainer.GetBlobClient(filename);
         streamContent.Position = 0;
         await blobStorageClient.UploadAsync(streamContent);
         return blobStorageClient.Uri.AbsoluteUri;
      }
      public async Task<string> DeleteImageFileFromBlobStorageAsync(string? imageUrl)
      {
         if (imageUrl == null) return "Can not delete image while FileName is null !";

         string filename = imageUrl.Split("/").Last();

         var blobStorageContainer = _blobServiceClient.GetBlobContainerClient(ConfigConstants.BlobContainer);
         var blobStorageClient = blobStorageContainer.GetBlobClient(filename);
         await blobStorageClient.DeleteIfExistsAsync();
         return $"Delete file {filename} successfully !";
      }

      public async Task<ImageEntity> CreateImageAsync(UploadImageRequest request, UploadImageResponse urlList, CancellationToken cancellationToken)
      {
         var user = await _userRepository.GetUserAsync();
         Room? room = null;

         if (request.RoomId != null)
         {
            room = await _context.Room.FirstOrDefaultAsync(item => item.Id == request.RoomId, cancellationToken);

            if (room == null) throw new NotFoundException($"Room with Id {request.RoomId} not found !");
         }

         var image = new Image()
         {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            HighQualityUrl = urlList.highQualityUrl,
            MediumQualityUrl = urlList.mediumQualityUrl,
            LowQualityUrl = urlList.lowQualityUrl,
            CreatedDate = DateTime.Now,
            CreatedBy = user.Email ?? "Unknown",
            Room = room
         };

         var result = _context.Image.Add(image);

         await _context.SaveChangesAsync(cancellationToken);

         return image;
      }

      public async Task<ImageEntity> GetImageByIdAsync(Guid id, CancellationToken cancellationToken)
      {
         var image = await _context.Image.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

         if (image == null) throw new ValidationException("Image not found !");

         return image;
      }

      public async Task<ImageEntity> DeleteImageByIdAsync(Guid id, CancellationToken cancellationToken)
      {
         var image = await _context.Image.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

         if (image == null) throw new ValidationException("Image not found !");

         // Remove Image Data From Blob Storage
         await DeleteImageFileFromBlobStorageAsync(image.HighQualityUrl);
         await DeleteImageFileFromBlobStorageAsync(image.MediumQualityUrl);
         await DeleteImageFileFromBlobStorageAsync(image.LowQualityUrl);

         _context.Remove(image);
         await _context.SaveChangesAsync(cancellationToken);
         return image;
      }

      public async Task<ImageEntity> UpdateImageByIdAsync(Guid id, UpdateImageRequest request, UploadImageResponse? urlList, CancellationToken cancellationToken)
      {
         var image = await _context.Image.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

         if (image == null) throw new ValidationException("Image not found !");

         // Update Image
         if (request.Title != null)
         {
            image.Title = request.Title;
         }

         if (request.Description != null)
         {
            image.Description = request.Description;
         }

         if (request.RoomId != null)
         {
            var room = await _context.Room.FirstOrDefaultAsync(item => item.Id == request.RoomId);

            if (room == null) throw new ValidationException("Room not found !");

            image.Room = room;
         }

         if (request.File != null && urlList != null)
         {
            // Remove Image Data From Blob Storage
            await DeleteImageFileFromBlobStorageAsync(image.HighQualityUrl);
            await DeleteImageFileFromBlobStorageAsync(image.MediumQualityUrl);
            await DeleteImageFileFromBlobStorageAsync(image.LowQualityUrl);

            // Save New Image Url
            image.HighQualityUrl = urlList.highQualityUrl;
            image.MediumQualityUrl = urlList.mediumQualityUrl;
            image.LowQualityUrl = urlList.lowQualityUrl;
         }

         var user = await _userRepository.GetUserAsync();
         image.ModifiedBy = user.Email;
         image.ModifiedDate = DateTime.Now;

         await _context.SaveChangesAsync(cancellationToken);

         return image;
      }
   }
}