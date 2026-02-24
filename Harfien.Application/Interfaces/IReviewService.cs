using Harfien.Application.DTO.Review;
using Harfien.Domain.Shared;

namespace Harfien.Application.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewDto> AddReviewAsync(CreateReviewDto dto, String currentUserId);
        Task<IEnumerable<ReviewDto>> GetReviewsByCraftsmanIdAsync(int craftsmanId);
        Task<PagedResult<ReviewDto>> GetPagedReviewsByCraftsmanIdAsync(int craftsmanId, int pageNumber, int pageSize);
    }
}
