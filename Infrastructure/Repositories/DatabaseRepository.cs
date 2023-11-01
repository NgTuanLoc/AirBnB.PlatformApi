using Azure.Storage.Blobs;
using Contracts;
using Core.Constants;
using Core.Domain.RepositoryInterface;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure.Repositories
{
   public class DatabaseRepository : IDatabaseRepository
   {
      private readonly ApplicationDbContext _context;
      private readonly BlobServiceClient _blobServiceClient;
      private readonly IUserRepository _userRepository;
      private readonly ISendingMessageRepository _sendingMessageRepository;

      public DatabaseRepository(ApplicationDbContext context, BlobServiceClient blobServiceClient, IUserRepository userRepository, ISendingMessageRepository sendingMessageRepository)
      {
         _context = context;
         _blobServiceClient = blobServiceClient;
         _userRepository = userRepository;
         _sendingMessageRepository = sendingMessageRepository;
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
         var blobStorageContainer = _blobServiceClient.GetBlobContainerClient(ConfigConstants.BLOB_CONTAINER);

         await foreach (var blobItem in blobStorageContainer.GetBlobsAsync())
         {
            await blobStorageContainer.DeleteBlobAsync(blobItem.Name);
         }
      }

      public async Task<string> SeedingAsync(CancellationToken cancellationToken)
      {
         var user = await _userRepository.GetUserAsync();
         await SendMessageToCreateLocationAsync(user?.Email ?? "Unknown", cancellationToken);
         // await SeedingRoomBlobStorageAsync(user, cancellationToken);
         // await SeedingLocationAsync(user, cancellationToken);
         // await SeedingRoomAsync(user, cancellationToken);
         return "Seeding Data Successful";
      }

      private async Task SendMessageToCreateLocationAsync(string userEmail, CancellationToken cancellationToken)
      {
         // Read location json file from blob storage
         string jsonString = await _sendingMessageRepository.ReadJsonFileFromBlobStorageAsync(ConfigConstants.LOCATION_JSON_BLOB_NAME, ConfigConstants.SEEDING_BLOB_DATA_CONTAINER, cancellationToken);
         var locationList = JsonConvert.DeserializeObject<List<LocationMessageModel>>(jsonString);
         List<string> messageStringList = new();
         if (locationList is null) return;
         // Map and create message to send to ASB
         foreach (var message in locationList)
         {
            message.Email = userEmail;
            messageStringList.Add(JsonConvert.SerializeObject(message));
         }
         await _sendingMessageRepository.SendMessageInBatchAsync(messageStringList, ConfigConstants.AMOUNT_OF_MESSAGES_PER_BATCH, cancellationToken);
      }
   }
}