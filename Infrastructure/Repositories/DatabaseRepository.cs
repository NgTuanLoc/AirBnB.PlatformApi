using Azure.Storage.Blobs;
using Core.Constants;
using Core.Domain.RepositoryInterface;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
   public class DatabaseRepository : IDatabaseRepository
   {
      private readonly ApplicationDbContext _context;
      private readonly BlobServiceClient _blobServiceClient;
      public DatabaseRepository(ApplicationDbContext context, BlobServiceClient blobServiceClient)
      {
         _context = context;
         _blobServiceClient = blobServiceClient;
      }
      public async Task<string> RestoreAsync(CancellationToken cancellationToken)
      {
         var allRoomList = await _context.Room.ToListAsync(cancellationToken);
         var allLocationList = await _context.Location.ToListAsync(cancellationToken);
         var allImageList = await _context.Image.ToListAsync(cancellationToken);
         var allReservationList = await _context.Reservation.ToListAsync(cancellationToken);
         var allReviewList = await _context.Review.ToListAsync(cancellationToken);

         _context.RemoveRange(allRoomList);
         _context.RemoveRange(allLocationList);
         _context.RemoveRange(allImageList);
         _context.RemoveRange(allReservationList);
         _context.RemoveRange(allReviewList);

         await DeleteAllDataInBlobStorageAsync();

         await _context.SaveChangesAsync(cancellationToken);
         return "Restore Successful";
      }

      private async Task DeleteAllDataInBlobStorageAsync()
      {
         var blobStorageContainer = _blobServiceClient.GetBlobContainerClient(ConfigConstants.BlobContainer);

         await foreach (var blobItem in blobStorageContainer.GetBlobsAsync())
         {
            await blobStorageContainer.DeleteBlobAsync(blobItem.Name);
         }
      }
   }
}