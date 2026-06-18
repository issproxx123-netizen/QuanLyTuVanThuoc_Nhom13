# Chiến lược nhánh Agile Development

## Các nhánh chính

### `main`

- Chỉ chứa phiên bản đã phát hành.
- Mỗi lần merge phải tương ứng một GitHub Release/Increment.
- Tag: `v0.1`, `v0.2`, `v1.0`.

### `develop`

- Nhánh tích hợp User Story đã review và kiểm thử.
- Tất cả feature Pull Request đều merge vào đây.

### `feature/*`

Định dạng:

```text
feature/scrum-<jira-key>-<ten-ngan>
```

Ví dụ:

```text
feature/scrum-46-login
feature/scrum-54-stock-receipt
feature/scrum-59-allergy-warning
```

### `release/*`

- `release/v0.1`
- `release/v0.2`
- `release/v1.0`

Tạo từ `develop` khi Sprint đã hoàn thành, dùng cho kiểm thử cuối và sửa lỗi phát hành.

## Luồng chuẩn

```text
Developer
→ Feature Branch
→ Pull Request vào develop
→ Code Review
→ GitHub Actions Build
→ Testing
→ Merge develop
→ Tạo release/vX
→ Sprint Review / Regression Test
→ Pull Request vào main
→ Tag và GitHub Release
```

## Quy tắc bảo vệ nhánh

Khuyến nghị bật rule cho `main` và `develop`:

- Require a pull request before merging.
- Require at least 1 approval.
- Require status checks to pass.
- Block force pushes.
- Block deletion.
