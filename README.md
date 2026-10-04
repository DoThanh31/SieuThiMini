# SieuThiMini

Web quản lý siêu thị nhỏ bằng ASP.NET Core MVC.

## Yêu cầu
- .NET SDK 8+
- SQL Server (LocalDB, Express hoặc Developer), xem `docs/database-setup.md`
- Công cụ EF: `dotnet tool install --global dotnet-ef`

## Chạy dự án
```
dotnet restore
dotnet ef database update
dotnet run
```

## Tài liệu
- `docs/plan.md`: kế hoạch và yêu cầu
- `docs/architecture.md`: kiến trúc, entity, services
- `docs/progress.md`: tiến độ
- `docs/database-setup.md`: cài đặt SQL Server, chuỗi kết nối, migration
- `.agents/rules/`: quy tắc cho agent AI trong Antigravity
