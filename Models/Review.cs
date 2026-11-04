namespace BookReviewApi.Models;

public class Review
{
    public int Id { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 1. Relationship with the user (the author of the review)
    public int UserId { get; set; }
    public User? User { get; set; }

    // 2. Relationship with the book (which book the review is about)
    public int BookId { get; set; }
    public Book? Book { get; set; }
}