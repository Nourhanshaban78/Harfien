using Harfien.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Harfien.Presentation
{
    public static class AdminSeedData
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string adminRole = "ADMIN";
        string adminEmail = "Mohamedhassan123@gmail.com";
        string adminPassword = "Moh123#!";

        // 1️⃣ إنشاء الرول لو مش موجودة
        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(adminRole));
            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                    System.Console.WriteLine($"Role creation error: {error.Description}");
                return; // لو فشل إنشاء الرول، نوقف
            }
        }

        // 2️⃣ إنشاء Admin User لو مش موجود
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Super Admin"
            };

            var userResult = await userManager.CreateAsync(adminUser, adminPassword);
            if (!userResult.Succeeded)
            {
                foreach (var error in userResult.Errors)
                    System.Console.WriteLine($"Admin creation error: {error.Description}");
                return; // لو فشل إنشاء المستخدم، نوقف
            }

            // 3️⃣ إضافة الـ Admin للرول
            var addRoleResult = await userManager.AddToRoleAsync(adminUser, adminRole);
            if (!addRoleResult.Succeeded)
            {
                foreach (var error in addRoleResult.Errors)
                    System.Console.WriteLine($"Add to role error: {error.Description}");
            }
            else
            {
                System.Console.WriteLine("Admin created and added to role successfully!");
            }
        }
        else
        {
            System.Console.WriteLine("Admin already exists.");
        }
    }
}

    }
