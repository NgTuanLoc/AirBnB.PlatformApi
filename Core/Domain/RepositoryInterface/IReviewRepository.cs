using Core.Domain.Entities;
using Core.Models.Review;

namespace Core.Domain.RepositoryInterface
{
   public interface IReviewRepository
   {
      Task<Review> CreateReviewByRoomIdAsync(CreateReviewRequest request, Guid roomId, CancellationToken cancellationToken);
      Task<List<Review>> GetAllReviewsByRoomIdAsync(Guid roomId, CancellationToken cancellationToken);
      Task<Review> UpdateReviewByIdAsync(UpdateReviewRequest request, Guid id, CancellationToken cancellationToken);
      Task<Review> DeleteReviewByIdAsync(Guid id, CancellationToken cancellationToken);
   }
}