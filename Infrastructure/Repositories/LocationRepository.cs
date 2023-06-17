using Core.Domain.Entities;
using Core.Domain.RepositoryInterface;
using Core.Exceptions;
using Core.Models.Location;
using Core.Utils;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
   public class LocationRepository : ILocationRepository
   {
      private readonly ApplicationDbContext _context;
      private readonly IUserRepository _userRepository;
      private readonly IImageRepository _imageRepository;
      public LocationRepository(ApplicationDbContext context, IUserRepository userRepository, IImageRepository imageRepository)
      {
         _context = context;
         _userRepository = userRepository;
         _imageRepository = imageRepository;
      }
      public async Task<CreateLocationResponse> CreateLocationAsync(CreateLocationRequest request, string? imageUrl, CancellationToken cancellationToken)
      {
         var user = await _userRepository.GetUserAsync();

         var newLocation = new Location()
         {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Province = request.Province,
            Country = request.Country,
            CreatedDate = DateTime.Now,
            Image = imageUrl,
            CreatedBy = user.Email ?? "Unknown",
         };

         _context.Location.Add(newLocation);
         await _context.SaveChangesAsync(cancellationToken);

         return GetLocationResponse(newLocation);
      }
      public async Task<CreateLocationResponse> GetLocationByIdAsync(Guid id, CancellationToken cancellationToken)
      {
         var location = await _context.Location.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

         if (location == null) throw new NotFoundException($"Location with id {id} can not be found !");

         return GetLocationResponse(location);
      }

      public async Task<List<CreateLocationResponse>> GetAllLocationAsync(CancellationToken cancellationToken)
      {
         var locationList = await _context.Location.ToListAsync(cancellationToken);

         var locationListResponse = new List<CreateLocationResponse>();

         foreach (var location in locationList)
         {
            locationListResponse.Add(GetLocationResponse(location));
         }

         return locationListResponse;
      }

      public async Task<CreateLocationResponse> DeleteLocationByIdAsync(Guid id, CancellationToken cancellationToken)
      {
         var location = await _context.Location.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

         if (location == null) throw new NotFoundException($"Location with id {id} can not be found !");

         _context.Remove(location);
         await _context.SaveChangesAsync(cancellationToken);
         return GetLocationResponse(location);
      }

      public async Task<CreateLocationResponse> UpdateLocationByIdAsync(Guid id, UpdateLocationRequest request, string? imageUrl, CancellationToken cancellationToken)
      {
         var location = await _context.Location.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

         if (location == null) throw new NotFoundException($"Location with id {id} can not be found !");

         if (request.Name != null) location.Name = request.Name;
         if (request.Country != null) location.Country = request.Country;
         if (request.Province != null) location.Province = request.Province;
         if (imageUrl != null) location.Image = imageUrl;

         var user = await _userRepository.GetUserAsync();
         location.ModifiedBy = user.Email;
         location.ModifiedDate = DateTime.Now;

         await _context.SaveChangesAsync(cancellationToken);

         return GetLocationResponse(location);
      }

      private CreateLocationResponse GetLocationResponse(Location location)
      {
         return new CreateLocationResponse()
         {
            Id = location.Id,
            Name = location.Name,
            Province = location.Province,
            Country = location.Country,
            Image = location.Image,
            CreatedDate = location.CreatedDate,
            CreatedBy = location.CreatedBy,
            ModifiedDate = location.ModifiedDate,
            ModifiedBy = location.ModifiedBy,
         };
      }
   }
}