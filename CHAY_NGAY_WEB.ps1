$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot
Write-Host '============================================================' -ForegroundColor Cyan
Write-Host '  HỆ THỐNG QUẢN LÝ TƯ VẤN THUỐC - NHÓM 13' -ForegroundColor Cyan
Write-Host '============================================================' -ForegroundColor Cyan

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Host 'Máy chưa có .NET 8 SDK. Hãy mở file QuanLyTuVanThuoc_Nhom13.sln bằng Visual Studio 2022 và cài workload ASP.NET and web development.' -ForegroundColor Red
    Read-Host 'Nhấn Enter để đóng'
    exit 1
}

$project = Join-Path $PSScriptRoot 'QuanLyTuVanThuoc_Nhom13\QuanLyTuVanThuoc_Nhom13.csproj'
if (-not (Test-Path $project)) {
    Write-Host 'Không tìm thấy file project. Vui lòng giải nén toàn bộ gói trước khi chạy.' -ForegroundColor Red
    Read-Host 'Nhấn Enter để đóng'
    exit 1
}

Write-Host 'Đang khởi động website: http://localhost:5088' -ForegroundColor Green
Write-Host 'Tài khoản: admin | Mật khẩu: 123456' -ForegroundColor Yellow
Start-Job -ScriptBlock { Start-Sleep -Seconds 4; Start-Process 'http://localhost:5088' } | Out-Null
dotnet run --project $project --launch-profile "QuanLyTuVanThuoc_Nhom13"
