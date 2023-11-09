using AutoMapper;
using Core.Domain.Entities;
using Core.Domain.RepositoryInterface;
using Core.Exceptions;
using Core.Models.Image;
using Core.Models.PaginationModel;
using Core.Models.Room;
using Core.Services;
using Core.Utils.ServerSidePaginationUtils;
using Infrastructure.DbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ImageEntity = Core.Domain.Entities.Image;

namespace Infrastructure.Repositories
{
   public class RoomRepository : IRoomRepository
   {
      private readonly IUserRepository _userRepository;
      private readonly IImageRepository _imageRepository;
      private readonly IImageService _imageService;
      private readonly ApplicationDbContext _context;
      private readonly IMapper _mapper;
      public RoomRepository(ApplicationDbContext context, IUserRepository userRepository, IImageRepository imageRepository, IImageService imageService, IMapper mapper)
      {
         _context = context;
         _userRepository = userRepository;
         _imageRepository = imageRepository;
         _imageService = imageService;
         _mapper = mapper;
      }
      public async Task<Room> CreateRoomAsync(CreateRoomRequest request, CancellationToken cancellationToken)
      {
         var user = await _userRepository.GetUserAsync();
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
            var createdImageList = await CreateImageListForRoom(newRoom, request.ImageList, cancellationToken);
            newRoom.ImageList = createdImageList;
         }
         return newRoom;
      }

      public async Task<Room> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken)
      {
         var room = await _context.Room
            .Include(item => item.Location)
            .Include(item => item.ImageList)
            .Include(item => item.Owner)
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken) ?? throw new NotFoundException($"Room with id {id} can not be found !");
         ;

         return room;
      }

      public async Task<PaginationResponse<CreateRoomResponse>> GetAllRoomListAsync(Guid? locationId, PagingParams pagingParams, PaginationModel paginationModel, CancellationToken cancellationToken)
      {
         IQueryable<Room> roomQuery = _context.Room
         .Include(item => item.Location)
         .Include(item => item.ImageList)
         .Include(item => item.Owner);
         List<Room> roomList = new();

         if (locationId != null)
         {
            var location = await _context.Location.Where(location => location.Id == locationId).FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException($"Location with id {locationId} can not be found !");

            roomQuery = roomQuery.Where(room => room.Location != null && room.Location.Id == locationId);
         }

         var filteredRoomQuery = RoomPaginateUtil.ApplyFilters(roomQuery, paginationModel.FilterDescriptorList);

         var sortedRoomQuery = RoomPaginateUtil.ApplySorting(filteredRoomQuery, paginationModel.SortField, paginationModel.SortOrder);

         var paginatedRoomQuery = RoomPaginateUtil.ApplyPagination(sortedRoomQuery, pagingParams);

         roomList = await paginatedRoomQuery.ToListAsync(cancellationToken);

         var totalPage = Math.Ceiling((double)roomList.Count / pagingParams.PageSize);

         List<CreateRoomResponse> data = _mapper.Map<List<Room>, List<CreateRoomResponse>>(roomList);

         PaginationResponse<CreateRoomResponse> response = new()
         {
            Data = data,
            Page = pagingParams.Page,
            PageSize = pagingParams.PageSize,
            TotalRecords = roomQuery.Count(),
            TotalFilteredRecords = roomList.Count,
         };

         return response;
      }

      public async Task<Room> DeleteRoomByIdAsync(Guid id, CancellationToken cancellationToken)
      {
         var room = await _context.Room
            .Include(item => item.Location)
            .Include(item => item.ImageList)
            .Include(item => item.Owner)
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken) ?? throw new NotFoundException($"Room with id {id} can not be found !");
         ;

         if (room.ImageList != null)
         {
            foreach (var image in room.ImageList)
            {
               await _imageService.DeleteImageByIdService(image.Id, cancellationToken);
            }
         }

         _context.Room.Remove(room);
         await _context.SaveChangesAsync(cancellationToken);

         return room;
      }

      public async Task<Room> UpdateRoomByIdAsync(Guid id, UpdateRoomRequest request, CancellationToken cancellationToken)
      {
         var room = await _context.Room
            .Include(item => item.Location)
            .Include(item => item.ImageList)
            .Include(item => item.Owner)
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken) ?? throw new NotFoundException($"Room with id {id} can not be found !");
         ;

         // Update Core Field
         room.Name = request.Name;
         room.HomeType = request.HomeType ?? room.HomeType;
         room.RoomType = request.RoomType ?? room.RoomType;
         room.TotalOccupancy = request.TotalOccupancy ?? room.TotalOccupancy;
         room.TotalBedrooms = request.TotalBedrooms ?? room.TotalBedrooms;
         room.TotalBathrooms = request.TotalBathrooms ?? room.TotalBathrooms;
         room.Summary = request.Summary ?? room.Summary;
         room.Address = request.Address ?? room.Address;
         room.HasTV = request.HasTV ?? room.HasTV;
         room.HasKitchen = request.HasKitchen ?? room.HasKitchen;
         room.HasAirCon = request.HasAirCon ?? room.HasAirCon;
         room.HasHeating = request.HasHeating ?? room.HasHeating;
         room.HasInternet = request.HasInternet ?? room.HasInternet;
         room.Price = request.Price ?? room.Price;
         room.Latitude = request.Latitude ?? room.Latitude;
         room.Longitude = request.Longitude ?? room.Longitude;

         if (request.ImageList != null)
         {
            var createdImageList = await CreateImageListForRoom(room, request.ImageList, cancellationToken);
            room.ImageList = createdImageList;
         }

         if (request.LocationId != null)
         {
            var location = await _context.Location.FirstOrDefaultAsync(item => item.Id == request.LocationId, cancellationToken) ?? throw new NotFoundException($"Location with id {request.LocationId} can not be found !");
            room.Location = location;
         }

         var user = await _userRepository.GetUserAsync();
         room.ModifiedBy = user.Email;
         room.ModifiedDate = DateTime.Now;

         await _context.SaveChangesAsync(cancellationToken);

         return room;
      }

      private async Task<List<ImageEntity>> CreateImageListForRoom(Room room, List<IFormFile> imageList, CancellationToken cancellationToken)
      {
         var createdImageList = new List<ImageEntity>();
         foreach (var image in imageList)
         {
            var uploadImageRequest = new UploadImageRequest()
            {
               Title = $"{room.Name} Image",
               Description = $"{room.Name} Description",
               File = image,
               RoomId = room.Id
            };
            var urlList = await _imageService.UploadImageService(image);
            var newImage = await _imageRepository.CreateImageAsync(uploadImageRequest, urlList, cancellationToken);
            createdImageList.Add(newImage);
         }
         return createdImageList;
      }
   }
}