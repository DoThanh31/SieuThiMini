using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SieuThiMini.Models;

/// <summary>
/// Đại diện cho tài khoản người dùng trong hệ thống (kế thừa từ IdentityUser).
/// </summary>
public class ApplicationUser : IdentityUser
{
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;
}
