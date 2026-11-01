using BookReviewApi.DTOs;
using BookReviewApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _service;

    public BooksController(IBookService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _service.GetAllBooksAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _service.GetBookByIdAsync(id);
        return book == null ? NotFound(new { message = "Book not found" }) : Ok(book);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateBookDto dto)
    {
        var createdBook = await _service.CreateBookAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
    }
}