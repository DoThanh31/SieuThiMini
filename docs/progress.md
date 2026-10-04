# Tiến độ

Cập nhật file này sau mỗi module. Đánh dấu `[x]` khi đã build sạch, test pass và tôi đã thử trên trình duyệt.

- [x] Module 1: Nền tảng (project, EF Core, Identity, layout)
- [ ] Module 2: Danh mục và Sản phẩm
- [ ] Module 3: Nhà cung cấp và Nhập hàng
- [ ] Module 4: Bán hàng (POS)
- [ ] Module 5: Báo cáo

## Ghi chú / vấn đề tồn đọng
- **Module 1 (Hoàn thành)**:
  - Cài đặt EF Core 10.0.12, SQL Server LocalDB `(localdb)\MSSQLLocalDB`, ASP.NET Core Identity.
  - Chuỗi kết nối `DefaultConnection` lưu trực tiếp trong [appsettings.json](file:///c:/Users/LOQ/.gemini/antigravity-ide/scratch/SieuThiMini/appsettings.json).
  - Tài khoản mẫu Admin và Cashier được cấu hình qua User Secrets (`SeedAdmin`, `SeedCashier`) và chỉ seed khi môi trường là `Development`.
  - Fallback authorization policy: Mặc định toàn ứng dụng yêu cầu đăng nhập, ngoại trừ các action công khai có `[AllowAnonymous]` (`Login`, `AccessDenied`).
  - Đăng nhập có bảo vệ lockout (`lockoutOnFailure: true`, khóa 5 phút sau 5 lần nhập sai).
  - Giao diện Bootstrap 5 chuẩn mực: Navbar phân quyền Admin/Cashier, thông báo `TempData` alert/toast, giao diện trang Đăng nhập & Báo từ chối quyền.
