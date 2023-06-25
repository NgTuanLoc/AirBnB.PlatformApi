using Core.Models.Reservation;

namespace Core.Domain.RepositoryInterface
{
   public interface IReservationRepository
   {
      Task<CreateReservationResponse> CreateReservationAsync(CreateReservationRequest request, CancellationToken cancellationToken);
      Task<List<CreateReservationResponse>> GetAllReservationByRoomIdAsync(Guid roomId, CancellationToken cancellationToken);
      Task<CreateReservationResponse> UpdateReservationByIdAsync(Guid roomId, UpdateReservationRequest request, CancellationToken cancellationToken);
      Task<CreateReservationResponse> DeleteReservationByIdAsync(Guid id, CancellationToken cancellationToken);
   }
}