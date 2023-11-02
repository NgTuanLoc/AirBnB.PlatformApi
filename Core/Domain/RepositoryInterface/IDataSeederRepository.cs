using Contracts;
using Core.Domain.Entities;
namespace Core.Domain.RepositoryInterface;
public interface IDataSeederRepository
{
    Task SeedingRoomRepositoryAsync(RoomMessageModel model, CancellationToken cancellationToken);
    Task<Location> SeedingLocationRepositoryAsync(LocationMessageModel model, CancellationToken cancellationToken);
    Task SendMessageToCreateRoomAsync(Location location, CancellationToken cancellationToken);
}
