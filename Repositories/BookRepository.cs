using BookReviewApi.Data;
using BookReviewApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReviewApi.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Book>> GetAllAsync() =>
        await _context.Books
            .AsNoTracking()
            .Include(b => b.Reviews)
            .ToListAsync();

    public async Task<Book?> GetByIdAsync(int id) =>
        await _context.Books
            .AsNoTracking()
            .Include(b => b.Reviews)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<bool> ExistsAsync(int id) =>
        await _context.Books.AnyAsync(b => b.Id == id);

    public async Task AddAsync(Book book) => await _context.Books.AddAsync(book);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
