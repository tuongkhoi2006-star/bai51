# LoginFormApp - C# WinForms + SQL Server LocalDB

## Chức năng
- Đăng nhập và đăng ký tài khoản lưu trong SQL Server.
- Kiểm tra bỏ trống bằng ErrorProvider.
- Mặc định ẩn mật khẩu; checkbox cho phép hiện mật khẩu.
- Enter kích hoạt Đăng nhập, Esc kích hoạt Thoát.
- Mật khẩu được băm bằng PBKDF2-SHA256 với salt riêng; truy vấn SQL có tham số.

## Yêu cầu
- Windows và Visual Studio có workload .NET desktop development.
- .NET 8 SDK (hoặc cài Targeting Pack phù hợp).
- SQL Server Express LocalDB (thường được cài cùng Visual Studio).
- Có kết nối Internet lần đầu để NuGet tải Microsoft.Data.SqlClient.

## Cách chạy
1. Mở `LoginFormApp.csproj` bằng Visual Studio.
2. Mở `database.sql`, chạy toàn bộ script bằng SQL Server Management Studio hoặc công cụ truy vấn kết nối tới `(localdb)\\MSSQLLocalDB`.
3. Nếu chưa có LocalDB, cài SQL Server Express LocalDB hoặc sửa chuỗi kết nối trong `Database.cs` theo SQL Server đang dùng.
4. Trong Visual Studio, chọn Build > Build Solution, sau đó nhấn F5.
5. Nhập tên đăng nhập và mật khẩu (ít nhất 8 ký tự), nhấn **Đăng ký** để tạo tài khoản.
6. Đăng nhập bằng tài khoản vừa tạo.

## Lưu ý
- Tài khoản được lưu trong database `LoginDB`, bảng `dbo.Users`.
- Nếu dùng SQL Server instance khác, sửa `ConnectionString` trong `Database.cs`.
- Đây là dự án học tập; ứng dụng triển khai thực tế cần thêm giới hạn thử đăng nhập, chính sách mật khẩu và quản lý cấu hình bí mật phù hợp.
