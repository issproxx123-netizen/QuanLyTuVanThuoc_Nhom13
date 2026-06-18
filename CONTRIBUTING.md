# Hướng dẫn đóng góp

## Nguyên tắc bắt buộc

1. Không code trực tiếp trên `main` hoặc `develop`.
2. Mỗi User Story dùng một branch `feature/*` tạo từ `develop`.
3. Branch, commit và Pull Request phải có mã Jira `SCRUM-xx`.
4. Pull Request phải liên kết Jira Issue bằng URL đầy đủ.
5. Người viết code không tự duyệt Pull Request của chính mình.
6. Chỉ merge khi CI build xanh, đã Code Review và Testing.

## Cấu hình Git cho từng thành viên

Mỗi thành viên thực hiện trên máy của mình bằng tên và email gắn với tài khoản GitHub:

```bash
git config --global user.name "Họ tên thành viên"
git config --global user.email "email-da-xac-minh-tren-github@example.com"
```

Không dùng chung một tài khoản GitHub nếu cần chứng minh lịch sử commit của từng thành viên.

## Tạo feature branch

```bash
git checkout develop
git pull origin develop
git checkout -b feature/scrum-46-login
```

Hoặc chạy `scripts/TAO_FEATURE_BRANCH.ps1`.

## Quy ước commit

```text
SCRUM-<mã> <type>(<scope>): <mô tả ngắn>
```

Các `type` khuyến nghị:

- `feat`: chức năng mới
- `fix`: sửa lỗi
- `test`: kiểm thử
- `docs`: tài liệu
- `refactor`: cải tiến code
- `chore`: cấu hình/công cụ

Ví dụ:

```bash
git commit -m "SCRUM-46 feat(auth): hoàn thiện đăng nhập và phân quyền"
git commit -m "SCRUM-55 test(alert): bổ sung test cảnh báo tồn kho"
git commit -m "SCRUM-65 docs(install): cập nhật hướng dẫn cài đặt"
```

## Push và tạo Pull Request

```bash
git push -u origin feature/scrum-46-login
```

Trên GitHub:

- Base branch: `develop`
- Compare branch: `feature/scrum-46-login`
- Title: `[SCRUM-46] Hoàn thiện đăng nhập và phân quyền`
- Jira: `https://trinhbaoanh2380600085.atlassian.net/browse/SCRUM-46`

Điền đầy đủ checklist từ `.github/pull_request_template.md`.

## Review và merge

Reviewer kiểm tra:

- Acceptance Criteria.
- Code style và logic nghiệp vụ.
- Không lộ secret.
- Build GitHub Actions thành công.
- Có minh chứng kiểm thử.

Sau khi đạt, merge bằng `Squash and merge` hoặc `Create a merge commit` theo thống nhất của nhóm, rồi xóa feature branch.
