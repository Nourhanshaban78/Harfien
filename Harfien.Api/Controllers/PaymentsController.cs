using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using Harfien.Application.DTO.Payment;
using Harfien.Application.Interfaces.payment_interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harfien.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

       
        [HttpPost("pay-card")]
        public async Task<IActionResult> PayWithCard([FromBody] CreatePaymentDto dto)
        {

            string clientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(clientId))
                return Unauthorized("User not authenticated");

            var result = await _paymentService.PayOrderWithCardAsync(dto, clientId);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
    }
}
