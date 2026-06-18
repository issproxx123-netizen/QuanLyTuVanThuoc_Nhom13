# Kế hoạch commit thật của từng thành viên

Không tạo commit giả hoặc đổi author của người khác. Mỗi thành viên phải đăng nhập GitHub, cấu hình Git bằng email đã xác minh và tự push thay đổi của mình.

| Thành viên | Công việc/nhánh phù hợp để tạo commit thật |
|---|---|
| Trình Bảo Anh | README, Dashboard, quản trị tài khoản, Release |
| Trần Duy Khương | GitHub Actions, test cases, review checklist |
| Triệu Thị Huyên | Cảnh báo tồn kho/dị ứng, phiếu tư vấn |
| Dương Thị Thanh Thuý | Loại thuốc, thuốc, phiếu nhập kho |
| Bùi Thị Lệ Quyên | CSS/UI, Figma docs, in phiếu, tài liệu demo |
| Phạm Ngọc Lợi | Bệnh nhân, lịch sử tư vấn, CSV |

Quy trình cho mỗi thành viên:

```bash
git clone <repo-url>
cd <repo>
git checkout develop
git pull origin develop
git checkout -b feature/scrum-XX-ten-chuc-nang
# sửa đúng file thuộc nhiệm vụ
git add .
git commit -m "SCRUM-XX feat(scope): mô tả thay đổi thật"
git push -u origin feature/scrum-XX-ten-chuc-nang
```

Sau đó tạo Pull Request vào `develop`, gắn Jira URL, yêu cầu thành viên khác review và chỉ merge khi CI xanh.
