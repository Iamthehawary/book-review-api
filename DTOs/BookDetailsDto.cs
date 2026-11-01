namespace BookReviewApi.DTOs;

public class BookDetailsDto : BookResponseDto
{
    public List<ReviewResponseDto> Reviews { get; set; } = new();
}