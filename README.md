# Hệ thống Quản lý Tư vấn Thuốc - Nhóm 13

Đồ án môn Công nghệ phần mềm tại HUTECH, xây dựng bằng ASP.NET Core MVC .NET 8, Entity Framework Core và SQL Server LocalDB.

## Thành viên và vai trò Scrum

| Họ tên | MSSV | Vai trò |
|---|---:|---|
| Trình Bảo Anh | 2380600085 | Product Owner |
| Trần Duy Khương | 2380601115 | Scrum Master |
| Triệu Thị Huyên | 2380600912 | Development Team |
| Dương Thị Thanh Thuý | 2380603181 | Development Team |
| Bùi Thị Lệ Quyên | 2380601869 | Development Team |
| Phạm Ngọc Lợi | 2380601290 | Development Team |

## Chức năng chính

- Đăng nhập, đăng xuất và phân quyền theo vai trò.
- Quản lý người dùng, bệnh nhân, loại thuốc và thuốc.
- Lập phiếu nhập kho và cập nhật số lượng tồn.
- Lập phiếu tư vấn, kiểm tra an toàn thuốc và trừ tồn kho.
- Cảnh báo tồn thấp, sắp hết hạn và nguy cơ dị ứng.
- Dashboard, báo cáo, xuất CSV và in phiếu tư vấn.

## Công nghệ

- C# và ASP.NET Core MVC .NET 8.
- Razor Views, HTML, CSS và JavaScript.
- Entity Framework Core 8.0.10.
- SQL Server LocalDB.
- xUnit và EF Core InMemory cho kiểm thử.

## Cài đặt và chạy

### Yêu cầu

- Visual Studio 2022 với workload ASP.NET and web development.
- .NET 8 SDK.
- SQL Server LocalDB hoặc SQL Server Express.

### Các bước

1. Mở `QuanLyTuVanThuoc_Nhom13.sln`.
2. Chờ Visual Studio Restore NuGet.
3. Chọn `Build` - `Rebuild Solution`.
4. Nhấn `Ctrl + F5` để chạy.
5. Hệ thống sử dụng địa chỉ `http://localhost:5088` theo cấu hình phát triển.

### Cơ sở dữ liệu

- Server: `(localdb)\MSSQLLocalDB`.
- Database: `QuanLyTuVanThuocDB`.
- Connection string: `QuanLyTuVanThuoc_Nhom13/appsettings.json`.
- Script SQL: `QuanLyTuVanThuoc_Nhom13/Database/Tao_CSDL_QuanLyTuVanThuocDB.sql`.

### Tài khoản demo

Mật khẩu chung: `123456`.

- `admin` - Quản trị viên.
- `bacsi` - Bác sĩ hoặc nhân viên tư vấn.
- `kho` - Nhân viên kho.
- `quanly` - Quản lý.

## Kiểm thử

Project `QuanLyTuVanThuoc_Nhom13.Tests` gồm:

- `DrugSafetyServiceTests.cs`.
- `PasswordHelperTests.cs`.
- `WarningServiceIntegrationTests.cs`.
