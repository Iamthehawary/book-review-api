using BookReviewApi.DTOs;

namespace BookReviewApi.Services;

public interface IReviewService
{
    Task<ServiceResult<ReviewResponseDto>> AddReviewAsync(CreateReviewDto dto, int userId);
    Task<ServiceResult<ReviewResponseDto>> UpdateReviewAsync(int reviewId, UpdateReviewDto dto, int userId);
    Task<ResultStatus> DeleteReviewAsync(int reviewId, int userId);
}