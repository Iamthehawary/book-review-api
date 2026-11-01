using System.ComponentModel.DataAnnotations;

namespace BookReviewApi.Models;

public class Book
{
    public int Id { get; set; }

    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Author { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    public List<Review> Reviews { get; set; } = new List<Review>();
}