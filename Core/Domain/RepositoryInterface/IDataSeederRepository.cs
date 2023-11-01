using Contracts;
namespace Core.Domain.RepositoryInterface;
public interface IDataSeederRepository
{
    Task SeedingRoomRepositoryAsync(RoomMessageModel model, CancellationToken cancellationToken);
    Task SeedingLocationRepositoryAsync(LocationMessageModel model, CancellationToken cancellationToken);
}
