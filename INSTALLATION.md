# Hướng dẫn cài đặt

## 1. Chuẩn bị môi trường

- Windows 10/11 64-bit.
- Visual Studio 2022.
- Workload `ASP.NET and web development`.
- .NET 8 SDK.
- SQL Server LocalDB hoặc SQL Server Express.
- SQL Server Management Studio nếu muốn xem/chạy script thủ công.

## 2. Clone source

```bash
git clone https://github.com/OWNER/REPOSITORY.git
cd REPOSITORY
```

## 3. Cách chạy không xóa dữ liệu

1. Mở `QuanLyTuVanThuoc_Nhom13.sln`.
2. Chờ Visual Studio restore NuGet.
3. Chọn `Build → Rebuild Solution`.
4. Xác nhận `0 Errors`.
5. Nhấn `Ctrl + F5`.

Ứng dụng dùng:

```text
Server=(localdb)\MSSQLLocalDB
Database=QuanLyTuVanThuocDB
```

Nếu database chưa có, chương trình tự tạo database và dữ liệu mẫu.

## 4. Tạo lại database từ SQL

> Thao tác này xóa dữ liệu hiện tại.

Chạy:

```text
01_TAO_LAI_CSDL_VA_CHAY_WEB.bat
```

Nhập `YES` khi được hỏi.

Script được lưu tại:

```text
QuanLyTuVanThuoc_Nhom13/Database/Tao_CSDL_QuanLyTuVanThuocDB.sql
```

## 5. Kiểm tra kết nối

Chạy:

```text
02_KIEM_TRA_KET_NOI_CSDL.bat
```

Kết quả đúng sẽ hiển thị tên database và số bản ghi của các bảng mẫu.

## 6. Tài khoản demo

Mật khẩu chung `123456`:

- `admin`
- `bacsi`
- `kho`
- `quanly`
