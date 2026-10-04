# Quy trình làm việc

## Trước khi code
1. Đọc `docs/plan.md`, `docs/architecture.md`, `docs/progress.md`.
2. Tóm tắt ngắn (2-3 dòng) việc sắp làm và liệt kê các file sẽ tạo hoặc sửa. Chờ tôi đồng ý nếu thay đổi lớn.

## Khi code
- Làm **một module mỗi lần**, theo thứ tự trong `docs/plan.md`.
- Sau khi sửa code phải chạy `dotnet build`. Có lỗi thì sửa cho đến khi build sạch.
- Thay đổi model thì tạo migration: `dotnet ef migrations add <Ten>` rồi `dotnet ef database update`.
- Với logic nghiệp vụ quan trọng (bán hàng, nhập hàng, tồn kho) phải viết unit test xUnit đi kèm.
- Không đổi thư viện, phiên bản .NET, hoặc cấu trúc thư mục khi chưa hỏi tôi.

## Sau khi xong module
- Chạy `dotnet test` và `dotnet build`.
- Cập nhật `docs/progress.md`: đánh dấu module đã xong, ghi chú vấn đề còn tồn đọng.
- Báo cáo ngắn: đã làm gì, cách chạy thử, những gì tôi cần tự kiểm tra trên trình duyệt.
- Gợi ý nội dung commit message (tiếng Anh, dạng `feat: ...`, `fix: ...`). Không tự commit.

## Khi gặp lỗi
- Đọc kỹ thông báo lỗi, tìm nguyên nhân gốc rồi mới sửa. Không sửa mò.
- Nếu không chắc, nêu giả thuyết và hỏi tôi thay vì đoán.

## Không được làm
- Không xóa dữ liệu hoặc database của tôi.
- Không chạy lệnh phá hoại (`git reset --hard`, xóa thư mục) khi chưa hỏi.
- Không bỏ qua test đang fail bằng cách xóa hoặc comment test.
