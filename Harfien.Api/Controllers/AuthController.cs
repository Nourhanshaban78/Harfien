using Harfien.Application.DTO.Authentication;
using Harfien.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Harfien.Presentation.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ==========================
        // Register Client
        // ==========================
        [HttpPost("register-client")]
        public async Task<IActionResult> RegisterClient(RegisterClientDto dto)
        {
            var resultMessage = await _authService.RegisterClientAsync(dto);

           
            if (resultMessage.Contains("failed") || resultMessage.Contains("already registered"))
                return BadRequest(new { Message = resultMessage });

   
            return Ok(new { Message = resultMessage });
        }
        // ==========================
        // Register Craftsman
        // ==========================
        [HttpPost("register-craftsman")]
        public async Task<IActionResult> RegisterCraftsman(RegisterCraftsmanDto dto)
        {
            var resultMessage = await _authService.RegisterCraftsmanAsync(dto);

            if (resultMessage.Contains("failed") || resultMessage.Contains("already registered"))
                return BadRequest(new { Message = resultMessage });

            return Ok(new { Message = resultMessage });
        }
        // ==========================
        // Login
        // ==========================
        [HttpPost("login")]
        public async Task<IActionResult> Login(loginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        // ==========================
        // Logout
        // ==========================
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized(new { message = "User not authenticated" });

            await _authService.LogoutAsync(userId);

            return Ok(new { message = "Logged out successfully" });
        }

        // ==========================
        // Approve Craftsman (Admin only)
        // ==========================
        [Authorize(Roles = "Admin")]
        [HttpPost("approve-craftsman/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var message = await _authService.ApproveCraftsmanAsync(id);

                if (message == null)
                    return NotFound(new { error = "Craftsman not found" });

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        // ==========================
        // Reject Craftsman (Admin only)
        // ==========================
        [Authorize(Roles = "Admin")]
        [HttpPost("reject-craftsman/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                var message = await _authService.RejectCraftsmanAsync(id);

                if (message == null)
                    return NotFound(new { error = "Craftsman not found" });

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        // ==========================
        // Confirm Password (Admin only)
        // ==========================


        [HttpPost("confirm-password")]
        [Authorize] // ·«“„ ÌﬂÊ‰ «·„” Œœ„ „”Ã· œŒÊ·
        public async Task<IActionResult> ConfirmPassword([FromBody] ConfirmPasswordDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var result = await _authService.ConfirmPasswordAsync(userId, request.Password);

            if (!result)
                return BadRequest(new { message = "Password is incorrect" });

            return Ok(new { message = "Password confirmed successfully" });
        }

        // ==========================
        // Verify Reset Code
        // ==========================
        [HttpPost("verify-reset-code")]
        public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var result = await _authService.VerifyResetCodeAsync(userId, request.ResetCode);

            if (!result)
                return BadRequest(new { message = "Invalid or expired reset code" });

            return Ok(new { message = "Reset code verified successfully" });
        }





        // ==========================
        // Forget Password
        // ==========================

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPassword model)
        {
            var result = await _authService.ForgetPasswordAsync(model);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }


        // ==========================
        // verify Code
        // ==========================

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode(VerifyResetCode dto)
        {
            var result = await _authService.VerifyResetCode(dto);

            if (result == null)
                return BadRequest("Invalid or expired code");

            return Ok(result);
        }

        // ==========================
        // ResetPassword
        // ==========================


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword dto)
        {
            var result = await _authService.ResetPassword(dto);

            if (result != "Password reset successfully")
                return BadRequest(result);

            return Ok(result);
        }





    }
}
