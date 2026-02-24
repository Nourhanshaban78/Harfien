using System.Security.Claims;
using Harfien.Application.Interfaces.payment_interfaces;
using Harfien.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harfien.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {

        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("my-wallet")]
        public async Task<IActionResult> GetMyWallet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wallet = await _walletService.GetWalletByUserIdAsync(userId);

            if (wallet == null)
                return NotFound("Wallet not found");

            return Ok(wallet);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWallet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wallet = await _walletService.CreateWalletAsync(userId);

            return Ok(wallet);
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var balance = await _walletService.GetBalanceAsync(userId);

            return Ok(new { Balance = balance });
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteWallet()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var deleted = await _walletService.DeleteWalletAsync(userId);

                if (!deleted)
                    return NotFound("Wallet not found");

                return Ok("Wallet deleted successfully");
            }
            catch (Exception ex) { 
            return BadRequest(ex.Message);
            }

            
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetMyTransactions(  int pageNumber = 1,  int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var transactions = await _walletService  .GetTransactionsAsync(userId, pageNumber, pageSize);

            return Ok(transactions);
        }

        [HttpGet("my-payments")]
        public async Task<IActionResult> GetMyPayments(
  int pageNumber = 1,
  int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

          
            var result = await _walletService
                .GetPaymentsByClientIdAsync(userId, pageNumber, pageSize);

            return Ok(result);
        }

    }
}
