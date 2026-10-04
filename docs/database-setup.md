# Cài đặt SQL Server cho SieuThiMini

## 1. Chọn bản SQL Server
| Bản | Khi nào dùng |
|---|---|
| LocalDB | Nhẹ nhất, đi kèm Visual Studio hoặc cài riêng. Hợp để phát triển trên máy cá nhân. |
| SQL Server Express | Miễn phí, chạy như dịch vụ thật. Hợp khi muốn gần với môi trường triển khai. |
| SQL Server Developer | Miễn phí cho phát triển, đầy đủ tính năng. |

Nên cài kèm **SQL Server Management Studio (SSMS)** hoặc dùng extension SQL trong IDE để xem dữ liệu.

## 2. Gói NuGet
```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet tool install --global dotnet-ef
```
Chọn phiên bản các gói **cùng major với .NET SDK** đang dùng (ví dụ .NET 8 thì dùng 8.x).

## 3. Chuỗi kết nối (`appsettings.json`)
LocalDB (Windows Authentication):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SieuThiMini;Trusted_Connection=True;TrustServerCertificate=True"
}
```
SQL Server Express (Windows Authentication):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=SieuThiMini;Trusted_Connection=True;TrustServerCertificate=True"
}
```
## 4. Cấu hình tài khoản mẫu qua User Secrets (Development)
Không lưu thông tin đăng nhập tài khoản seed vào file mã nguồn. Chạy các lệnh sau trong terminal:
```bash
# Khởi tạo user secrets (nếu chưa khởi tạo)
dotnet user-secrets init

# Tài khoản Admin mẫu
dotnet user-secrets set "SeedAdmin:Email" "admin@sieuthimini.vn"
dotnet user-secrets set "SeedAdmin:Password" "Admin@123"
dotnet user-secrets set "SeedAdmin:FullName" "Quản trị viên"

# Tài khoản Thu ngân (Cashier) mẫu
dotnet user-secrets set "SeedCashier:Email" "cashier@sieuthimini.vn"
dotnet user-secrets set "SeedCashier:Password" "Cashier@123"
dotnet user-secrets set "SeedCashier:FullName" "Thu ngân 01"
```

## 5. Đăng ký trong `Program.cs`
```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

## 6. Tạo database bằng migration
```bash
dotnet ef migrations add InitialIdentity
dotnet ef database update
```

Mỗi lần sửa entity: tạo migration mới có tên rõ nghĩa rồi `dotnet ef database update`.

## 7. Sự cố thường gặp
- **Không kết nối được**: kiểm tra dịch vụ SQL Server đang chạy (Services > SQL Server) và tên instance đúng.
- **Lỗi chứng chỉ SSL**: thêm `TrustServerCertificate=True` vào chuỗi kết nối (chỉ cho môi trường phát triển).
- **`dotnet ef` không nhận lệnh**: cài lại công cụ `dotnet-ef` hoặc mở terminal mới.
- **Phiên bản gói không khớp**: gỡ gói, cài lại đúng major với .NET SDK.
