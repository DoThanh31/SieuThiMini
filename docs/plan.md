# Kế hoạch: Web quản lý siêu thị nhỏ (SieuThiMini)

## 1. Mục tiêu
Phần mềm web giúp một siêu thị nhỏ quản lý hàng hóa, nhập hàng, bán hàng tại quầy và xem báo cáo cơ bản.

## 2. Người dùng và quyền
| Vai trò | Quyền |
|---|---|
| Admin (chủ cửa hàng) | Toàn quyền: quản lý người dùng, danh mục, sản phẩm, nhà cung cấp, nhập hàng, xem mọi báo cáo |
| Thu ngân | Chỉ bán hàng, xem sản phẩm, xem hóa đơn của chính mình |

## 3. Chức năng (theo thứ tự làm)
### Module 1: Nền tảng
- Tạo project MVC, EF Core + SQL Server, ASP.NET Core Identity (cài đặt DB xem `docs/database-setup.md`)
- Đăng nhập, đăng xuất, phân quyền Admin / Thu ngân
- Seed tài khoản Admin mặc định, layout Bootstrap 5

### Module 2: Danh mục và Sản phẩm
- CRUD danh mục
- CRUD sản phẩm: tên, mã vạch (duy nhất), danh mục, giá nhập, giá bán, đơn vị tính, tồn kho, ngưỡng cảnh báo tồn thấp
- Tìm kiếm, lọc theo danh mục, phân trang
- Xóa mềm

### Module 3: Nhà cung cấp và Nhập hàng
- CRUD nhà cung cấp
- Tạo phiếu nhập: chọn nhà cung cấp, thêm nhiều dòng sản phẩm (số lượng, giá nhập)
- Lưu phiếu nhập thì **tăng tồn kho** và cập nhật giá nhập gần nhất

### Module 4: Bán hàng (POS)
- Màn hình bán hàng: tìm sản phẩm theo tên hoặc mã vạch, thêm vào giỏ, sửa số lượng
- Thanh toán: tính tổng, tiền khách đưa, tiền thừa
- Lưu hóa đơn thì **trừ tồn kho** trong một transaction; không cho bán vượt tồn
- Lịch sử hóa đơn, xem chi tiết, in hóa đơn (trang in đơn giản)

### Module 5: Báo cáo
- Doanh thu theo ngày / tháng
- Top sản phẩm bán chạy
- Danh sách hàng sắp hết (dưới ngưỡng cảnh báo)
- Lợi nhuận gộp ước tính (giá bán - giá nhập)

## 4. Ngoài phạm vi (làm sau nếu cần)
Khách hàng thân thiết, khuyến mãi, thanh toán online, đa chi nhánh, ứng dụng di động.

## 5. Quy tắc nghiệp vụ quan trọng
- Mã vạch sản phẩm là duy nhất.
- Tồn kho không bao giờ âm.
- Giá bán và tổng tiền hóa đơn luôn do server tính lại.
- Hóa đơn đã tạo không sửa, không xóa; chỉ hủy (kèm lý do) nếu cần, và hủy phải hoàn lại tồn kho.
- Mọi thay đổi tồn kho đều ghi vào bảng StockMovement để truy vết.

## 6. Công nghệ
ASP.NET Core MVC (.NET 8+), EF Core, **SQL Server**, ASP.NET Core Identity, Bootstrap 5, xUnit.

## 7. Câu hỏi còn mở
- [ ] Có cần đọc mã vạch bằng máy quét không (máy quét thường hoạt động như bàn phím)?
- [ ] Đơn vị tiền tệ và cách làm tròn (VND, không có số lẻ)?
- [ ] Có cần xuất báo cáo ra Excel không?
