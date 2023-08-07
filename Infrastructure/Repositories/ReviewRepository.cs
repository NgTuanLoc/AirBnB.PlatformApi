using Core.Domain.Entities;
using Core.Domain.RepositoryInterface;
using Core.Exceptions;
using Core.Models.Review;
using Core.Services;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
   public class ReviewRepository : IReviewRepository
   {
      private readonly ApplicationDbContext _context;
      private readonly IUserService _userService;
      public ReviewRepository(ApplicationDbContext context, IUserService userService)
      {
         _context = context;
         _userService = userService;
      }
      public async Task<Review> CreateReviewByRoomIdAsync(CreateReviewRequest request, Guid roomId, CancellationToken cancellationToken)
      {
         var existedRoom = await _context.Room.FirstOrDefaultAsync(r => r.Id == roomId, cancellationToken);

         if (existedRoom == null) throw new NotFoundException($"Room with id {roomId} is not found");

         var existedUser = await _userService.GetUserService();

         var existedReservation = await (
            from reservation in _context.Reservation
            join room in _context.Room on reservation.Room equals room
            join user in _context.Users on reservation.User equals user
            where room.Id == roomId && user.Id == existedUser.Id
            select reservation
            ).FirstOrDefaultAsync(cancellationToken);

         if (existedReservation == null) throw new ValidationException($"This user does not have a reservation for this room");

         var newReviewId = Guid.NewGuid();
         var newReview = new Review()
         {
            Id = newReviewId,
            Title = request.Title,
            Comment = request.Comment,
            Rating = request.Rating,
            User = existedUser,
            Reservation = existedReservation,
            CreatedBy = existedUser.Email ?? "Unknown",
            CreatedDate = DateTime.Now
         };

         _context.Review.Add(newReview);
         await _context.SaveChangesAsync(cancellationToken);

         return newReview;
      }

      public async Task<List<Review>> GetAllReviewsByRoomIdAsync(Guid roomId, CancellationToken cancellationToken)
      {
         var existedRoom = await _context.Room.FirstOrDefaultAsync(r => r.Id == roomId, cancellationToken);

         if (existedRoom == null) throw new NotFoundException($"Room with id {roomId} is not found");

         var reviewList = await (
            from review in _context.Review
            join reservation in _context.Reservation on review.Reservation equals reservation
            join user in _context.Users on review.User equals user
            join room in _context.Room on reservation.Room equals room
            where room.Id == roomId
            select review
         ).ToListAsync(cancellationToken);

         return reviewList;
      }

      public async Task<Review> UpdateReviewByIdAsync(UpdateReviewRequest request, Guid id, CancellationToken cancellationToken)
      {
         var existedReview = await _context.Review
         .Include("Reservation.Room")
         .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

         if (existedReview == null) throw new NotFoundException($"review with id {id} is not found");

         var user = await _userService.GetUserService();

         if (existedReview.User == null || existedReview?.User.Id != user.Id) throw new NotFoundException($"Update review failed ! Only the author can update the review");

         if (request.Title != null) existedReview.Title = request.Title;
         if (request.Comment != null) existedReview.Comment = request.Comment;
         if (request.Rating != 0) existedReview.Rating = request.Rating;

         existedReview.ModifiedBy = user.Email;
         existedReview.ModifiedDate = DateTime.Now;

         await _context.SaveChangesAsync(cancellationToken);

         return existedReview;
      }

      public async Task<Review> DeleteReviewByIdAsync(Guid id, CancellationToken cancellationToken)
      {
         var existedReview = await _context.Review
         .Include("Reservation.Room")
         .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

         if (existedReview == null) throw new NotFoundException($"review with id {id} is not found");

         var user = await _userService.GetUserService();

         if (existedReview.User == null || existedReview?.User.Id != user.Id) throw new NotFoundException($"Delete review failed ! Only the author can update the review");

         _context.Review.Remove(existedReview);
         await _context.SaveChangesAsync(cancellationToken);

         return existedReview;
      }
   }
}