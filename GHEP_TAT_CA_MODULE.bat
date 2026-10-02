@echo off
chcp 65001 >nul
title [TỔNG HỢP NHÓM] GHÉP TẤT CẢ 4 MODULE VÀO DỰ ÁN QUẢN LÝ THƯ VIỆN
color 0B
echo ===============================================================================
echo     [TỔNG HỢP NHÓM 04 THÀNH VIÊN] ĐỒ ÁN HỆ THỐNG QUẢN LÝ THƯ VIỆN (UNETI)
echo     Nhóm thực hiện: Trương Trọng Cường, Nguyễn Xuân Trường, Nguyễn Viết Hồng, Đặng Xuân Tình
echo ===============================================================================
echo.
set ROOT_DIR=%~dp0
if not exist "%ROOT_DIR%quanlythuvien\QuanLyThuVien.csproj" (
    set ROOT_DIR=%~dp0..\
)

set PROJ_DIR=%ROOT_DIR%quanlythuvien
set MODS_DIR=%ROOT_DIR%MODULES_4_THANH_VIEN

if not exist "%PROJ_DIR%\QuanLyThuVien.csproj" (
    echo [LỖI] Không tìm thấy thư mục dự án gốc quanlythuvien tại: %PROJ_DIR%
    pause
    exit /b
)

echo [BƯỚC 1/5] Ghép Module 1 của bạn TRƯƠNG TRỌNG CƯỜNG (Tài khoản, Đăng nhập, Thể loại)...
xcopy "%MODS_DIR%\ThanhVien_1_TruongTrongCuong\Controllers\*" "%PROJ_DIR%\Controllers\" /Y /Q /E >nul
xcopy "%MODS_DIR%\ThanhVien_1_TruongTrongCuong\Models\*" "%PROJ_DIR%\Models\" /Y /Q /E >nul
xcopy "%MODS_DIR%\ThanhVien_1_TruongTrongCuong\ViewModels\*" "%PROJ_DIR%\ViewModels\" /Y /Q /E >nul
xcopy "%MODS_DIR%\ThanhVien_1_TruongTrongCuong\Views\*" "%PROJ_DIR%\Views\" /Y /Q /E >nul
echo   -> Hoàn thành ghép Module 1.

echo.
echo [BƯỚC 2/5] Ghép Module 2 của bạn NGUYỄN XUÂN TRƯỜNG (Sách, Tìm kiếm, Lọc, Giao diện)...
xcopy "%MODS_DIR%\ThanhVien_2_NguyenXuanTruong\Controllers\*" "%PROJ_DIR%\Controllers\" /Y /Q /E >nul
xcopy "%MODS_DIR%\ThanhVien_2_NguyenXuanTruong\Models\*" "%PROJ_DIR%\Models\" /Y /Q /E >nul
xcopy "%MODS_DIR%\ThanhVien_2_NguyenXuanTruong\ViewModels\*" "%PROJ_DIR%\ViewModels\" /Y /Q /E >nul
xcopy "%MODS_DIR%\ThanhVien_2_NguyenXuanTruong\Views\*" "%PROJ_DIR%\Views\" /Y /Q /E >nul
if exist "%MODS_DIR%\ThanhVien_2_NguyenXuanTruong\Shared_Layout\_Layout.cshtml" (
    xcopy "%MODS_DIR%\ThanhVien_2_NguyenXuanTruong\Shared_Layout\_Layout.cshtml" "%PROJ_DIR%\Views\Shared\" /Y /Q >nul
)
if exist "%MODS_DIR%\ThanhVien_2_NguyenXuanTruong\wwwroot_css\site.css" (
    xcopy "%MODS_DIR%\ThanhVien_2_NguyenXuanTruong\wwwroot_css\site.css" "%PROJ_DIR%\wwwroot\css\" /Y /Q >nul
)
echo   -> Hoàn thành ghép Module 2.

echo.
echo [BƯỚC 3/5] Ghép Module 3 của bạn NGUYỄN VIẾT HỒNG (Độc giả, Mượn sách trực tuyến)...
xcopy "%MODS_DIR%\ThanhVien_3_NguyenVietHong\Controllers\*" "%PROJ_DIR%\Controllers\" /Y /Q /E >nul
xcopy "%MODS_DIR%\ThanhVien_3_NguyenVietHong\Models\*" "%PROJ_DIR%\Models\" /Y /Q /E >nul
xcopy "%MODS_DIR%\ThanhVien_3_NguyenVietHong\ViewModels\*" "%PROJ_DIR%\ViewModels\" /Y /Q /E >nul
xcopy "%MODS_DIR%\ThanhVien_3_NguyenVietHong\Views\*" "%PROJ_DIR%\Views\" /Y /Q /E >nul
echo   -> Hoàn thành ghép Module 3.

echo.
echo [BƯỚC 4/5] Ghép Module 4 của bạn ĐẶNG XUÂN TÌNH (Phiếu mượn/trả, Quá hạn, QR MB Bank/MoMo, Thống kê)...
xcopy "%MODS_DIR%\ThanhVien_4_DangXuanTinh\Controllers\*" "%PROJ_DIR%\Controllers\" /Y /Q /E >nul
xcopy "%MODS_DIR%\ThanhVien_4_DangXuanTinh\Models\*" "%PROJ_DIR%\Models\" /Y /Q /E >nul
xcopy "%MODS_DIR%\ThanhVien_4_DangXuanTinh\ViewModels\*" "%PROJ_DIR%\ViewModels\" /Y /Q /E >nul
xcopy "%MODS_DIR%\ThanhVien_4_DangXuanTinh\Views\*" "%PROJ_DIR%\Views\" /Y /Q /E >nul
if exist "%MODS_DIR%\ThanhVien_4_DangXuanTinh\wwwroot_images_payments" (
    xcopy "%MODS_DIR%\ThanhVien_4_DangXuanTinh\wwwroot_images_payments\*" "%PROJ_DIR%\wwwroot\images\payments\" /Y /Q /E >nul
)
echo   -> Hoàn thành ghép Module 4.

echo.
echo [BƯỚC 5/5] Đang biên dịch tổng thể toàn bộ dự án (dotnet build)...
taskkill /F /IM QuanLyThuVien.exe >nul 2>&1
cd /d "%PROJ_DIR%"
dotnet build
if %ERRORLEVEL% NEQ 0 (
    color 0C
    echo.
    echo ===============================================================================
    echo     [LỖI BIÊN DỊCH] Quá trình ghép gặp lỗi cú pháp giữa các module!
    echo     Vui lòng kiểm tra lại thông báo lỗi màu đỏ ở phía trên.
    echo ===============================================================================
    pause
    exit /b
)

color 0A
echo.
echo ===============================================================================
echo     [THÀNH CÔNG RỰC RỠ] ĐÃ GHÉP TOÀN BỘ 04 MODULE CỦA 4 BẠN VÀO DỰ ÁN (0 LỖI)!
echo     Dự án sẵn sàng 100%% để chạy thử nghiệm và bảo vệ trước Hội đồng chấm thi!
echo ===============================================================================
echo.
set /p RUN_APP="Bạn có muốn khởi chạy ngay website để trải nghiệm không? (Y/N): "
if /i "%RUN_APP%"=="Y" (
    echo Đang khởi chạy hệ thống tại http://localhost:5000 ...
    start http://localhost:5000
    dotnet run --urls "http://localhost:5000"
)
pause
