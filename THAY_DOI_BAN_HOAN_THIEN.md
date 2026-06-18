# THAY ĐỔI TRONG BẢN HOÀN THIỆN

## Sửa lỗi chạy dự án
- Sửa `CHAY_NGAY_WEB.bat` và `CHAY_NGAY_WEB.ps1` dùng đúng launch profile `QuanLyTuVanThuoc_Nhom13`.
- Giữ cổng chạy nhanh tại `http://localhost:5088`.

## Hoàn thiện nghiệp vụ
- Lưu cảnh báo dị ứng thuốc vào bảng `CanhBao` khi hệ thống chặn thuốc nguy hiểm.
- Đồng bộ cảnh báo tồn kho thấp và sắp hết hạn khi thêm/sửa/ngừng/kích hoạt/nhập/xuất thuốc.
- Xóa cảnh báo kho cũ khi tồn kho hoặc hạn sử dụng đã trở lại bình thường.
- Kiểm tra từng dòng thuốc: không cho chọn thuốc nhưng để số lượng bằng 0.
- Giữ transaction cho nhập kho và phiếu tư vấn.

## Hoàn thiện báo cáo và bảo mật
- Chuẩn hóa CSV UTF-8 cho Excel, escape dấu phẩy và dấu ngoặc kép, dùng định dạng số bất biến.
- Ngăn Admin tự khóa, tự gỡ quyền Admin và ngăn khóa Admin cuối cùng.
- Bổ sung trang Privacy còn thiếu.
- Bảo vệ phép tính giá trị tồn kho khi cơ sở dữ liệu chưa có thuốc.

## Kiểm tra tĩnh đã thực hiện
- Kiểm tra cân bằng cú pháp các file C#.
- Kiểm tra JSON, XML cấu hình.
- Kiểm tra các `asp-action` trong View đều có Action tương ứng.
- Kiểm tra launch profile trong hai script chạy nhanh.

## Việc cần xác nhận trên máy Windows
Do môi trường đóng gói không có .NET 8 SDK và SQL Server LocalDB, người dùng cần mở Visual Studio 2022 và thực hiện:
1. Build > Rebuild Solution.
2. Xác nhận Error List = 0 Errors.
3. Ctrl + F5.
4. Thực hiện checklist trong `KIEM_TRA_TRUOC_KHI_NOP.txt`.
