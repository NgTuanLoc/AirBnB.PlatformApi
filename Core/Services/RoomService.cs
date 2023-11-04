using AutoMapper;
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
      Task<List<CreateRoomResponse>> GetAllRoomListService(Guid? locationId, CancellationToken cancellationToken);
      Task<CreateRoomResponse> DeleteRoomByIdService(Guid id, CancellationToken cancellationToken);
      Task<CreateRoomResponse> UpdateRoomByIdService(Guid id, UpdateRoomRequest request, CancellationToken cancellationToken);
   }
   public class RoomService : IRoomService
   {
      private readonly IRoomRepository _roomRepository;
      private readonly IMapper _mapper;
      public RoomService(IRoomRepository roomRepository, IMapper mapper)
      {
         _roomRepository = roomRepository;
         _mapper = mapper;
      }
      public async Task<CreateRoomResponse> CreateRoomService(CreateRoomRequest request, CancellationToken cancellationToken)
      {
         var createdRoom = await _roomRepository.CreateRoomAsync(request, cancellationToken);
         var response = _mapper.Map<Room, CreateRoomResponse>(createdRoom);
         return response;
      }
      public async Task<CreateRoomResponse> GetRoomByIdService(Guid id, CancellationToken cancellationToken)
      {
         var room = await _roomRepository.GetRoomByIdAsync(id, cancellationToken);
         var response = _mapper.Map<Room, CreateRoomResponse>(room);

         return response;
      }

      public async Task<List<CreateRoomResponse>> GetAllRoomListService(Guid? locationId, CancellationToken cancellationToken)
      {
         var roomList = await _roomRepository.GetAllRoomListAsync(locationId, cancellationToken);
         var response = new List<CreateRoomResponse>();

         foreach (var room in roomList)
         {
            response.Add(_mapper.Map<Room, CreateRoomResponse>(room));
         }

         return response;
      }

      public async Task<CreateRoomResponse> DeleteRoomByIdService(Guid id, CancellationToken cancellationToken)
      {
         var deletedRoom = await _roomRepository.DeleteRoomByIdAsync(id, cancellationToken);
         var response = _mapper.Map<Room, CreateRoomResponse>(deletedRoom);
         return response;
      }

      public async Task<CreateRoomResponse> UpdateRoomByIdService(Guid id, UpdateRoomRequest request, CancellationToken cancellationToken)
      {
         var updatedRoom = await _roomRepository.UpdateRoomByIdAsync(id, request, cancellationToken);
         var response = _mapper.Map<Room, CreateRoomResponse>(updatedRoom);
         return response;
      }
   }
}