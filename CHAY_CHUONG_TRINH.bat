@echo off
chcp 65001 >nul
title HỆ THỐNG QUẢN LÝ THƯ VIỆN - ASP.NET CORE 10 MVC
color 0B
cls
echo ===============================================================================
echo     📚 HỆ THỐNG QUẢN LÝ THƯ VIỆN & MƯỢN TRẢ SÁCH (ASP.NET CORE 10 MVC)
echo ===============================================================================
echo.
echo  [*] Đang khởi động hệ thống Web ASP.NET Core 10...
echo  [*] Trình duyệt web sẽ tự động mở trang chủ tại http://localhost:5000 sau 3 giây.
echo.
echo  -----------------------------------------------------------------------------
echo   TÀI KHOẢN MẪU ĐỂ ĐĂNG NHẬP KIỂM THỬ:
echo   - Admin (Quản trị viên) : admin     / mật khẩu: 123456
echo   - Độc giả (Mượn sách)   : docgia1   / mật khẩu: 123456
echo   - Độc giả (Bị khóa)     : docgia15  / mật khẩu: 123456
echo  -----------------------------------------------------------------------------
echo.
echo  [*] Bấm Ctrl + C hoặc đóng cửa sổ này khi muốn dừng chương trình.
echo.

:: Tự động mở trình duyệt sau 3 giây
start "" cmd /c "timeout /t 3 /nobreak >nul && start http://localhost:5000"

:: Khởi chạy ứng dụng Web trên cổng 5000
dotnet run --urls "http://localhost:5000"

pause
