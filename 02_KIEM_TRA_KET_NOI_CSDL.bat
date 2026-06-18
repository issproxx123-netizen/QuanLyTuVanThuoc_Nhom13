@echo off
chcp 65001 >nul
cd /d "%~dp0"
title KIEM TRA KET NOI CSDL - NHOM 13
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0QuanLyTuVanThuoc_Nhom13\Database\KIEM_TRA_KET_NOI_CSDL.ps1"
echo.
pause
