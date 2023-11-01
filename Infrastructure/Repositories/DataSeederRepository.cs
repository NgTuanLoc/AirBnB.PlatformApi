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
    public async Task SeedingLocationRepositoryAsync(LocationMessageModel model, CancellationToken cancellationToken)
    {
        BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(ConfigConstants.SEEDING_BLOB_DATA_CONTAINER);

        var imageStream = await GetImageStreamFromBlobStream(model.ImagePath, blobContainerClient);

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

        // Prepare for room messaging
        var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new NotFoundException("User not found !");
        Console.WriteLine(user.Email);
    }

    public Task SeedingRoomRepositoryAsync(RoomMessageModel model, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task SendMessageToCreateRoomAsync(CancellationToken cancellationToken)
    {
        // Read location json file from blob storage
        string jsonString = await _sendingMessageRepository.ReadJsonFileFromBlobStorageAsync(ConfigConstants.ROOM_JSON_BLOB_NAME, ConfigConstants.SEEDING_BLOB_DATA_CONTAINER, cancellationToken);
        var locationList = JsonConvert.DeserializeObject<List<RoomMessageModel>>(jsonString);
        List<string> messageStringList = new();
        if (locationList is null) return;
        // Map and create message to send to ASB
        foreach (var message in locationList)
        {
            messageStringList.Add(JsonConvert.SerializeObject(message));
        }
        await _sendingMessageRepository.SendMessageInBatchAsync(messageStringList, ConfigConstants.AMOUNT_OF_MESSAGES_PER_BATCH, cancellationToken);
    }

    private static async Task<Stream> GetImageStreamFromBlobStream(string blobName, BlobContainerClient blobContainer)
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
}
