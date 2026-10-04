using System.ComponentModel.DataAnnotations;

namespace SieuThiMini.ViewModels.Account;

/// <summary>
/// Dữ liệu nhận từ form đăng nhập.
/// </summary>
public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email.")]
    [EmailAddress(ErrorMessage = "Địa chỉ Email không đúng định dạng.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Ghi nhớ đăng nhập")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
