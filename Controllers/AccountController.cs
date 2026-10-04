using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SieuThiMini.Models;
using SieuThiMini.ViewModels.Account;

namespace SieuThiMini.Controllers;

/// <summary>
/// Quản lý xác thực tài khoản: Đăng nhập, Đăng xuất, Phân quyền.
/// </summary>
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// Hiển thị màn hình đăng nhập.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        var model = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };

        return View(model);
    }

    /// <summary>
    /// Xử lý yêu cầu đăng nhập.
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            _logger.LogWarning("Đăng nhập thất bại: không tìm thấy người dùng có email {Email}.", model.Email);
            ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không chính xác.");
            return View(model);
        }

        // Đăng nhập với lockoutOnFailure: true để chống brute-force
        var result = await _signInManager.PasswordSignInAsync(
            user.UserName!,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            _logger.LogInformation("Người dùng {Email} ({FullName}) đã đăng nhập thành công.", user.Email, user.FullName);
            TempData["SuccessMessage"] = $"Đăng nhập thành công! Chào mừng {user.FullName}.";

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning("Tài khoản {Email} đã bị tạm khóa do nhập sai nhiều lần.", model.Email);
            ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị tạm khóa vì nhập sai mật khẩu quá 5 lần. Vui lòng thử lại sau 5 phút.");
            return View(model);
        }

        _logger.LogWarning("Mật khẩu không chính xác cho tài khoản {Email}.", model.Email);
        ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không chính xác.");
        return View(model);
    }

    /// <summary>
    /// Xử lý đăng xuất an toàn bằng POST.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var userEmail = User.Identity?.Name;
        await _signInManager.SignOutAsync();
        _logger.LogInformation("Người dùng {Email} đã đăng xuất.", userEmail);

        TempData["SuccessMessage"] = "Bạn đã đăng xuất khỏi hệ thống thành công.";
        return RedirectToAction(nameof(Login));
    }

    /// <summary>
    /// Màn hình thông báo khi không có quyền truy cập chức năng.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
