using BookReviewApi.Models;

namespace BookReviewApi.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();   // books + their reviews (for average and count)
    Task<Book?> GetByIdAsync(int id);        // book + reviews + each review's user
    Task<bool> ExistsAsync(int id);
    Task AddAsync(Book book);
    Task SaveChangesAsync();
}