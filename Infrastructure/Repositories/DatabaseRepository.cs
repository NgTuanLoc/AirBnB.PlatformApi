using System.ComponentModel.DataAnnotations;
using Azure.Storage.Blobs;
using Core.Constants;
using Core.Domain.Entities;
using Core.Domain.IdentityEntities;
using Core.Domain.RepositoryInterface;
using Core.Models.Database;
using Core.Services;
using Core.Utils;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
   public class DatabaseRepository : IDatabaseRepository
   {
      private readonly ApplicationDbContext _context;
      private readonly BlobServiceClient _blobServiceClient;
      private readonly IUserRepository _userRepository;
      private readonly IImageRepository _imageRepository;
      public DatabaseRepository(ApplicationDbContext context, BlobServiceClient blobServiceClient, IUserRepository userRepository, IImageRepository imageRepository, IImageService imageService)
      {
         _context = context;
         _blobServiceClient = blobServiceClient;
         _userRepository = userRepository;
         _imageRepository = imageRepository;
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

      public async Task<string> SeedingAsync(CancellationToken cancellationToken)
      {
         var user = await _userRepository.GetUserAsync();
         await SeedingLocationBlobStorageAsync(user, cancellationToken);
         // await SeedingLocationAsync(user, cancellationToken);
         // await SeedingRoomAsync(user, cancellationToken);
         return "Seeding Data Successful";
      }

      private async Task SeedingLocationAsync(ApplicationUser user, CancellationToken cancellationToken)
      {
         string locationJson = System.IO.File.ReadAllText("./Data/Json/location.json");
         List<LocationModel>? locationList = System.Text.Json.JsonSerializer.Deserialize<List<LocationModel>>(locationJson) ?? throw new ValidationException("Fail to seeding room data");
         foreach (var location in locationList)
         {
            var imageStream = ImageToStream(location.ImagePath);
            Console.WriteLine("fileName room", location.ImagePath);

            var imageName = $"{DateHelper.GetDateTimeNowString()}_{location.ImagePath.Split("/").Last()}";
            var imageUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(imageStream, imageName, ConfigConstants.BlobContainer);

            var newLocation = new Location()
            {
               Name = location.Name,
               Province = location.Province,
               Country = location.Country,
               Image = imageUrl,
               CreatedBy = user.Email ?? "Unknown",
               CreatedDate = DateTime.Now
            };

            _context.Location.Add(newLocation);
         }

         await _context.SaveChangesAsync(cancellationToken);
      }

      private async Task SeedingRoomAsync(ApplicationUser user, CancellationToken cancellationToken)
      {
         string roomJson = File.ReadAllText("./Data/Json/room.json");
         List<RoomModel>? roomList = System.Text.Json.JsonSerializer.Deserialize<List<RoomModel>>(roomJson);
         var locationList = await _context.Location.ToListAsync(cancellationToken);

         if (roomList == null)
         {
            throw new ValidationException("Fail to seeding room data");
         }

         foreach (var room in roomList)
         {

            var location = locationList.FirstOrDefault(l => l.Name == room.LocationName);

            var newRoom = ConvertRoomModelIntoRoomEntity(room, location, user);
            _context.Room.Add(newRoom);
            await _context.SaveChangesAsync(cancellationToken);

            await SeedingRoomImageAsync(room.ImagePath, newRoom, cancellationToken);
         }
      }
      // Seeding Image
      private async Task SeedingLocationBlobStorageAsync(ApplicationUser user, CancellationToken cancellationToken)
      {
         var jsonBlobName = "Data/Json/location.json";
         var sourceContainerName = ConfigConstants.SeedingDataContainer;

         var sourceContainerClient = _blobServiceClient.GetBlobContainerClient(sourceContainerName);

         var jsonBlobClient = sourceContainerClient.GetBlobClient(jsonBlobName);

         if (await jsonBlobClient.ExistsAsync(cancellationToken))
         {
            using var response = await jsonBlobClient.OpenReadAsync(cancellationToken: cancellationToken);
            using var streamReader = new StreamReader(response);
            var locationJson = streamReader.ReadToEnd();
            var locationList = System.Text.Json.JsonSerializer.Deserialize<List<LocationModel>>(locationJson);

            if (locationList != null)
            {
               foreach (var location in locationList)
               {

                  var relativePath = location.ImagePath;
                  relativePath = relativePath.Replace("..", "");
                  var imagePath = "Data" + relativePath;

                  var imageStream = await ImageToBlobStream(imagePath, sourceContainerClient, sourceContainerName);

                  var imageName = $"{DateHelper.GetDateTimeNowString()}_{location.ImagePath.Split("/").Last()}";
                  var imageUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(imageStream, imageName, ConfigConstants.BlobContainer);

                  var newLocation = new Location()
                  {
                     Name = location.Name,
                     Province = location.Province,
                     Country = location.Country,
                     Image = imageUrl,
                     CreatedBy = user.Email ?? "Unknown",
                     CreatedDate = DateTime.Now
                  };

                  _context.Location.Add(newLocation);
               }
            }
         }
         await _context.SaveChangesAsync(cancellationToken);
      }

      private async Task SeedingRoomImageAsync(string folderPath, Room room, CancellationToken cancellationToken)
      {
         string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };  // Add more extensions if needed

         // Get all files in the folder
         string[] fileNames = Directory.GetFiles(folderPath);

         foreach (string fileName in fileNames)
         {
            string extension = Path.GetExtension(fileName);

            // Check if the file has a supported image extension
            if (imageExtensions.Contains(extension.ToLower()))
            {
               Stream imageStream1 = ImageToStream(fileName);
               Stream imageStream2 = ImageToStream(fileName);
               Stream imageStream3 = ImageToStream(fileName);

               var convertFileName = fileName.Split("/").Last();
               var imageName = $"{DateHelper.GetDateTimeNowString()}_{convertFileName}";

               // Original Image
               var imageUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(imageStream1, imageName, ConfigConstants.BlobContainer);

               // Save Medium Size Image
               var processedMediumQualityImageStream = ProcessedImageFactory.TransformToMediumQualityImageFromStream(imageStream2);
               var processedMediumQualityFileName = $"{DateHelper.GetDateTimeNowString()}_medium_quality_{convertFileName}";
               var mediumQualityUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(processedMediumQualityImageStream, processedMediumQualityFileName, ConfigConstants.BlobContainer);

               // Save Small Size Image
               var processedSmallQualityImageStream = ProcessedImageFactory.TransformToLowQualityImageFromStream(imageStream3);
               var processedFileName = $"{DateHelper.GetDateTimeNowString()}_low_quality_{convertFileName}";
               var lowQualityUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(processedSmallQualityImageStream, processedFileName, ConfigConstants.BlobContainer);

               var newImage = new Image
               {
                  Title = fileName,
                  Description = "Image Description",
                  LowQualityUrl = lowQualityUrl,
                  MediumQualityUrl = mediumQualityUrl,
                  HighQualityUrl = imageUrl,
                  Room = room
               };

               _context.Image.Add(newImage);
            }
         }
         await _context.SaveChangesAsync(cancellationToken);
      }
      // Read the image file and convert it into a stream
      public Stream ImageToStream(string imagePath)
      {
         Stream stream = new FileStream(imagePath, FileMode.Open);
         return stream;
      }
      public async Task<Stream> ImageToBlobStream(string blobName, BlobContainerClient blobContainer, string containerName)
      {

         BlobClient blobClient = blobContainer.GetBlobClient(blobName);
         Stream stream = await blobClient.OpenReadAsync();


         MemoryStream memoryStream = new MemoryStream();
         await stream.CopyToAsync(memoryStream);

         memoryStream.Seek(0, SeekOrigin.Begin);
         return memoryStream;
      }
      private Room ConvertRoomModelIntoRoomEntity(RoomModel roomModel, Location? location, ApplicationUser user)
      {
         return new Room()
         {
            Id = Guid.NewGuid(),
            Name = roomModel.Name,
            HomeType = roomModel.HomeType,
            RoomType = roomModel.RoomType,
            TotalOccupancy = roomModel.TotalOccupancy,
            TotalBedrooms = roomModel.TotalBedrooms,
            TotalBathrooms = roomModel.TotalBathrooms,
            Summary = roomModel.Summary,
            Address = roomModel.Address,
            HasTV = roomModel.HasTV,
            HasKitchen = roomModel.HasKitchen,
            HasAirCon = roomModel.HasAirCon,
            HasHeating = roomModel.HasHeating,
            HasInternet = roomModel.HasInternet,
            Price = roomModel.Price,
            Latitude = roomModel.Latitude,
            Longitude = roomModel.Longitude,
            Location = location,
            Owner = user,
            CreatedBy = user.Email ?? "Unknown",
            CreatedDate = DateTime.Now
         };
      }
   }
}