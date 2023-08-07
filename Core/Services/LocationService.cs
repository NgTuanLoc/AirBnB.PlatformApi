using AutoMapper;
using Core.Domain.Entities;
using Core.Domain.RepositoryInterface;
using Core.Models.Location;
using Core.Utils;
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
      private readonly IMapper _mapper;
      public LocationService(ILocationRepository locationRepository, IImageService imageService, IMapper mapper)
      {
         _locationRepository = locationRepository;
         _imageService = imageService;
         _mapper = mapper;
      }

      public async Task<CreateLocationResponse> CreateLocationService(CreateLocationRequest request, CancellationToken cancellationToken)
      {
         var imageUrlList = await _imageService.UploadImageService(request.File);
         var createdLocation = await _locationRepository.CreateLocationAsync(request, imageUrlList.highQualityUrl, cancellationToken);
         var response = _mapper.Map<Location, CreateLocationResponse>(createdLocation);
         return response;
      }

      public async Task<CreateLocationResponse> GetLocationByIdService(Guid id, CancellationToken cancellationToken)
      {
         var location = await _locationRepository.GetLocationByIdAsync(id, cancellationToken);
         var response = _mapper.Map<Location, CreateLocationResponse>(location);
         return response;
      }

      public async Task<List<CreateLocationResponse>> GetAllLocationService(CancellationToken cancellationToken)
      {
         var result = await _locationRepository.GetAllLocationAsync(cancellationToken);

         var response = new List<CreateLocationResponse>();

         foreach (var location in result)
         {
            response.Add(_mapper.Map<Location, CreateLocationResponse>(location));
         }
         return response;
      }

      public async Task<CreateLocationResponse> DeleteLocationByIdService(Guid id, CancellationToken cancellationToken)
      {
         var deletedLocation = await _locationRepository.DeleteLocationByIdAsync(id, cancellationToken);
         var response = _mapper.Map<Location, CreateLocationResponse>(deletedLocation);
         return response;
      }

      public async Task<CreateLocationResponse> UpdateLocationByIdService(Guid id, UpdateLocationRequest request, CancellationToken cancellationToken)
      {
         string? imageUrl = null;

         if (request.File != null)
         {
            var imageUrlList = await _imageService.UploadImageService(request.File);
            imageUrl = imageUrlList.highQualityUrl;
         }

         var updatedLocation = await _locationRepository.UpdateLocationByIdAsync(id, request, imageUrl, cancellationToken);

         var response = _mapper.Map<Location, CreateLocationResponse>(updatedLocation);
         return response;
      }
   }
}