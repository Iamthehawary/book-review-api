using System.ComponentModel.DataAnnotations;

namespace BookReviewApi.Models;

public class User
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public List<Review> Reviews { get; set; } = new List<Review>();
}