using Core.Domain.Entities;
using Core.Models.PaginationModel;
using Core.Models.Room;

namespace Core.Domain.RepositoryInterface
{
   public interface IRoomRepository
   {
      Task<Room> CreateRoomAsync(CreateRoomRequest request, CancellationToken cancellationToken);
      Task<Room> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken);
      Task<PaginationResponse<CreateRoomResponse>> GetAllRoomListAsync(Guid? locationId, PagingParams pagingParams, PaginationModel paginationModel, CancellationToken cancellationToken);
      Task<Room> DeleteRoomByIdAsync(Guid id, CancellationToken cancellationToken);
      Task<Room> UpdateRoomByIdAsync(Guid id, UpdateRoomRequest request, CancellationToken cancellationToken);
   }
}