@echo off
chcp 65001 >nul
cd /d "%~dp0"
title HUTECH CLINIC - Quản lý Tư vấn Thuốc

echo ============================================================
echo   HỆ THỐNG QUẢN LÝ TƯ VẤN THUỐC - NHÓM 13
echo ============================================================
echo.

where dotnet >nul 2>nul
if errorlevel 1 (
    echo [THIẾU MÔI TRƯỜNG] Máy chưa có .NET 8 SDK hoặc chưa nhận lệnh dotnet.
    echo.
    echo Cách xử lý nhanh:
    echo 1. Mở QuanLyTuVanThuoc_Nhom13.sln bằng Visual Studio 2022.
    echo 2. Cài workload ASP.NET and web development nếu Visual Studio yêu cầu.
    echo 3. Nhấn Ctrl + F5.
    echo.
    pause
    exit /b 1
)

if not exist "QuanLyTuVanThuoc_Nhom13\QuanLyTuVanThuoc_Nhom13.csproj" (
    echo [LỖI] Không tìm thấy file project. Vui lòng giải nén toàn bộ gói trước khi chạy.
    pause
    exit /b 1
)

echo Đang khởi động website tại http://localhost:5088 ...
echo Tài khoản: admin  -  Mật khẩu: 123456
echo.
start "" powershell -NoProfile -WindowStyle Hidden -Command "Start-Sleep -Seconds 4; Start-Process 'http://localhost:5088'"
dotnet run --project "QuanLyTuVanThuoc_Nhom13\QuanLyTuVanThuoc_Nhom13.csproj" --launch-profile "QuanLyTuVanThuoc_Nhom13"

echo.
echo Website đã dừng. Nhấn phím bất kỳ để đóng cửa sổ.
pause >nul
