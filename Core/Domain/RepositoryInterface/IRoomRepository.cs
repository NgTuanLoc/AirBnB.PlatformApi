using Core.Domain.Entities;
using Core.Models.Room;

namespace Core.Domain.RepositoryInterface
{
   public interface IRoomRepository
   {
      Task<Room> CreateRoomAsync(CreateRoomRequest request, CancellationToken cancellationToken);
      Task<Room> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken);
      Task<List<Room>> GetAllRoomListAsync(CancellationToken cancellationToken);
      Task<Room> DeleteRoomByIdAsync(Guid id, CancellationToken cancellationToken);
      Task<Room> UpdateRoomByIdAsync(Guid id, UpdateRoomRequest request, CancellationToken cancellationToken);
   }
}