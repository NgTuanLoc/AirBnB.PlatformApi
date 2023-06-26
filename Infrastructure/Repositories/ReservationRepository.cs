using Core.Domain.Entities;
using Core.Domain.RepositoryInterface;
using Core.Exceptions;
using Core.Models.Reservation;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
   public class ReservationRepository : IReservationRepository
   {
      private readonly ApplicationDbContext _context;
      public ReservationRepository(ApplicationDbContext context)
      {
         _context = context;
      }
      public async Task<CreateReservationResponse> CreateReservationAsync(CreateReservationRequest request, CancellationToken cancellationToken)
      {
         DateRange dateRange = new DateRange()
         {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
         };

         var room = await _context.Room.FirstOrDefaultAsync(r => r.Id == request.RoomId, cancellationToken);
         var guest = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

         if (room == null)
         {
            throw new NotFoundException($"Room with id {request.RoomId} is not found !");
         }

         if (guest == null)
         {
            throw new NotFoundException($"Guest with id {request.UserId} is not found !");
         }

         var totalPrice = CalculatePrice(room.Price, dateRange, request.ToTalGuests);

         var reservationId = Guid.NewGuid();
         var newReservation = new Reservation()
         {
            Id = reservationId,
            User = guest,
            Room = room,
            StartDate = dateRange.StartDate,
            EndDate = dateRange.EndDate,
            Price = totalPrice,
            Total = request.ToTalGuests,
            CreatedBy = guest.Email ?? "Unknown User",
            CreatedDate = DateTime.Now
         };

         _context.Reservation.Add(newReservation);
         await _context.SaveChangesAsync(cancellationToken);

         return new CreateReservationResponse()
         {
            Id = reservationId,
            CustomerEmail = guest.Email ?? "Unknown Email",
            RoomName = room.Name,
            StartDate = dateRange.StartDate,
            EndDate = dateRange.EndDate,
            TotalGuest = request.ToTalGuests,
            TotalPrice = totalPrice
         };
      }

      public async Task<CreateReservationResponse> DeleteReservationByIdAsync(Guid id, CancellationToken cancellationToken)
      {
         var reservation = await _context.Reservation
            .Include(r => r.Room)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

         if (reservation == null) throw new NotFoundException($"Reservation with id {id} is not found !");

         _context.Reservation.Remove(reservation);

         await _context.SaveChangesAsync(cancellationToken);

         return new CreateReservationResponse()
         {
            Id = reservation.Id,
            CustomerEmail = reservation?.User?.Email ?? "Unknown Email",
            RoomName = reservation?.Room?.Name ?? "Unknown Room",
            StartDate = reservation.StartDate,
            EndDate = reservation.EndDate,
            TotalGuest = reservation.Total,
            TotalPrice = reservation.Price
         };
      }

      public async Task<List<CreateReservationResponse>> GetAllReservationByRoomIdAsync(Guid roomId, CancellationToken cancellationToken)
      {
         var existedRoom = await _context.Room.FirstOrDefaultAsync(r => r.Id == roomId, cancellationToken);

         if (existedRoom == null) throw new NotFoundException($"Room with id {roomId} is not found!");


         var reservationList = await (from reservation in _context.Reservation
                                      join room in _context.Room on reservation.Room equals room
                                      join user in _context.Users on reservation.User equals user
                                      where room.Id == roomId
                                      select new CreateReservationResponse()
                                      {
                                         Id = reservation.Id,
                                         CustomerEmail = user.Email ?? "Unknown Email",
                                         RoomName = room.Name,
                                         StartDate = reservation.StartDate,
                                         EndDate = reservation.EndDate,
                                         TotalGuest = reservation.Total,
                                         TotalPrice = reservation.Price
                                      })
                                      .ToListAsync(cancellationToken);
         return reservationList;
      }
      public async Task<CreateReservationResponse> UpdateReservationByIdAsync(Guid id, UpdateReservationRequest request, CancellationToken cancellationToken)
      {
         var existedReservation = await _context.Reservation
            .Include(r => r.User)
            .Include(r => r.Room)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

         if (existedReservation == null) throw new NotFoundException($"Reservation with id {id} is not found !");

         if (request.StartDate != null)
         {
            existedReservation.StartDate = request.StartDate;
         }
         if (request.EndDate != null)
         {
            existedReservation.EndDate = request.EndDate;
         }
         if (request.ToTalGuests != 0)
         {
            existedReservation.Total = request.ToTalGuests;
         }

         DateRange dateRange = new DateRange()
         {
            StartDate = existedReservation.StartDate,
            EndDate = existedReservation.EndDate,
         };
         existedReservation.Price = CalculatePrice(existedReservation.Price, dateRange, existedReservation.Total);
         existedReservation.ModifiedBy = existedReservation?.User?.Email ?? "Unknown User";
         existedReservation.ModifiedDate = DateTime.Now;

         await _context.SaveChangesAsync(cancellationToken);

         return new CreateReservationResponse()
         {
            Id = existedReservation.Id,
            CustomerEmail = existedReservation?.User?.Email ?? "Unknown Email",
            RoomName = existedReservation?.Room?.Name ?? "Unknown Room",
            StartDate = existedReservation.StartDate,
            EndDate = existedReservation.EndDate,
            TotalGuest = existedReservation.Total,
            TotalPrice = existedReservation.Price
         };
      }
      private float CalculatePrice(float basePrice, DateRange dateRange, float totalGuests)
      {
         return basePrice * totalGuests;
      }
   }
}