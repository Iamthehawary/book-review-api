using BookReviewApi.DTOs;
using BookReviewApi.Models;
using BookReviewApi.Repositories;

namespace BookReviewApi.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;

    public BookService(IBookRepository repository) => _repository = repository;

    public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync()
    {
        var books = await _repository.GetAllAsync();
        return books.Select(ToBookResponse).ToList();
    }

    public async Task<BookDetailsDto?> GetBookByIdAsync(int id)
    {
        var book = await _repository.GetByIdAsync(id);
        if (book == null) return null;

        var summary = ToBookResponse(book);

        return new BookDetailsDto
        {
            Id = summary.Id,
            Title = summary.Title,
            Author = summary.Author,
            Description = summary.Description,
            AverageRating = summary.AverageRating,
            ReviewCount = summary.ReviewCount,
            Reviews = book.Reviews
                .OrderByDescending(r => r.CreatedAt)
                .Select(ReviewMapper.ToDto)
                .ToList()
        };
    }

    public async Task<BookResponseDto> CreateBookAsync(CreateBookDto dto)
    {
        var book = new Book
        {
            Title = dto.Title.Trim(),
            Author = dto.Author.Trim(),
            Description = dto.Description.Trim()
        };

        await _repository.AddAsync(book);
        await _repository.SaveChangesAsync();

        return ToBookResponse(book);
    }

    private static BookResponseDto ToBookResponse(Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        Description = book.Description,
        ReviewCount = book.Reviews.Count,
        AverageRating = book.Reviews.Count == 0
            ? 0
            : Math.Round(book.Reviews.Average(r => r.Rating), 1)
    };
}