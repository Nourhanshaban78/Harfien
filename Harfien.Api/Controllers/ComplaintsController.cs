//using Harfien.Application.DTO;
//using Harfien.Application.Interfaces;
//using Harfien.Domain.Enums;
//using Harfien.Domain.Shared.Repositories;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace Harfien.Presentation.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ComplaintsController : ControllerBase
//    {
//        private readonly IComplaintService _service;
//        private readonly IUnitOfWork _unit;

//        public ComplaintsController(IComplaintService service,IUnitOfWork unit)
//        {
//            _service = service;
//            _unit = unit;
//        }

//        [Authorize(Roles = "Client,Carftsman")]
//        [HttpPost]
//        public async Task<IActionResult> Create(CreateComplaintDto dto)
//        {
//            await _service.CreateComplaintAsync(await GetReporterId(), dto);
//            return Ok("Complaint Created");
//        }

//        [Authorize(Roles = "Client,Carftsman")]
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, UpdateComplaintDto dto)
//        {
//            await _service.UpdateComplaintAsync(await GetReporterId(), id, dto);
//            return Ok("Updated");
//        }

//        [Authorize(Roles = "Client,Carftsman")]
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            await _service.DeleteComplaintAsync(await GetReporterId(), id);
//            return Ok("Deleted");
//        }

//        [Authorize(Roles = "Client,Carftsman")]
//        [HttpGet("my")]
//        public async Task<IActionResult> GetMy()
//        {
//            var data = await _service.GetMyComplaintsAsync(await GetReporterId());
//            return Ok(data);
//        }

//        [Authorize(Roles = "Client,Carftsman")]
//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var data = await _service.GetComplaintByIdAsync(await GetReporterId(), id);
//            if (data == null) return Forbid();

//            return Ok(data);
//        }


//        /// <summary>
//        /// Admin  Endpoints
//        /// </summary>
//        /// 
//        [Authorize(Roles = "Admin")]
//        [HttpGet("admin/complaints")]

//        public async Task<IActionResult> GetAll()
//        {
//            var data = await _service.GetAllComplaintsAsync();
//            return Ok(data);
//        }


//        [Authorize(Roles = "Admin")]
//        [HttpGet("admin/complaints/{id}")]
//        public async Task<IActionResult> Details(int id)
//        {
//            var data = await _service.GetComplaintDetailsAsync(id);
//            return Ok(data);
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpPatch("admin/complaints/{id}/status")]
//        public async Task<IActionResult> ChangeStatus(int id, [FromQuery] ComplaintStatus status)
//        {
//            await _service.ChangeStatusAsync(id, status);
//            return Ok("Status Updated");
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpPatch("admin/complaints/{id}/resolution")]
//        public async Task<IActionResult> Resolution(int id, [FromBody] string notes)
//        {
//            await _service.AddResolutionAsync(id, notes);
//            return Ok("Resolved");
//        }

//        private async Task<int> GetReporterId()
//        {
//            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


//            var craftsman = await _unit.Craftsmen.GetByUserIdAsync(userId);
//            if (craftsman != null)
//                return craftsman.Id;

//            var client = await _unit.Clients.GetByUserIdAsync(userId);
//            if (client != null)
//                return client.Id;

//            throw new Exception("User is not linked to Client or Craftsman");
//        }

//        //private int GetUserId()
//        //{
//        //    return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
//        //}

//    }
//}

using Harfien.Application.DTO.Complaint;
using Harfien.Application.Interfaces;
using Harfien.Domain.Enums;
using Harfien.Domain.Shared.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Harfien.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly IComplaintService _service;
        private readonly IUnitOfWork _unit;

        public ComplaintsController(IComplaintService service, IUnitOfWork unit)
        {
            _service = service;
            _unit = unit;
        }

        #region Client / Craftsman

        [Authorize(Roles = "Client,Carftsman")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateComplaintDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });

            int reporterId = await GetReporterIdSafe();
            await _service.CreateComplaintAsync(reporterId, dto);

            return Ok(new { success = true, message = "Complaint created successfully" });
        }

        [Authorize(Roles = "Client,Carftsman")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateComplaintDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });

            int reporterId = await GetReporterIdSafe();

            var complaint = await _service.GetComplaintByIdAsync(reporterId, id);
            if (complaint == null)
                return NotFound(new { success = false, message = "Complaint not found or access denied" });

            await _service.UpdateComplaintAsync(reporterId, id, dto);
            return Ok(new { success = true, message = "Complaint updated successfully" });
        }


        [Authorize(Roles = "Client,Carftsman")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int reporterId = await GetReporterIdSafe();
            var complaint = await _service.GetComplaintByIdAsync(reporterId, id);
            if (complaint == null)
                return NotFound(new { success = false, message = "Complaint not found or access denied" });

            await _service.DeleteComplaintAsync(reporterId, id);
            return Ok(new { success = true, message = "Complaint deleted successfully" });
        }

        [Authorize(Roles = "Client,Carftsman")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            int reporterId = await GetReporterIdSafe();
            var data = await _service.GetMyComplaintsAsync(reporterId);
            return Ok(data ?? new List<ComplaintResponseDto>());
        }

        [Authorize(Roles = "Client,Carftsman")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            int reporterId = await GetReporterIdSafe();
            var data = await _service.GetComplaintByIdAsync(reporterId, id);
            if (data == null)
                return NotFound(new { success = false, message = "Complaint not found or access denied" });

            return Ok(data);
        }

        #endregion

        #region Admin

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/complaints")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllComplaintsAsync();
            return Ok(data ?? new List<ComplaintDetailsDto>());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/complaints/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var data = await _service.GetComplaintDetailsAsync(id);
            if (data == null)
                return NotFound(new { success = false, message = "Complaint not found" });

            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("admin/complaints/{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromQuery] ComplaintStatus status)
        {
            var updated = await _service.ChangeStatusAsync(id, status);
            if (!updated)
                return NotFound(new { success = false, message = "Complaint not found" });

            return Ok(new { success = true, message = "Status updated successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("admin/complaints/{id}/resolution")]
        public async Task<IActionResult> Resolution(int id, [FromBody] string notes)
        {
            if (string.IsNullOrWhiteSpace(notes))
                return BadRequest(new { success = false, message = "Resolution notes cannot be empty" });

            var updated = await _service.AddResolutionAsync(id, notes);
            if (!updated)
                return NotFound(new { success = false, message = "Complaint not found" });

            return Ok(new { success = true, message = "Complaint resolved successfully" });
        }

        #endregion

        #region Helpers

        private async Task<int> GetReporterIdSafe()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User not authenticated");

            var craftsman = await _unit.Craftsmen.GetByUserIdAsync(userId);
            if (craftsman != null)
                return craftsman.Id;

            var client = await _unit.Clients.GetByUserIdAsync(userId);
            if (client != null)
                return client.Id;

            throw new UnauthorizedAccessException("User is not linked to Client or Craftsman");
        }

        #endregion
    }
}
