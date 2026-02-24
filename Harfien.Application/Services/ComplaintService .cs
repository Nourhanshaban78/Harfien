using Harfien.Application.DTO.Complaint;
using Harfien.Application.DTO.Order;
using Harfien.Application.Interfaces;
using Harfien.Domain.Entities;
using Harfien.Domain.Enums;
using Harfien.Domain.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly IUnitOfWork _unit;

        public ComplaintService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task CreateComplaintAsync(int reporterId, CreateComplaintDto dto)
        {
            var complaint = new Complaint
            {
                ReporterId = reporterId,
                OrderId = dto.OrderId,
                Description = dto.Description,
                EvidenceAttachmentUrl = dto.EvidenceAttachmentUrl,
                Status = ComplaintStatus.Pending
            };

            await _unit.ComplaintRepository.AddAsync(complaint);
            await _unit.SaveAsync();
        }

        public async Task UpdateComplaintAsync(int reporterId, int complaintId, UpdateComplaintDto dto)
        {
            var complaint = await _unit.ComplaintRepository.GetByIdAsync(complaintId);

            if (complaint == null || complaint.ReporterId != reporterId || complaint.Status != ComplaintStatus.Pending)
                throw new Exception("Not allowed");

            complaint.Description = dto.Description;
            complaint.EvidenceAttachmentUrl = dto.EvidenceAttachmentUrl;

            _unit.ComplaintRepository.Update(complaint);
            await _unit.SaveAsync();
        }

        public async Task DeleteComplaintAsync(int reporterId, int complaintId)
        {
            var complaint = await _unit.ComplaintRepository.GetByIdAsync(complaintId);

            if (complaint == null || complaint.ReporterId != reporterId || complaint.Status != ComplaintStatus.Pending)
                throw new Exception("Not allowed");

            _unit.ComplaintRepository.Delete(complaint);
            await _unit.SaveAsync();
        }

        //public async Task<IEnumerable<Complaint>> GetMyComplaintsAsync(int reporterId)
        //    => await _unit.ComplaintRepository.GetByReporterIdAsync(reporterId);

        public async Task<IEnumerable<ComplaintResponseDto>> GetMyComplaintsAsync(int reporterId)
        {
            var complaints = await _unit.ComplaintRepository.GetByReporterIdAsync(reporterId);

            return complaints.Select(c => new ComplaintResponseDto  {
                Id = c.Id,
                Description = c.Description,
                EvidenceAttachmentUrl = c.EvidenceAttachmentUrl,
                Status = c.Status,
                CreatedAt = c.CreatedAt,
                AdminResolutionNotes = c.AdminResolutionNotes,
                OrderId = c.OrderId,
                ScheduledAt = c.Order.ScheduledAt,
                OrderStatus = c.Order.Status
            });
        }

        public async Task<ComplaintResponseDto?> GetComplaintByIdAsync(int reporterId, int complaintId)
        {
            var c = await _unit.ComplaintRepository.GetWithOrderAsync(complaintId);

            if (c?.ReporterId != reporterId)
                return null;

            return new ComplaintResponseDto
            {
                Id = c.Id,
                Description = c.Description,
                EvidenceAttachmentUrl = c.EvidenceAttachmentUrl,
                Status = c.Status,
                CreatedAt = c.CreatedAt,
                AdminResolutionNotes= c.AdminResolutionNotes,
                OrderId = c.OrderId,
                ScheduledAt = c.Order.ScheduledAt,
                OrderStatus = c.Order.Status
            };
        }

        // Admin
        public async Task<IEnumerable<ComplaintDetailsDto>> GetAllComplaintsAsync()
        {
            var complaints = await _unit.ComplaintRepository.GetAllWithOrdersAsync();

            return complaints.Select(c => new ComplaintDetailsDto
            {
                Id = c.Id,
                Description = c.Description,
                EvidenceAttachmentUrl = c.EvidenceAttachmentUrl,
                Status = c.Status,
                AdminResolutionNotes = c.AdminResolutionNotes,
                ResolvedAt = c.ResolvedAt,
               
                Order = new OrderInfoDto
                {
                    OrderId = c.Order.Id,
                    ClientName = c.Order.Client.User.FullName,
                    CraftsmanName = c.Order.Craftsman.User.FullName,
                    ServiceName = c.Order.Service.Name,
                    ScheduledAt = c.Order.ScheduledAt,
                    OrderStatus = c.Order.Status,
                    Amount = c.Order.Amount
                }
            });
        }

        public async Task<ComplaintDetailsDto?> GetComplaintDetailsAsync(int complaintId)
        {
            var c = await _unit.ComplaintRepository.GetWithOrderAsync(complaintId);

            if (c == null) return null;

            return new ComplaintDetailsDto
            {
                Id = c.Id,
                Description = c.Description,
                EvidenceAttachmentUrl = c.EvidenceAttachmentUrl,
                Status = c.Status,
                AdminResolutionNotes = c.AdminResolutionNotes,
                ResolvedAt = c.ResolvedAt,

                Order = new OrderInfoDto
                {
                    OrderId = c.Order.Id,
                    ClientName = c.Order.Client.User.FullName,
                    CraftsmanName = c.Order.Craftsman.User.FullName,
                    ServiceName = c.Order.Service.Name,
                    ScheduledAt = c.Order.ScheduledAt,
                    OrderStatus = c.Order.Status,
                    Amount = c.Order.Amount
                }
            };
        }


      
        public async Task<bool> ChangeStatusAsync(int complaintId, ComplaintStatus status)
        {
            var complaint = await _unit.ComplaintRepository.GetByIdAsync(complaintId);
            if (complaint == null)
                return false; 

            complaint.Status = status;
            await _unit.SaveAsync();
            return true;
        }

        public async Task<bool> AddResolutionAsync(int complaintId, string notes)
        {
            var complaint = await _unit.ComplaintRepository.GetByIdAsync(complaintId);
            if (complaint == null)
                return false;

            complaint.AdminResolutionNotes = notes;
            complaint.ResolvedAt = DateTime.UtcNow;
            complaint.Status = ComplaintStatus.Resolved;
            await _unit.SaveAsync();
            return true;
        }




    }
}
