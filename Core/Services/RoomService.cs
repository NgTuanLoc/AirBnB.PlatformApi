using Core.Domain.Entities;
using Core.Domain.RepositoryInterface;
using Core.Models.Room;
using Core.Utils;

namespace Core.Services
{
   public interface IRoomService
   {
      Task<CreateRoomResponse> CreateRoomService(CreateRoomRequest request, CancellationToken cancellationToken);
   }
   public class RoomService : IRoomService
   {
      private readonly IRoomRepository _roomRepository;
      public RoomService(IRoomRepository roomRepository)
      {
         _roomRepository = roomRepository;
      }
      public async Task<CreateRoomResponse> CreateRoomService(CreateRoomRequest request, CancellationToken cancellationToken)
      {
         var result = await _roomRepository.CreateRoomAsync(request, cancellationToken);

         return ConvertEntityIntoResponse.GetRoomResponse(result);
      }
   }
}