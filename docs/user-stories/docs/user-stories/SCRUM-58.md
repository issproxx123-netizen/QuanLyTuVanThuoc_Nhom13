# SCRUM-58 - Chặn kê thuốc vượt tồn kho

## Jira Issue

https://trinhbaoanh2380600085.atlassian.net/browse/SCRUM-58

## Người thực hiện

Trần Duy Khương

## Mục tiêu

Hệ thống không cho phép bác sĩ hoặc nhân viên tư vấn kê số lượng thuốc vượt quá số lượng tồn kho hiện tại.

## Acceptance Criteria

* Đọc đúng số lượng tồn của thuốc từ cơ sở dữ liệu.
* So sánh số lượng kê với số lượng tồn hiện tại.
* Hiển thị lỗi khi số lượng kê vượt tồn kho.
* Không lưu phiếu tư vấn khi dữ liệu không hợp lệ.
* Không thay đổi tồn kho nếu quá trình lưu thất bại.
* Cho phép lưu khi số lượng kê nhỏ hơn hoặc bằng tồn kho.

## Test Case

1. Số lượng kê nhỏ hơn số lượng tồn.
2. Số lượng kê bằng số lượng tồn.
3. Số lượng kê lớn hơn số lượng tồn.
4. Số lượng kê bằng 0.
5. Số lượng kê là số âm.
6. Phiếu có nhiều thuốc, trong đó một thuốc vượt tồn kho.

## Database Mapping

* Bảng `Thuoc`: đọc trường `SoLuongTon`.
* Bảng `DonTuVan`: lưu thông tin phiếu tư vấn.
* Bảng `ChiTietDonTuVan`: lưu thuốc và số lượng kê.
* Sử dụng transaction khi lưu phiếu và cập nhật tồn kho.

## Kết quả mong đợi

Hệ thống chỉ lưu phiếu tư vấn khi tất cả các dòng thuốc đều hợp lệ và không có thuốc nào vượt số lượng tồn kho.
