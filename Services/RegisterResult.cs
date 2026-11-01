using BookReviewApi.DTOs;

namespace BookReviewApi.Services;

public record RegisterResult(RegisterResponseDto? User, string? Error)
{
    public bool Succeeded => Error is null;
}