using Core.Domain.Entities;
using Core.Domain.RepositoryInterface;
using Core.Models.Room;
using Core.Utils;

namespace Core.Services
{
   public interface IRoomService
   {
      Task<CreateRoomResponse> CreateRoomService(CreateRoomRequest request, CancellationToken cancellationToken);
      Task<CreateRoomResponse> GetRoomByIdService(Guid id, CancellationToken cancellationToken);
      Task<List<CreateRoomResponse>> GetAllRoomListService(CancellationToken cancellationToken);
      Task<CreateRoomResponse> DeleteRoomByIdService(Guid id, CancellationToken cancellationToken);
      Task<CreateRoomResponse> UpdateRoomByIdService(Guid id, UpdateRoomRequest request, CancellationToken cancellationToken);
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
      public async Task<CreateRoomResponse> GetRoomByIdService(Guid id, CancellationToken cancellationToken)
      {
         var result = await _roomRepository.GetRoomByIdAsync(id, cancellationToken);

         return ConvertEntityIntoResponse.GetRoomResponse(result);
      }

      public async Task<List<CreateRoomResponse>> GetAllRoomListService(CancellationToken cancellationToken)
      {
         var roomList = await _roomRepository.GetAllRoomListAsync(cancellationToken);
         var response = new List<CreateRoomResponse>();

         foreach (var room in roomList)
         {
            response.Add(ConvertEntityIntoResponse.GetRoomResponse(room));
         }

         return response;
      }

      public async Task<CreateRoomResponse> DeleteRoomByIdService(Guid id, CancellationToken cancellationToken)
      {
         var result = await _roomRepository.DeleteRoomByIdAsync(id, cancellationToken);
         return ConvertEntityIntoResponse.GetRoomResponse(result);
      }

      public async Task<CreateRoomResponse> UpdateRoomByIdService(Guid id, UpdateRoomRequest request, CancellationToken cancellationToken)
      {
         var result = await _roomRepository.UpdateRoomByIdAsync(id, request, cancellationToken);
         return ConvertEntityIntoResponse.GetRoomResponse(result);
      }
   }
}