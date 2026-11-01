using BookReviewApi.DTOs;

namespace BookReviewApi.Services;

public interface IBookService
{
    Task<IEnumerable<BookResponseDto>> GetAllBooksAsync();
    Task<BookDetailsDto?> GetBookByIdAsync(int id);
    Task<BookResponseDto> CreateBookAsync(CreateBookDto dto);
}