using Harfien.Application.DTO.Complaint;
using Harfien.Domain.Entities;
using Harfien.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Interfaces
{
    public interface IComplaintService
    {
        Task CreateComplaintAsync(int reporterId, CreateComplaintDto dto);
        Task UpdateComplaintAsync(int reporterId, int complaintId, UpdateComplaintDto dto);
        Task DeleteComplaintAsync(int reporterId, int complaintId);

        Task<IEnumerable<ComplaintResponseDto>> GetMyComplaintsAsync(int reporterId);
        Task<ComplaintResponseDto?> GetComplaintByIdAsync(int reporterId, int complaintId);

        Task<IEnumerable<ComplaintDetailsDto>> GetAllComplaintsAsync(); // Admin
        Task<ComplaintDetailsDto?> GetComplaintDetailsAsync(int complaintId); // Admin
        Task<bool> ChangeStatusAsync(int complaintId, ComplaintStatus status); // Admin
        Task<bool> AddResolutionAsync(int complaintId, string notes); // Admin
    }

}
