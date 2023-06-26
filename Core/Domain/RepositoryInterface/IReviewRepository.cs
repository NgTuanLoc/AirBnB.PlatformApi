using Core.Models.Review;

namespace Core.Domain.RepositoryInterface
{
   public interface IReviewRepository
   {
      Task<CreateReviewResponse> CreateReviewByRoomIdAsync(CreateReviewRequest request, Guid roomId, CancellationToken cancellationToken);
      Task<List<CreateReviewResponse>> GetAllReviewsByRoomIdAsync(Guid roomId, CancellationToken cancellationToken);
      Task<CreateReviewResponse> UpdateReviewByIdAsync(UpdateReviewRequest request, Guid id, CancellationToken cancellationToken);
      Task<CreateReviewResponse> DeleteReviewByIdAsync(Guid id, CancellationToken cancellationToken);
   }
}