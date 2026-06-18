@echo off
chcp 65001 >nul
cd /d "%~dp0"
title CAI DAT CSDL VA CHAY WEB - NHOM 13

echo ============================================================
echo   CAI DAT CSDL SQL SERVER + CHAY VISUAL STUDIO WEB
echo ============================================================
echo.

powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0QuanLyTuVanThuoc_Nhom13\Database\TAO_LAI_CSDL_TU_DONG.ps1"
if errorlevel 1 (
    echo.
    echo [LOI] Khong tao duoc CSDL. Web chua duoc khoi dong.
    pause
    exit /b 1
)

echo.
echo Dang khoi dong website...
call "%~dp0CHAY_NGAY_WEB.bat"
