using BookReviewApi.DTOs;
using BookReviewApi.Models;

namespace BookReviewApi.Services;

public static class ReviewMapper
{
    public static ReviewResponseDto ToDto(Review review) => new()
    {
        Id = review.Id,
        Comment = review.Comment,
        Rating = review.Rating,
        CreatedAt = review.CreatedAt,
        BookId = review.BookId,
        UserId = review.UserId,
        Username = review.User?.Username ?? string.Empty
    };
}