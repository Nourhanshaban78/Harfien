using Harfien.Application.Autherization;
using Harfien.Application.DTO.Authentication;
using Harfien.Application.Interfaces;
using Harfien.Domain.Entities;
using Harfien.Domain.Interface_Repository.Repositories;
using Harfien.Domain.Shared.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;


using System;
using System.Transactions;


public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICraftsmanRepository _craftsmanRepo;
    private readonly IJwtTokenService _jwtService;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailSender;
    private readonly IMemoryCache _cache;
    private readonly IUnitOfWork _unitOfWork;




    public AuthService(
        UserManager<ApplicationUser> userManager,
        ICraftsmanRepository craftsmanRepo,
        IJwtTokenService jwtService,
        IConfiguration configuration,
        IEmailService emailSender,
        IMemoryCache cache,
        IUnitOfWork context
        )
    {
        _userManager = userManager;
        _craftsmanRepo = craftsmanRepo;
        _jwtService = jwtService;
        _configuration = configuration;
        _emailSender = emailSender;
        _cache = cache;
        _unitOfWork = context;
      
    }

    public async Task<string> RegisterClientAsync(RegisterClientDto dto)
    {
        if (await _userManager.FindByEmailAsync(dto.Email) != null)
            return "This email is already registered. Please use another email.";

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {

            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return $"Registration failed: {errors}";
        }

        await _userManager.AddToRoleAsync(user, "Client");
        var client = new Client
        {
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };
        await _unitOfWork.Clients.AddAsync(client);
        await _unitOfWork.SaveAsync();
        return "Registration successful. You can now login.";
    }

    public async Task<string> RegisterCraftsmanAsync(RegisterCraftsmanDto dto)
    {
        
        if (await _userManager.FindByEmailAsync(dto.Email) != null)
            return "This email is already registered.";

        // إنشاء المستخدم أولاً
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            FullName = dto.FullName,
            AreaId = dto.AreaId,



        };

        // تسجيل المستخدم في Identity
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return $"Registration failed: {errors}";
        }

        // إضافة الدور
        await _userManager.AddToRoleAsync(user, "Craftsman");

        // إنشاء سجل الحرفي
        var craftsman = new Craftsman
        {
            UserId = user.Id,
          
            YearsOfExperience = dto.YearsOfExperience,
            IsApproved = false
        };
        await _unitOfWork.Craftsmen.AddAsync(craftsman);
        await _unitOfWork.SaveAsync();

        return "Your registration is successful. Your account is pending approval by the admin.";
    }

    public async Task<LoginResponse> LoginAsync(loginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

        var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!passwordValid)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

       
        var craftsman = await _craftsmanRepo.GetByUserIdAsync(user.Id);

        if (craftsman != null && !craftsman.IsApproved)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Your account is not approved yet"
            };
        }

        // Generate JWT
        var token = await _jwtService.GenerateTokenAsync(user);

        return new LoginResponse
        {
            Success = true,
            Token = token
        };
    }


    public async Task<string> ApproveCraftsmanAsync(int craftsmanId)
    {
        // جلب الحرفي من قاعدة البيانات
        var craftsman = await _craftsmanRepo.GetByIdAsync(craftsmanId);
        if (craftsman == null)
            return null; // لو الحرفي مش موجود

        // تغيير حالة الحرفي من false لـ true
        craftsman.IsApproved = true;

        // تحديث البيانات في قاعدة البيانات
        await _craftsmanRepo.UpdateAsync(craftsman);

        
        string userId = craftsman.UserId;

        return "Craftsman approved successfully. User can now log in.";
    }


    public async Task<string> RejectCraftsmanAsync(int craftsmanId)
    {
       
        var craftsman = await _craftsmanRepo.GetByIdAsync(craftsmanId);
        if (craftsman == null)
            return null; 
        craftsman.IsApproved = false;

        
        await _craftsmanRepo.UpdateAsync(craftsman);


        string userId = craftsman.UserId;

        return "Craftsman Reject successfully.";
    }


    public async Task<(bool Success, string Message)> ForgetPasswordAsync(ForgetPassword forgetPassword)
    {
        var user = await _userManager.FindByEmailAsync(forgetPassword.Email);

        if (user == null)
            return (false, "This email is not registered.");

        var code = Random.Shared.Next(10000, 100000).ToString();

        user.ResetCode = BCrypt.Net.BCrypt.HashPassword(code);
        user.ResetCodeExpiry = DateTime.UtcNow.AddMinutes(10);

        await _userManager.UpdateAsync(user);

        await _emailSender.SendAsync(
            user.Email,
            "Password Reset Code",
            $"Your password reset code is: {code}\nThis code expires in 10 minutes."
        );

        return (true, "Your reset code has been sent to your email.");
    }



    public async Task<string?> VerifyResetCode(VerifyResetCode dto)
    {
        var user = _userManager.Users
            .AsEnumerable()
            .FirstOrDefault(u =>
                u.ResetCode != null &&
                BCrypt.Net.BCrypt.Verify(dto.Code, u.ResetCode));

        if (user == null)
            return null;

        if (user.ResetCodeExpiry < DateTime.UtcNow)
            return null;

        // store userId temporarily (10 minutes)
        _cache.Set("PasswordResetUser", user.Id, TimeSpan.FromMinutes(10));

        return "Code verified";
    }

    public async Task<string?> ResetPassword(ResetPassword dto)
    {
        // 1️⃣ Get userId from cache
        if (!_cache.TryGetValue("PasswordResetUser", out string userId))
            return "Verification expired";

        // 2️⃣ Find user
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return "User not found";

        // 3️⃣ Generate reset token
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        // 4️⃣ Reset password
        var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);

        if (!result.Succeeded)
            return string.Join(", ", result.Errors.Select(e => e.Description));

        // 5️⃣ Cleanup
        _cache.Remove("PasswordResetUser");

        user.ResetCode = null;
        user.ResetCodeExpiry = null;
        await _userManager.UpdateAsync(user);

        return "Password reset successfully";
    }


    public async Task<bool> ConfirmPasswordAsync(string userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<bool> VerifyResetCodeAsync(string userId, string resetCode)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        return await _userManager.VerifyUserTokenAsync(
            user,
            _userManager.Options.Tokens.PasswordResetTokenProvider,
            "ResetPassword",
            resetCode
        );
    }

    public async Task LogoutAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            // Optional: Invalidate refresh tokens or perform cleanup
            // For now, invalidate the security stamp to invalidate all tokens
            await _userManager.UpdateSecurityStampAsync(user);
        }
    }
}
