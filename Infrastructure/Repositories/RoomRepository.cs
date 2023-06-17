using Core.Domain.Entities;
using Core.Domain.RepositoryInterface;
using Core.Exceptions;
using Core.Models.Image;
using Core.Models.Room;
using Core.Services;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using ImageEntity = Core.Domain.Entities.Image;

namespace Infrastructure.Repositories
{
   public class RoomRepository : IRoomRepository
   {
      private readonly IUserService _userService;
      private readonly IImageRepository _imageRepository;
      private readonly IImageService _imageService;
      private readonly ApplicationDbContext _context;
      public RoomRepository(ApplicationDbContext context, IUserService userService, IImageRepository imageRepository, IImageService imageService)
      {
         _context = context;
         _userService = userService;
         _imageRepository = imageRepository;
         _imageService = imageService;
      }
      public async Task<Room> CreateRoomAsync(CreateRoomRequest request, CancellationToken cancellationToken)
      {
         var user = await _userService.GetUserService();
         Location? location = null;

         if (request.LocationId != null)
         {
            location = await _context.Location.FirstOrDefaultAsync(item => item.Id == request.LocationId, cancellationToken);

            if (location == null) throw new NotFoundException($"Location with id {request.LocationId} can not be found !");
         }

         var newRoom = new Room()
         {
            Id = Guid.NewGuid(),
            Name = request.Name,
            HomeType = request.HomeType,
            RoomType = request.RoomType,
            TotalOccupancy = request.TotalOccupancy,
            TotalBedrooms = request.TotalBedrooms,
            TotalBathrooms = request.TotalBathrooms,
            Summary = request.Summary,
            Address = request.Address,
            HasTV = request.HasTV,
            HasKitchen = request.HasKitchen,
            HasAirCon = request.HasAirCon,
            HasHeating = request.HasHeating,
            HasInternet = request.HasInternet,
            Price = request.Price,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Owner = user,
            Location = location,
            CreatedBy = user.Email ?? "Unknown",
            CreatedDate = DateTime.Now,
         };

         _context.Room.Add(newRoom);
         await _context.SaveChangesAsync(cancellationToken);

         // Create Image
         if (request.ImageList != null)
         {
            newRoom.ImageList = new List<ImageEntity>();
            foreach (var image in request.ImageList)
            {
               var uploadImageRequest = new UploadImageRequest()
               {
                  Title = $"{newRoom.Name} Image",
                  Description = $"{newRoom.Name} Description",
                  File = image,
                  RoomId = newRoom.Id
               };
               var urlList = await _imageService.UploadImageService(image);
               var newImage = await _imageRepository.CreateImageAsync(uploadImageRequest, urlList, cancellationToken);
               newRoom.ImageList.Add(newImage);
            }
         }
         return newRoom;
      }
   }
}