# Quy ước code C# / ASP.NET Core MVC

## Cấu trúc thư mục
```
SieuThiMini/
  Controllers/      Chỉ nhận request, gọi Service, trả View. Không viết logic nghiệp vụ.
  Services/         Logic nghiệp vụ (interface IXxxService + XxxService).
  Models/           Entity của EF Core.
  ViewModels/       Dữ liệu truyền sang View và nhận từ form.
  Data/             AppDbContext, cấu hình entity, seed dữ liệu, Migrations.
  Views/            Razor views, theo thư mục từng controller.
  wwwroot/          CSS, JS, ảnh.
SieuThiMini.Tests/  Dự án xUnit.
```

## Quy tắc chung
- Dùng C# hiện đại: `record` cho DTO nếu phù hợp, `async/await` cho mọi truy cập DB, `nullable reference types` bật.
- Dùng **dependency injection**, đăng ký service trong `Program.cs`. Không dùng `new` cho service.
- Tên: PascalCase cho class/method/property, camelCase cho biến cục bộ, `_camelCase` cho field private, interface bắt đầu bằng `I`.
- Method async có hậu tố `Async` và nhận `CancellationToken` ở các service quan trọng.
- Mỗi class một file. Method ngắn, một việc.
- Không viết comment thừa cho code hiển nhiên. Chỉ comment khi giải thích "vì sao".

## Dữ liệu (EF Core + SQL Server)
- Provider: `Microsoft.EntityFrameworkCore.SqlServer`. Dùng `UseSqlServer(...)` trong `Program.cs`.
- Chuỗi kết nối lấy từ configuration (`GetConnectionString("DefaultConnection")`), **không hard-code** trong code.
- Chuỗi chứa mật khẩu (nếu dùng SQL login) phải để trong User Secrets, không commit lên git.
- Cấu hình rõ kiểu dữ liệu: `HasMaxLength(...)` cho chuỗi (tránh `nvarchar(max)` không cần thiết), `HasPrecision(18, 2)` cho tiền.
- Tạo **unique index** cho `Product.Barcode` (lọc `IsDeleted = 0` nếu cần), index cho các cột hay tìm kiếm (Name, CreatedAt của Sale).
- Chống xung đột khi nhiều thu ngân cùng bán: dùng cột `RowVersion` (`[Timestamp] byte[]`) trên `Product`, bắt `DbUpdateConcurrencyException` và báo người dùng thử lại.
- Không viết SQL thô nếu EF Core làm được. Nếu bắt buộc, dùng truy vấn có tham số, không nối chuỗi.
- Dùng **EF Core Code First + Migrations**. Mỗi thay đổi model phải có migration có tên rõ nghĩa.
- Tiền tệ dùng `decimal`, cấu hình `HasPrecision(18, 2)`. **Không dùng float/double cho tiền.**
- Truy vấn chỉ đọc dùng `AsNoTracking()`.
- Tránh N+1: dùng `Include` hoặc projection (`Select`) khi cần dữ liệu liên quan.
- Thao tác nhiều bảng liên quan (bán hàng, nhập hàng) phải nằm trong **một transaction**.
- Xóa mềm (`IsDeleted`) cho Product, Category, Supplier. Không xóa cứng dữ liệu đã phát sinh hóa đơn.

## Controller và View
- Mọi form POST có `[ValidateAntiForgeryToken]`.
- Validate bằng Data Annotations trên ViewModel, kiểm tra `ModelState.IsValid` ở controller.
- Dùng pattern **Post-Redirect-Get** sau khi POST thành công, thông báo qua `TempData`.
- Phân quyền bằng `[Authorize(Roles = "...")]`. Mặc định yêu cầu đăng nhập.
- View dùng Bootstrap 5, layout chung, partial view cho phần lặp lại. Không viết CSS/JS inline dài.

## Bảo mật
- Không lưu mật khẩu dạng thô, dùng ASP.NET Core Identity.
- Không đưa connection string, secret vào code; dùng `appsettings.Development.json` (đã `.gitignore`) hoặc User Secrets.
- Không tin dữ liệu từ client: giá bán, tồn kho luôn tính lại ở server.

## Xử lý lỗi
- Lỗi nghiệp vụ (hết hàng, trùng mã vạch) dùng exception riêng hoặc `Result` rõ ràng, hiển thị thông báo tiếng Việt thân thiện.
- Không nuốt exception (`catch {}` rỗng). Ghi log bằng `ILogger`.
