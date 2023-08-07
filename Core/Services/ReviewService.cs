using AutoMapper;
using Core.Domain.Entities;
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
      private readonly IMapper _mapper;
      public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
      {
         _reviewRepository = reviewRepository;
         _mapper = mapper;
      }
      public async Task<CreateReviewResponse> CreateReviewByRoomIdService(CreateReviewRequest request, Guid roomId, CancellationToken cancellationToken)
      {
         var createdReview = await _reviewRepository.CreateReviewByRoomIdAsync(request, roomId, cancellationToken);
         var response = _mapper.Map<Review, CreateReviewResponse>(createdReview);

         return response;
      }

      public async Task<List<CreateReviewResponse>> GetAllReviewsByRoomIdService(Guid roomId, CancellationToken cancellationToken)
      {
         var reviewList = await _reviewRepository.GetAllReviewsByRoomIdAsync(roomId, cancellationToken);
         var response = new List<CreateReviewResponse>();

         foreach (var review in reviewList)
         {
            response.Add(_mapper.Map<Review, CreateReviewResponse>(review));
         }

         return response;
      }

      public async Task<CreateReviewResponse> UpdateReviewByIdService(UpdateReviewRequest request, Guid id, CancellationToken cancellationToken)
      {
         var updatedReview = await _reviewRepository.UpdateReviewByIdAsync(request, id, cancellationToken);
         var response = _mapper.Map<Review, CreateReviewResponse>(updatedReview);

         return response;
      }

      public async Task<CreateReviewResponse> DeleteReviewByIdService(Guid id, CancellationToken cancellationToken)
      {
         var deletedReview = await _reviewRepository.DeleteReviewByIdAsync(id, cancellationToken);
         var response = _mapper.Map<Review, CreateReviewResponse>(deletedReview);

         return response;
      }
   }
}