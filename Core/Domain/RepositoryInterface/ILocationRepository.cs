using Core.Models.Location;

namespace Core.Domain.RepositoryInterface
{
   public interface ILocationRepository
   {
      Task<CreateLocationResponse> CreateLocationAsync(CreateLocationRequest request, string? imageUrl, CancellationToken cancellationToken);
      Task<CreateLocationResponse> GetLocationByIdAsync(Guid id, CancellationToken cancellationToken);
      Task<List<CreateLocationResponse>> GetAllLocationAsync(CancellationToken cancellationToken);
      Task<CreateLocationResponse> DeleteLocationByIdAsync(Guid id, CancellationToken cancellationToken);
      Task<CreateLocationResponse> UpdateLocationByIdAsync(Guid id, UpdateLocationRequest request, string? imageUrl, CancellationToken cancellationToken);
   }
}