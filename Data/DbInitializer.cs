using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SieuThiMini.Models;

namespace SieuThiMini.Data;

/// <summary>
/// Khởi tạo dữ liệu ban đầu cho database (Roles, Tài khoản Admin & Thu ngân mẫu).
/// </summary>
public static class DbInitializer
{
    public const string RoleAdmin = "Admin";
    public const string RoleCashier = "Cashier";

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DbInitializer");
        var environment = services.GetRequiredService<IHostEnvironment>();

        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var configuration = services.GetRequiredService<IConfiguration>();

            // Tự động migrate nếu cần
            await context.Database.MigrateAsync();

            // 1. Khởi tạo các Vai trò mặc định (Admin, Cashier)
            string[] roles = [RoleAdmin, RoleCashier];
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                    if (roleResult.Succeeded)
                    {
                        logger.LogInformation("Khởi tạo thành công vai trò: {Role}", role);
                    }
                    else
                    {
                        logger.LogError("Lỗi khi tạo vai trò {Role}: {Errors}", role,
                            string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    }
                }
            }

            // 2. Chỉ seed tài khoản mẫu khi ở môi trường Development
            if (!environment.IsDevelopment())
            {
                logger.LogInformation("Bỏ qua seed tài khoản mẫu vì không phải môi trường Development.");
                return;
            }

            // Seed tài khoản Admin từ cấu hình (User Secrets)
            var adminEmail = configuration["SeedAdmin:Email"];
            var adminPassword = configuration["SeedAdmin:Password"];
            var adminFullName = configuration["SeedAdmin:FullName"] ?? "Quản trị viên";

            if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
            {
                var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
                if (existingAdmin is null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FullName = adminFullName,
                        EmailConfirmed = true
                    };

                    var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                    if (createResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, RoleAdmin);
                        logger.LogInformation("Tạo thành công tài khoản Admin mặc định: {Email}", adminEmail);
                    }
                    else
                    {
                        logger.LogWarning("Không thể tạo tài khoản Admin: {Errors}",
                            string.Join(", ", createResult.Errors.Select(e => e.Description)));
                    }
                }
            }
            else
            {
                logger.LogWarning("Chưa cấu hình 'SeedAdmin:Email' hoặc 'SeedAdmin:Password' trong User Secrets. Bỏ qua tạo tài khoản Admin mẫu.");
            }

            // Seed tài khoản Cashier từ cấu hình (User Secrets)
            var cashierEmail = configuration["SeedCashier:Email"];
            var cashierPassword = configuration["SeedCashier:Password"];
            var cashierFullName = configuration["SeedCashier:FullName"] ?? "Thu ngân";

            if (!string.IsNullOrWhiteSpace(cashierEmail) && !string.IsNullOrWhiteSpace(cashierPassword))
            {
                var existingCashier = await userManager.FindByEmailAsync(cashierEmail);
                if (existingCashier is null)
                {
                    var cashierUser = new ApplicationUser
                    {
                        UserName = cashierEmail,
                        Email = cashierEmail,
                        FullName = cashierFullName,
                        EmailConfirmed = true
                    };

                    var createResult = await userManager.CreateAsync(cashierUser, cashierPassword);
                    if (createResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(cashierUser, RoleCashier);
                        logger.LogInformation("Tạo thành công tài khoản Cashier mặc định: {Email}", cashierEmail);
                    }
                    else
                    {
                        logger.LogWarning("Không thể tạo tài khoản Cashier: {Errors}",
                            string.Join(", ", createResult.Errors.Select(e => e.Description)));
                    }
                }
            }
            else
            {
                logger.LogWarning("Chưa cấu hình 'SeedCashier:Email' hoặc 'SeedCashier:Password' trong User Secrets. Bỏ qua tạo tài khoản Cashier mẫu.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Đã xảy ra lỗi trong quá trình khởi tạo dữ liệu.");
            throw;
        }
    }
}
