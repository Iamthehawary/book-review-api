using BookReviewApi.DTOs;
using BookReviewApi.Models;
using BookReviewApi.Repositories;

namespace BookReviewApi.Services;

public class ReviewService(IReviewRepository reviewRepo, IBookRepository bookRepo) : IReviewService
{
    public async Task<ServiceResult<ReviewResponseDto>> AddReviewAsync(CreateReviewDto dto, int userId)
    {
        if (!await bookRepo.ExistsAsync(dto.BookId))
            return ServiceResult<ReviewResponseDto>.NotFound();

        var review = new Review
        {
            Comment = dto.Comment.Trim(),
            Rating = dto.Rating,
            BookId = dto.BookId,
            UserId = userId
        };

        await reviewRepo.AddReviewAsync(review);

        // Reload so the User navigation is filled in (we need the username)
        var saved = await reviewRepo.GetByIdAsync(review.Id);

        return ServiceResult<ReviewResponseDto>.Ok(ReviewMapper.ToDto(saved!));
    }

    public async Task<ServiceResult<ReviewResponseDto>> UpdateReviewAsync(int reviewId, UpdateReviewDto dto, int userId)
    {
        var review = await reviewRepo.GetByIdAsync(reviewId);

        if (review == null)
            return ServiceResult<ReviewResponseDto>.NotFound();

        if (review.UserId != userId)
            return ServiceResult<ReviewResponseDto>.Forbidden();

        review.Comment = dto.Comment.Trim();
        review.Rating = dto.Rating;

        await reviewRepo.SaveChangesAsync();

        return ServiceResult<ReviewResponseDto>.Ok(ReviewMapper.ToDto(review));
    }

    public async Task<ResultStatus> DeleteReviewAsync(int reviewId, int userId)
    {
        var review = await reviewRepo.GetByIdAsync(reviewId);

        if (review == null)
            return ResultStatus.NotFound;

        if (review.UserId != userId)
            return ResultStatus.Forbidden;

        reviewRepo.Delete(review);
        await reviewRepo.SaveChangesAsync();

        return ResultStatus.Success;
    }
}