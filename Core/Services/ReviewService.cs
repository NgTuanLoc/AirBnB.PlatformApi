using Core.Domain.RepositoryInterface;
using Core.Models.Review;
using Core.Models.Room;

namespace Core.Services
{
   public interface IReviewService
   {
      Task<CreateReviewResponse> CreateReviewByRoomIdService(CreateReviewRequest request, Guid roomId, CancellationToken cancellationToken);
      Task<List<CreateReviewResponse>> GetAllReviewsByRoomIdService(Guid roomId, CancellationToken cancellationToken);
      Task<CreateReviewResponse> UpdateReviewByIdService(UpdateReviewRequest request, Guid id, CancellationToken cancellationToken);
      Task<CreateReviewResponse> DeleteReviewByIdService(Guid id, CancellationToken cancellationToken);
   }
   public class ReviewService : IReviewService
   {
      private readonly IReviewRepository _reviewRepository;
      public ReviewService(IReviewRepository reviewRepository)
      {
         _reviewRepository = reviewRepository;
      }
      public async Task<CreateReviewResponse> CreateReviewByRoomIdService(CreateReviewRequest request, Guid roomId, CancellationToken cancellationToken)
      {
         var result = await _reviewRepository.CreateReviewByRoomIdAsync(request, roomId, cancellationToken);
         return result;
      }

      public async Task<List<CreateReviewResponse>> GetAllReviewsByRoomIdService(Guid roomId, CancellationToken cancellationToken)
      {
         var result = await _reviewRepository.GetAllReviewsByRoomIdAsync(roomId, cancellationToken);
         return result;
      }

      public async Task<CreateReviewResponse> UpdateReviewByIdService(UpdateReviewRequest request, Guid id, CancellationToken cancellationToken)
      {
         var result = await _reviewRepository.UpdateReviewByIdAsync(request, id, cancellationToken);
         return result;
      }

      public async Task<CreateReviewResponse> DeleteReviewByIdService(Guid id, CancellationToken cancellationToken)
      {
         var result = await _reviewRepository.DeleteReviewByIdAsync(id, cancellationToken);
         return result;
      }
   }
}