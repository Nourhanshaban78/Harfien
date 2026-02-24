using Harfien.Domain.Entities;

namespace Harfien.Domain.Shared.Repositories
{
    public interface IComplaintRepository : IGenericRepository<Complaint>
    {
        Task<IEnumerable<Complaint>> GetByReporterIdAsync(int reporterId);
        Task<IEnumerable<Complaint>> GetByOrderIdAsync(int OrderId);
        Task<Complaint?> GetWithOrderAsync(int complaintId);
        Task<IEnumerable<Complaint>> GetAllWithOrdersAsync();
    }
}