using Core.Domain.RepositoryInterface;
using Core.Models.Location;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Core.Services
{
   public interface ILocationService
   {
      Task<CreateLocationResponse> CreateLocationService(CreateLocationRequest request, CancellationToken cancellationToken);
      Task<CreateLocationResponse> GetLocationByIdService(Guid id, CancellationToken cancellationToken);
      Task<List<CreateLocationResponse>> GetAllLocationService(CancellationToken cancellationToken);
      Task<CreateLocationResponse> DeleteLocationByIdService(Guid id, CancellationToken cancellationToken);
      Task<CreateLocationResponse> UpdateLocationByIdService(Guid id, UpdateLocationRequest request, CancellationToken cancellationToken);
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
         var result = await _locationRepository.CreateLocationAsync(request, imageUrlList.highQualityUrl, cancellationToken);
         return result;
      }

      public async Task<CreateLocationResponse> GetLocationByIdService(Guid id, CancellationToken cancellationToken)
      {
         var result = await _locationRepository.GetLocationByIdAsync(id, cancellationToken);
         return result;
      }

      public async Task<List<CreateLocationResponse>> GetAllLocationService(CancellationToken cancellationToken)
      {
         var result = await _locationRepository.GetAllLocationAsync(cancellationToken);
         return result;
      }

      public async Task<CreateLocationResponse> DeleteLocationByIdService(Guid id, CancellationToken cancellationToken)
      {
         var result = await _locationRepository.DeleteLocationByIdAsync(id, cancellationToken);
         return result;
      }

      public async Task<CreateLocationResponse> UpdateLocationByIdService(Guid id, UpdateLocationRequest request, CancellationToken cancellationToken)
      {
         string? imageUrl = null;

         if (request.File != null)
         {
            var imageUrlList = await _imageService.UploadImageService(request.File);
            imageUrl = imageUrlList.highQualityUrl;
         }

         var result = await _locationRepository.UpdateLocationByIdAsync(id, request, imageUrl, cancellationToken);
         return result;
      }
   }
}