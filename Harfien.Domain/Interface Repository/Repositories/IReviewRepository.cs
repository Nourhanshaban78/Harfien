using Harfien.Domain.Entities;

namespace Harfien.Domain.Shared.Repositories
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetAllByCraftsmanIdAsync(int craftsmanId);
        Task<bool> HasReviewForOrderAsync(int orderId);
        Task<(IEnumerable<Review> Reviews, int TotalCount)> GetPagedByCraftsmanIdAsync(int craftsmanId, int pageNumber, int pageSize);
    }
}