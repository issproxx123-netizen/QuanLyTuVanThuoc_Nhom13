$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
Set-Location $root

$jira = Read-Host "Nhập mã Jira, ví dụ SCRUM-59"
$slug = Read-Host "Nhập tên ngắn không dấu, ví dụ allergy-warning"
if ($jira -notmatch '^SCRUM-\d+$') { throw "Mã Jira không hợp lệ." }
if ([string]::IsNullOrWhiteSpace($slug)) { throw "Tên branch không được trống." }

$branch = "feature/$($jira.ToLower())-$slug"
git checkout develop
git pull origin develop
git checkout -b $branch
Write-Host "Đã tạo branch $branch" -ForegroundColor Green
