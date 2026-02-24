using Harfien.Application.DTO.Review;
using Harfien.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Harfien.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
        {
           var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            try
            {
                var result = await _reviewService.AddReviewAsync(dto, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("craftsman/{craftsmanId}/all")]
        public async Task<IActionResult> GetAllCraftsmanReviews(int craftsmanId)
        {
            var reviews = await _reviewService.GetReviewsByCraftsmanIdAsync(craftsmanId);
            return Ok(reviews);
        }

        [HttpGet("craftsman/{craftsmanId}")]
        public async Task<IActionResult> GetCraftsmanReviews(int craftsmanId,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var result = await _reviewService.GetPagedReviewsByCraftsmanIdAsync(craftsmanId, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
