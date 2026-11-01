using BookReviewApi.Data;
using BookReviewApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReviewApi.Repositories;

public class ReviewRepository(AppDbContext context) : IReviewRepository
{
    public async Task<Review?> GetByIdAsync(int id)
        => await context.Reviews
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);

    public void Delete(Review review)
        => context.Reviews.Remove(review);

    public async Task SaveChangesAsync()
        => await context.SaveChangesAsync();

    public async Task AddReviewAsync(Review review)
    {
        await context.Reviews.AddAsync(review);
        await context.SaveChangesAsync();
    }
}