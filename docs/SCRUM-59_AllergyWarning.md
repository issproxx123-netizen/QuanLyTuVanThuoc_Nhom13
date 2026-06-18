# SCRUM-59 - Cảnh báo dị ứng thuốc

## Jira Issue

https://trinhbaoanh2380600085.atlassian.net/browse/SCRUM-59

## Mục tiêu

Hệ thống kiểm tra tiền sử dị ứng của bệnh nhân khi bác sĩ hoặc nhân viên tư vấn chọn thuốc trong phiếu tư vấn.

## Acceptance Criteria

- Lấy đúng thông tin dị ứng của bệnh nhân.
- Kiểm tra thuốc trước khi lưu phiếu tư vấn.
- Hiển thị cảnh báo khi thuốc có nguy cơ gây dị ứng.
- Không cảnh báo sai với thuốc không liên quan.
- Lưu cảnh báo an toàn vào cơ sở dữ liệu.

## Test Case

1. Bệnh nhân có dị ứng và chọn thuốc liên quan.
2. Bệnh nhân có dị ứng nhưng chọn thuốc không liên quan.
3. Bệnh nhân chưa khai báo tiền sử dị ứng.
4. Phiếu tư vấn có nhiều loại thuốc.
5. Kiểm tra bản ghi cảnh báo trong cơ sở dữ liệu.
