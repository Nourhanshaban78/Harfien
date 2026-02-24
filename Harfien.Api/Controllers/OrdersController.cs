using Harfien.Application.DTO.Order;
using Harfien.Application.Interfaces;
using Harfien.DataAccess;
using Harfien.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Harfien.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 🔥 مهم جداً
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly HarfienDbContext _context;

        public OrdersController(IOrderService service, HarfienDbContext context)
        {
            _service = service;
            _context = context;
        }

        // ===========================
        // Helper Method
        // ===========================
        private async Task<int> GetClientIdAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User not authenticated");

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (client == null)
                throw new Exception("Client not found");

            return client.Id;
        }

        private async Task<int> GetCraftsmanIdAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User not authenticated");

            var craftsman = await _context.Craftsmen
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (craftsman == null)
                throw new Exception("Craftsman not found");

            return craftsman.Id;
        }

        // ===========================
        // Create Order
        // ===========================
        [HttpPost]
        //[Authorize(Roles = "Client")]
        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            try
            {
                var clientId = await GetClientIdAsync();
                var result = await _service.CreateAsync(dto, clientId);

                if (result != "Order created successfully")
                    return BadRequest(result);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // ===========================
        // Get By Id
        // ===========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            return Ok(order);
        }

        // ===========================
        // Get Client Orders
        // ===========================
        [HttpGet("client")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetClientOrders()
        {
            var clientId = await GetClientIdAsync();

            var orders = await _service.GetClientOrdersAsync(clientId);
            return Ok(orders);
        }

        // ===========================
        // Get Craftsman Orders
        // ===========================
        [HttpGet("craftsman")]
        [Authorize(Roles = "Craftsman")]
        public async Task<IActionResult> GetCraftsmanOrders()
        {
            var craftsmanId = await GetCraftsmanIdAsync();

            var orders = await _service.GetCraftsmanOrdersAsync(craftsmanId);
            return Ok(orders);
        }

        // ===========================
        // Accept Order
        // ===========================
        [HttpPut("{id}/accept")]
        [Authorize(Roles = "Craftsman")]
        public async Task<IActionResult> Accept(int id)
        {
            var craftsmanId = await GetCraftsmanIdAsync();

            await _service.AcceptAsync(id, craftsmanId);
            return NoContent();
        }

        // ===========================
        // Reject Order
        // ===========================
        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Craftsman")]
        public async Task<IActionResult> Reject(int id)
        {
            var craftsmanId = await GetCraftsmanIdAsync();

            await _service.RejectAsync(id, craftsmanId);
            return NoContent();
        }

        // ===========================
        // Start Order
        // ===========================
        [HttpPut("{id}/start")]
        [Authorize(Roles = "Craftsman")]
        public async Task<IActionResult> Start(int id)
        {
            var craftsmanId = await GetCraftsmanIdAsync();

            await _service.StartAsync(id, craftsmanId);
            return NoContent();
        }

        // ===========================
        // Complete Order
        // ===========================
        [HttpPut("{id}/complete")]
        [Authorize(Roles = "Craftsman")]
        public async Task<IActionResult> Complete(int id)
        {
            var craftsmanId = await GetCraftsmanIdAsync();

            await _service.CompleteAsync(id, craftsmanId);
            return NoContent();
        }

        // ===========================
        // Cancel Order
        // ===========================
        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Cancel(int id)
        {
            var clientId = await GetClientIdAsync();

            await _service.CancelAsync(id, clientId);
            return NoContent();
        }
    }
}