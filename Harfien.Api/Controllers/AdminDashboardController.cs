using Harfien.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harfien.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _service;

        public AdminDashboardController(IAdminDashboardService service)
        {
            _service = service;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
            => Ok(await _service.GetDashboardSummaryAsync());

        [HttpGet("orders-per-day")]
        public async Task<IActionResult> GetOrdersPerDay()
            => Ok(await _service.GetOrdersPerDayAsync());

        [HttpGet("users-by-governorate")]
        public async Task<IActionResult> GetUsersByGovernorate()
            => Ok(await _service.GetUsersByGovernorateAsync());

        [HttpGet("craftmen")]
        public async Task<IActionResult> GetCraftmen(string? name)
            => Ok(await _service.GetAllCraftmenAsync(name));
    }
}
