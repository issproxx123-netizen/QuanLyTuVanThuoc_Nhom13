# 3.5. Quản lý mã nguồn

Nhóm sử dụng Git và GitHub để quản lý mã nguồn theo quy trình Agile Development. Repository được tổ chức với ba nhóm nhánh chính: `main`, `develop` và `feature/*`. Nhánh `main` chứa các phiên bản ổn định đã phát hành; nhánh `develop` dùng để tích hợp các User Story đã hoàn thành; mỗi User Story được phát triển trên một nhánh `feature/scrum-<mã-jira>-<tên-ngắn>` riêng.

Mỗi commit và Pull Request đều chứa mã Jira Issue, ví dụ `SCRUM-59 feat(alert): bổ sung cảnh báo dị ứng thuốc`, đồng thời phần mô tả Pull Request liên kết trực tiếp đến User Story tương ứng trên Jira. Quy trình phát triển được thực hiện theo thứ tự: Developer → Feature Branch → Pull Request → Code Review → GitHub Actions Build → Testing → Merge Develop → Sprint Release → Merge Main.

Repository có README, hướng dẫn cài đặt, tài liệu quy trình nhánh, mapping Jira–GitHub, Pull Request Template, Issue Template, CODEOWNERS, GitHub Actions và tài liệu Wiki. Ba Sprint tương ứng ba Increment được phát hành dưới dạng GitHub Release: `v0.1`, `v0.2` và `v1.0`.

Lịch sử commit được thực hiện bằng tài khoản GitHub riêng của từng thành viên. Mỗi thành viên cấu hình tên và email Git bằng thông tin đã xác minh trên GitHub, tự thực hiện commit và push trên feature branch được phân công. Điều này giúp GitHub hiển thị chính xác lịch sử đóng góp và danh sách Contributors.
