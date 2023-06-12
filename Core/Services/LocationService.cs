using Core.Domain.RepositoryInterface;
using Core.Models.Location;

namespace Core.Services
{
   public interface ILocationService
   {
      Task<CreateLocationResponse> CreateLocationService(CreateLocationRequest request, CancellationToken cancellationToken);
   }
   public class LocationService : ILocationService
   {
      private readonly ILocationRepository _locationRepository;
      private readonly IImageService _imageService;
      public LocationService(ILocationRepository locationRepository, IImageService imageService)
      {
         _locationRepository = locationRepository;
         _imageService = imageService;
      }

      public async Task<CreateLocationResponse> CreateLocationService(CreateLocationRequest request, CancellationToken cancellationToken)
      {
         var imageUrlList = await _imageService.UploadImageService(request.File);
         throw new NotImplementedException();
      }
   }
}