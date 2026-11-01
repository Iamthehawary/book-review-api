using BookReviewApi.DTOs;
using BookReviewApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookReviewApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReviewsController(IReviewService reviewService) : ControllerBase
{
    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Add(CreateReviewDto dto)
    {
        var result = await reviewService.AddReviewAsync(dto, CurrentUserId);

        if (result.Status == ResultStatus.NotFound)
            return NotFound(new { message = "Book not found" });

        return StatusCode(StatusCodes.Status201Created, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateReviewDto dto)
    {
        var result = await reviewService.UpdateReviewAsync(id, dto, CurrentUserId);

        return result.Status switch
        {
            ResultStatus.NotFound => NotFound(new { message = "Review not found" }),
            ResultStatus.Forbidden => StatusCode(StatusCodes.Status403Forbidden,
                new { message = "You can only edit your own reviews" }),
            _ => Ok(result.Data)
        };
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var status = await reviewService.DeleteReviewAsync(id, CurrentUserId);

        return status switch
        {
            ResultStatus.NotFound => NotFound(new { message = "Review not found" }),
            ResultStatus.Forbidden => StatusCode(StatusCodes.Status403Forbidden,
                new { message = "You can only delete your own reviews" }),
            _ => NoContent()
        };
    }
}