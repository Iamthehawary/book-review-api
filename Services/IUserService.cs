using BookReviewApi.DTOs;

namespace BookReviewApi.Services
{
    public interface IUserService
    {
        Task<RegisterResult> RegisterAsync(RegisterDto dto);
        Task<string?> LoginAsync(LoginDto dto);
    }
}