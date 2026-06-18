# Checklist minh chứng GitHub cho báo cáo 3.5

## Repository

- [ ] Trang chính repository hiển thị README.
- [ ] README có mô tả, công nghệ, chức năng, thành viên và hướng dẫn cài đặt.
- [ ] Tab Wiki có Home/Cài đặt/Quy trình Git/Release.
- [ ] Tab Releases có `v0.1`, `v0.2`, `v1.0`.

## Branch

- [ ] Branch `main`.
- [ ] Branch `develop`.
- [ ] Có các branch `feature/*` của từng thành viên.
- [ ] Có branch `release/v0.1`, `release/v0.2`, `release/v1.0` hoặc lịch sử tag tương ứng.

## Agile Development

- [ ] Pull Request từ feature branch vào develop.
- [ ] PR có Jira URL và mã SCRUM.
- [ ] PR có reviewer khác người viết code.
- [ ] PR có comment Code Review.
- [ ] GitHub Actions build xanh.
- [ ] Có minh chứng Testing trước merge.
- [ ] Merge develop vào release/main.

## Lịch sử thành viên

- [ ] Trang Contributors hiển thị thành viên.
- [ ] Commits hiển thị đúng tên/avatar từng tài khoản.
- [ ] Mỗi thành viên có ít nhất một commit thật.
- [ ] Không sửa giả author hoặc backdate commit.

## Ảnh nên chụp

1. Repository + README.
2. Danh sách branches.
3. Commit history có nhiều thành viên.
4. Một feature branch có commit gắn mã Jira.
5. Pull Request mở vào develop.
6. Code Review/Approval.
7. GitHub Actions build thành công.
8. Pull Requests đã merged.
9. Releases v0.1, v0.2, v1.0.
10. Wiki.
11. Contributors.
