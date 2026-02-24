using Harfien.Application.Autherization;
using Harfien.Application.DTO.Authentication;

namespace Harfien.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterClientAsync(RegisterClientDto dto);
        Task <string> RegisterCraftsmanAsync(RegisterCraftsmanDto dto);
        Task<LoginResponse> LoginAsync(loginDto dto);
        Task LogoutAsync(string userId);
        Task<string> ApproveCraftsmanAsync(int craftsmanId); 
        Task<string> RejectCraftsmanAsync(int craftsmanId);
        Task<(bool Success, string Message)> ForgetPasswordAsync(ForgetPassword forgetPassword);
        Task<string?> VerifyResetCode(VerifyResetCode dto);
        Task<string?> ResetPassword(ResetPassword dto);
        Task<bool> ConfirmPasswordAsync(string userId, string password);
        Task<bool> VerifyResetCodeAsync(string userId, string resetCode);
    }
}
