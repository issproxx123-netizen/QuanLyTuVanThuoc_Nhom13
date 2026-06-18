$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
Set-Location $root

if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    throw "Chưa cài Git. Hãy cài Git for Windows trước."
}

if (-not (Test-Path ".git")) {
    git init -b main
}

Write-Host "Kiểm tra file trước khi commit..." -ForegroundColor Cyan
git status

$repoUrl = Read-Host "Nhập URL repository GitHub RỖNG (ví dụ https://github.com/user/repo.git)"
if ([string]::IsNullOrWhiteSpace($repoUrl)) {
    throw "Repository URL không được để trống."
}

$origin = git remote get-url origin 2>$null
if ($LASTEXITCODE -eq 0) {
    git remote set-url origin $repoUrl
} else {
    git remote add origin $repoUrl
}

git add .
git commit -m "SCRUM-138 chore(repo): khởi tạo repository và tài liệu dự án"
git push -u origin main

git checkout -b develop
git push -u origin develop

git checkout main
Write-Host "Đã push main và develop. Tiếp theo mời từng thành viên clone repo và tạo feature branch thật." -ForegroundColor Green
