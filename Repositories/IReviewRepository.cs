using BookReviewApi.Models;

namespace BookReviewApi.Repositories;

public interface IReviewRepository
{
    Task AddReviewAsync(Review review);
    Task<Review?> GetByIdAsync(int id);   // now includes the review's User
    void Delete(Review review);
    Task SaveChangesAsync();
}