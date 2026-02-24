using Harfien.Application.DTO.Payment;
using Harfien.Application.Interfaces.payment_interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Harfien.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletTransactionsController : ControllerBase
    {
        private readonly IWalletTransactionService _service;

        public WalletTransactionsController(IWalletTransactionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWalletTransactionDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            try
            {
                var result = await _service.CreateAsync(userId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
               
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("my-transactions")]
        public async Task<IActionResult> GetMyTransactions(
            int pageNumber = 1,
            int pageSize = 10)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _service
                .GetMyTransactionsAsync(userId, pageNumber, pageSize);

            return Ok(result);
        }
    }

}
