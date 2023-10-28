using Core.Domain.Entities;
using Core.Models.Reservation;

namespace Core.Domain.RepositoryInterface
{
   public interface IReservationRepository
   {
      Task<Reservation> CreateReservationAsync(CreateReservationRequest request, CancellationToken cancellationToken);
      Task<List<Reservation>> GetAllReservationByRoomIdAsync(Guid roomId, CancellationToken cancellationToken);
      Task<Reservation> UpdateReservationByIdAsync(Guid roomId, UpdateReservationRequest request, CancellationToken cancellationToken);
      Task<Reservation> DeleteReservationByIdAsync(Guid id, CancellationToken cancellationToken);
   }
}