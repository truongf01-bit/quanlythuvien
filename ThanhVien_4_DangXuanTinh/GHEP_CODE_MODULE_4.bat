@echo off
chcp 65001 >nul
title [MODULE 4 - ĐẶNG XUÂN TÌNH] GHÉP CODE VÀO DỰ ÁN QUẢN LÝ THƯ VIỆN
color 0B
echo ===============================================================================
echo     [MODULE 4] ĐẶNG XUÂN TÌNH - GHÉP CODE VÀO DỰ ÁN
echo     Nghiệp vụ: Phiếu mượn/trả, Xử lý Quá hạn, Thanh toán QR MB Bank/MoMo & Thống kê
echo ===============================================================================
echo.
set PROJ_DIR=%~dp0..\..\quanlythuvien

if not exist "%PROJ_DIR%\QuanLyThuVien.csproj" (
    echo [LỖI] Không tìm thấy thư mục dự án gốc quanlythuvien!
    pause
    exit /b
)

echo [1/5] Đang sao chép Controllers (PhieuMuonController, ThongKeController, ThanhToanController)...
xcopy "%~dp0Controllers\*" "%PROJ_DIR%\Controllers\" /Y /Q /E >nul

echo [2/5] Đang sao chép Models & ViewModels (PhieuMuon, DashboardViewModel, ThongKe, ThanhToan)...
xcopy "%~dp0Models\*" "%PROJ_DIR%\Models\" /Y /Q /E >nul
xcopy "%~dp0ViewModels\*" "%PROJ_DIR%\ViewModels\" /Y /Q /E >nul

echo [3/5] Đang sao chép Views (PhieuMuon, ThongKe, ThanhToan)...
xcopy "%~dp0Views\*" "%PROJ_DIR%\Views\" /Y /Q /E >nul

echo [4/5] Đang sao chép QR Code thanh toán (nếu có cập nhật ảnh)...
if exist "%~dp0wwwroot_images_payments" (
    xcopy "%~dp0wwwroot_images_payments\*" "%PROJ_DIR%\wwwroot\images\payments\" /Y /Q /E >nul
)

echo [5/5] Đang biên dịch kiểm tra dự án (dotnet build)...
taskkill /F /IM QuanLyThuVien.exe >nul 2>&1
cd /d "%PROJ_DIR%"
dotnet build
if %ERRORLEVEL% NEQ 0 (
    color 0C
    echo.
    echo [LỖI BIÊN DỊCH] Code của bạn có lỗi cú pháp, vui lòng kiểm tra lại!
    pause
    exit /b
)

color 0A
echo.
echo ===============================================================================
echo     [THÀNH CÔNG] GHÉP CODE MODULE 4 CỦA BẠN VÀO DỰ ÁN HOÀN TẤT (0 LỖI)!
echo ===============================================================================
echo.
set /p RUN_APP="Bạn có muốn khởi chạy website ngay để kiểm tra không? (Y/N): "
if /i "%RUN_APP%"=="Y" (
    echo Đang khởi chạy website tại http://localhost:5000 ...
    start http://localhost:5000
    dotnet run --urls "http://localhost:5000"
)
pause
