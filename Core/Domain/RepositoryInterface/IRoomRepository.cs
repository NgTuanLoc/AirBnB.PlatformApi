using Core.Domain.Entities;
using Core.Models.Room;

namespace Core.Domain.RepositoryInterface
{
   public interface IRoomRepository
   {
      Task<Room> CreateRoomAsync(CreateRoomRequest request, CancellationToken cancellationToken);
   }
}