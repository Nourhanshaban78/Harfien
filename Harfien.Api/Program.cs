using Harfien.Application;
using Harfien.Application.Autherization;
using Harfien.Application.Interfaces;
using Harfien.Application.Interfaces.payment_interfaces;
using Harfien.Application.Services;
using Harfien.DataAccess;
using Harfien.Domain.Entities;
using Harfien.Domain.Interface_Repository;
using Harfien.Domain.Interface_Repository.Repositories;
using Harfien.Domain.Shared.Repositories;
using Harfien.Infrastructure.Repositories;
using Harfien.Infrastructure.Services;
using Harfien.Presentation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;

namespace Harfien.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // =========================
            // DbContext
            // =========================
            builder.Services.AddDbContext<HarfienDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            // =========================
            // Identity
            // =========================
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
            })
            .AddEntityFrameworkStores<HarfienDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddScoped<IClientRepository, ClientRepository>();
            builder.Services.AddScoped<ICraftsmanRepository, CraftsmanRepository>();
         



            builder.Services.AddScoped<IWalletRepository, WalletRepository>();
            builder.Services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();
            builder.Services.AddScoped<ISubscriptionPlanDetailsRepository, SubscriptionPlanDetailsRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
            builder.Services.AddScoped<ICraftsmanService, CraftsmanService>();

            // chat 
            builder.Services.AddScoped<IMessageRepositry, MessageRepositry>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IChatNotifier, ChatNotifier>();
            builder.Services.AddSignalR();




          
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IWalletRepository, WalletRepository>();
            builder.Services.AddScoped<IComplaintService, ComplaintService>();   


            builder.Services.AddScoped<IWalletTransactionRepository,WalletTransactionRepository>();
            builder.Services.AddScoped<IWalletTransactionService, WalletTransactionService>();
            // =========================
            // JWT Authentication (مرة واحدة)
            // =========================
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false, // زي ما كان عندك
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                    )
                };

                // ✅ Add event handler to validate security stamp
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
                        var userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

                        if (userId == null)
                        {
                            context.Fail("Invalid token");
                            return;
                        }

                        var user = await userManager.FindByIdAsync(userId);
                        if (user == null)
                        {
                            context.Fail("User not found");
                            return;
                        }

                        // Get the security stamp claim from token
                        var tokenSecurityStamp = context.Principal?.FindFirstValue("SecurityStamp");
                        var userSecurityStamp = await userManager.GetSecurityStampAsync(user);

                        // If security stamps don't match, token has been invalidated (user logged out)
                        if (tokenSecurityStamp != userSecurityStamp)
                        {
                            context.Fail("Token has been invalidated");
                            return;
                        }
                    }
                };
            });

            // =========================
            // Authorization
            // =========================
            builder.Services.AddAuthorization();
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, DynamicPolicyProvider>();
            builder.Services.AddSingleton<IAuthorizationHandler, DynamicRoleHanlder>();

            // =========================
            // Repositories
            // =========================
            builder.Services.AddScoped<IWalletRepository, WalletRepository>();
            builder.Services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();
            builder.Services.AddScoped<ISubscriptionPlanDetailsRepository, SubscriptionPlanDetailsRepository>();
            builder.Services.AddScoped<ICraftsmanRepository, CraftsmanRepository>();
            builder.Services.AddScoped<IClientRepository, ClientRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
            builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();

            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();


            builder.Services.AddScoped<ICityRepository, CityRepository>();
            builder.Services.AddScoped<IAreaRepository, AreaRepository>();

            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IWalletService, WalletService>();
            // =========================
            //services
            // =========================

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IServiceService, ServiceService>();
            
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();

            //Admin Dash
            builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();


            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<ICityService, CityService>();
            builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();
            builder.Services.AddScoped<IAreaService, AreaService>();


            // =========================
            // AutoMapper
            // =========================


            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = true;
            }, typeof(Application.AssemblyReference).Assembly);
            //  builder.Services.AddAutoMapper(typeof(OrderProfile));

            // =========================
            // Forget Password
            // =========================
            builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(10));

            builder.Services.AddScoped<IEmailService, EmailSender>();
            builder.Services.AddMemoryCache();

            // =========================
            // Controllers
            // =========================
            builder.Services.AddControllers();

            // =========================
            // Swagger
            // =========================
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(o =>
            {
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT like: Bearer {token}"
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            ///////
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });
            var app = builder.Build();

            // =========================
            // Initialize Roles and Seed Admin
            // =========================
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                // 1️⃣ إنشاء كل الرولز
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roles = { "CLIENT", "CRAFTSMAN", "ADMIN" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                // 2️⃣ إنشاء الـ Admin
                await AdminSeedData.SeedAdminAsync(services);
            }


            // =========================
            // Middleware
            // =========================
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapHub<ChatHub>("/chatHub");
            app.MapHub<NotificationHub>("/notificationHub");
            await app.RunAsync();
        }
    }
}