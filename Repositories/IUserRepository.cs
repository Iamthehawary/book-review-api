using BookReviewApi.Models;

namespace BookReviewApi.Repositories;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UsernameExistsAsync(string username);
}