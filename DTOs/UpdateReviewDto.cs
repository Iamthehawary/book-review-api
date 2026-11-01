using System.ComponentModel.DataAnnotations;

namespace BookReviewApi.DTOs;

public class UpdateReviewDto
{
    [Required(ErrorMessage = "Comment is required")]
    [StringLength(1000, ErrorMessage = "Comment must be 1000 characters or less")]
    public string Comment { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }
}