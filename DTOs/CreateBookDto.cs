using System.ComponentModel.DataAnnotations;

namespace BookReviewApi.DTOs;

public class CreateBookDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title must be 200 characters or less")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Author is required")]
    [StringLength(200, ErrorMessage = "Author must be 200 characters or less")]
    public string Author { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Description must be 2000 characters or less")]
    public string Description { get; set; } = string.Empty;
}