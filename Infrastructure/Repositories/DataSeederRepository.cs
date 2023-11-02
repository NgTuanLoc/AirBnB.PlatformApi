using Azure.Storage.Blobs;
using Contracts;
using Core.Constants;
using Core.Domain.Entities;
using Core.Domain.IdentityEntities;
using Core.Domain.RepositoryInterface;
using Core.Exceptions;
using Core.Utils;
using Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure.Repositories;
public class DataSeederRepository : IDataSeederRepository
{
    private readonly ApplicationDbContext _context;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IImageRepository _imageRepository;
    private readonly ISendingMessageRepository _sendingMessageRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public DataSeederRepository(ApplicationDbContext context, BlobServiceClient blobServiceClient, IImageRepository imageRepository, ISendingMessageRepository sendingMessageRepository, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
        _imageRepository = imageRepository;
        _sendingMessageRepository = sendingMessageRepository;
        _userManager = userManager;
    }
    public async Task<Location> SeedingLocationRepositoryAsync(LocationMessageModel model, CancellationToken cancellationToken)
    {
        BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(ConfigConstants.SEEDING_BLOB_DATA_CONTAINER);

        var imageStream = await GetImageStreamFromBlob(model.ImagePath, blobContainerClient);

        var imageName = CreateImageName(model.ImagePath);
        var imageUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(imageStream, imageName, ConfigConstants.BLOB_CONTAINER);

        var newLocation = new Location()
        {
            Name = model.Name,
            Province = model.Province,
            Country = model.Country,
            Image = imageUrl,
            CreatedBy = model.Email,
            CreatedDate = DateTime.Now
        };

        _context.Location.Add(newLocation);
        await _context.SaveChangesAsync(cancellationToken);

        return newLocation;
    }

    public async Task SeedingRoomRepositoryAsync(RoomMessageModel model, CancellationToken cancellationToken)
    {
        // Prepare Room entity to create
        var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new NotFoundException("User not found !");
        var location = await _context.Location.FirstOrDefaultAsync(location => location.Name == model.LocationName, cancellationToken);

        var newRoom = ConvertRoomModelIntoRoomEntity(model, location, user);
        _context.Room.Add(newRoom);

        // Prepare Image 
        BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(ConfigConstants.SEEDING_BLOB_DATA_CONTAINER);

        foreach (var imagePath in model.ImagePath)
        {
            var imageStream = await GetImageStreamFromBlob(imagePath, blobContainerClient);
            var imageStream1 = await GetImageStreamFromBlob(imagePath, blobContainerClient);
            var imageStream2 = await GetImageStreamFromBlob(imagePath, blobContainerClient);

            // Original Image
            var imageName = CreateImageName(imagePath);
            var imageUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(imageStream, imageName, ConfigConstants.BLOB_CONTAINER);

            // Save Medium Size Image
            var processedMediumQualityImageStream = ProcessedImageFactory.TransformToMediumQualityImageFromStream(imageStream1);
            var processedMediumQualityFileName = $"medium_quality_{imageName}";
            var mediumQualityUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(processedMediumQualityImageStream, processedMediumQualityFileName, ConfigConstants.BLOB_CONTAINER);

            // Save Small Size Image
            var processedSmallQualityImageStream = ProcessedImageFactory.TransformToLowQualityImageFromStream(imageStream2);
            var processedFileName = $"low_quality_{imageName}";
            var lowQualityUrl = await _imageRepository.UploadImageFileToBlobStorageAsync(processedSmallQualityImageStream, processedFileName, ConfigConstants.BLOB_CONTAINER);

            var newImage = new Image
            {
                Title = imageName,
                Description = "Image Description",
                LowQualityUrl = lowQualityUrl,
                MediumQualityUrl = mediumQualityUrl,
                HighQualityUrl = imageUrl,
                Room = newRoom
            };
            _context.Image.Add(newImage);
        }
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SendMessageToCreateRoomAsync(Location location, CancellationToken cancellationToken)
    {
        // Read location json file from blob storage
        string jsonString = await _sendingMessageRepository.ReadJsonFileFromBlobStorageAsync(ConfigConstants.ROOM_JSON_BLOB_NAME, ConfigConstants.SEEDING_BLOB_DATA_CONTAINER, cancellationToken);
        var roomList = JsonConvert.DeserializeObject<List<RoomMessageModel>>(jsonString);

        List<string> messageStringList = new();
        if (roomList is null) return;
        // Map and create message to send to ASB
        foreach (var message in roomList)
        {
            if (message.LocationName != location.Name) continue;
            message.Email = location.CreatedBy;
            messageStringList.Add(JsonConvert.SerializeObject(message));
        }
        await _sendingMessageRepository.SendMessageInBatchAsync(messageStringList, ConfigConstants.AMOUNT_OF_MESSAGES_PER_BATCH, ConfigConstants.ROOM_SEEDER_QUEUE, cancellationToken);
    }

    private static async Task<Stream> GetImageStreamFromBlob(string blobName, BlobContainerClient blobContainer)
    {

        BlobClient blobClient = blobContainer.GetBlobClient(blobName);
        Stream stream = await blobClient.OpenReadAsync();

        MemoryStream memoryStream = new();
        await stream.CopyToAsync(memoryStream);

        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    private static string CreateImageName(string path)
    {
        return $"{DateHelper.GetDateTimeNowString()}_{path.Split("/").Last()}";
    }

    private static Room ConvertRoomModelIntoRoomEntity(RoomMessageModel roomModel, Location? location, ApplicationUser user)
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
