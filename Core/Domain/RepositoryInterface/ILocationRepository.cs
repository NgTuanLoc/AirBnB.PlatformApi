using Core.Domain.Entities;
using Core.Models.Location;

namespace Core.Domain.RepositoryInterface
{
   public interface ILocationRepository
   {
      Task<Location> CreateLocationAsync(CreateLocationRequest request, string? imageUrl, CancellationToken cancellationToken);
      Task<Location> GetLocationByIdAsync(Guid id, CancellationToken cancellationToken);
      Task<List<Location>> GetAllLocationAsync(CancellationToken cancellationToken);
      Task<Location> DeleteLocationByIdAsync(Guid id, CancellationToken cancellationToken);
      Task<Location> UpdateLocationByIdAsync(Guid id, UpdateLocationRequest request, string? imageUrl, CancellationToken cancellationToken);
   }
}