using AutoMapper;
using Core.Domain.Entities;
using Core.Domain.RepositoryInterface;
using Core.Models.Reservation;

namespace Core.Services
{
   public interface IReservationService
   {
      Task<CreateReservationResponse> CreateReservationService(CreateReservationRequest request, CancellationToken cancellationToken);
      Task<List<CreateReservationResponse>> GetAllReservationByRoomIdService(Guid roomId, CancellationToken cancellationToken);
      Task<CreateReservationResponse> DeleteReservationByIdService(Guid id, CancellationToken cancellationToken);
      Task<CreateReservationResponse> UpdateReservationByIdService(Guid id, UpdateReservationRequest request, CancellationToken cancellationToken);
   }
   public class ReservationService : IReservationService
   {
      private readonly IReservationRepository _reservationRepository;
      private readonly IMapper _mapper;

      public ReservationService(IReservationRepository reservationRepository, IMapper mapper)
      {
         _reservationRepository = reservationRepository;
         _mapper = mapper;
      }
      public async Task<CreateReservationResponse> CreateReservationService(CreateReservationRequest request, CancellationToken cancellationToken)
      {
         var result = await _reservationRepository.CreateReservationAsync(request, cancellationToken);
         return _mapper.Map<Reservation, CreateReservationResponse>(result);
      }
      public async Task<List<CreateReservationResponse>> GetAllReservationByRoomIdService(Guid roomId, CancellationToken cancellationToken)
      {
         var result = await _reservationRepository.GetAllReservationByRoomIdAsync(roomId, cancellationToken);

         var response = new List<CreateReservationResponse>();
         foreach (var room in result)
         {
            response.Add(_mapper.Map<Reservation, CreateReservationResponse>(room));
         };
         return response;
      }

      public async Task<CreateReservationResponse> DeleteReservationByIdService(Guid id, CancellationToken cancellationToken)
      {
         var result = await _reservationRepository.DeleteReservationByIdAsync(id, cancellationToken);
         return _mapper.Map<Reservation, CreateReservationResponse>(result);
      }

      public async Task<CreateReservationResponse> UpdateReservationByIdService(Guid id, UpdateReservationRequest request, CancellationToken cancellationToken)
      {
         var result = await _reservationRepository.UpdateReservationByIdAsync(id, request, cancellationToken);
         return _mapper.Map<Reservation, CreateReservationResponse>(result);
      }
   }
}