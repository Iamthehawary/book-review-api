namespace BookReviewApi.DTOs;

public class ReviewResponseDto
{
    public int Id { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }

    public int BookId { get; set; }

    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
}