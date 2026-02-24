using Harfien.DataAccess;
using Harfien.Domain.Entities;
using Harfien.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Harfien.Infrastructure.Repositories
{
    public class ComplaintRepository : GenericRepository<Complaint>, IComplaintRepository
    {
        public ComplaintRepository(HarfienDbContext context) : base(context) { }

        public async Task<IEnumerable<Complaint>> GetAllWithOrdersAsync()
        {
            //return await _context.Complaints.Include(c=>c.Order).ToListAsync();
            return await _context.Complaints
                    .Include(c => c.Order)
                        .ThenInclude(o => o.Client)
                            .ThenInclude(cl => cl.User)
                    .Include(c => c.Order)
                        .ThenInclude(o => o.Craftsman)
                            .ThenInclude(cr => cr.User)
                    .Include(c => c.Order)
                        .ThenInclude(o => o.Service)
                    .ToListAsync();

        }

        public async Task<IEnumerable<Complaint>> GetByOrderIdAsync(int orderId)
        {
            return await _context.Complaints.Where(c=>c.OrderId == orderId).ToListAsync();  
        }

        public async Task<IEnumerable<Complaint>> GetByReporterIdAsync(int reporterId)
        {
            return await _context.Complaints.Where(c=>c.ReporterId==reporterId).Include(c=>c.Order).ToListAsync();  
        }

        public async Task<Complaint?> GetWithOrderAsync(int complaintId)
        {
            return await _context.Complaints
                   .Include(c => c.Order)
                        .ThenInclude(o => o.Client)
                            .ThenInclude(cl => cl.User)
                    .Include(c => c.Order)
                        .ThenInclude(o => o.Craftsman)
                            .ThenInclude(cr => cr.User)
                    .Include(c => c.Order)
                        .ThenInclude(o => o.Service)
                 .FirstOrDefaultAsync(c=>c.Id == complaintId);
        }
    }
}