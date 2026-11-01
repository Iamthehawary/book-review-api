namespace BookReviewApi.Models;

public class Review
{
    public int Id { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 1. العلاقة مع اليوزر (صاحب المراجعة)
    public int UserId { get; set; }
    public User? User { get; set; }

    // 2. العلاقة مع الكتاب (المراجعة مكتوبة عن أنهي كتاب؟)
    public int BookId { get; set; }
    public Book? Book { get; set; }
}