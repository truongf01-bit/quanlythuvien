# HƯỚNG DẪN CODE VÀ GHÉP CODE - MODULE 1
**Người phụ trách:** Trương Trọng Cường (Trưởng nhóm - 25% Đóng góp)  
**Chuyên đề:** Xác thực hệ thống, Phân quyền RBAC (`User` vs `Admin`), Quản lý Tài khoản & Thể loại sách, Thiết kế CSDL SSMS 22.

---

### 1. Cấu trúc thư mục code của bạn:
```
ThanhVien_1_TruongTrongCuong/
│
├── Controllers/
│   ├── TaiKhoanController.cs       <-- Xử lý Đăng ký, Đăng nhập, Đăng xuất, Phân quyền
│   └── TheLoaiController.cs        <-- CRUD Thể loại sách
│
├── Models/
│   ├── TaiKhoan.cs                 <-- Thực thể Tài khoản (MaTaiKhoan, TenDangNhap, MatKhau, VaiTro, TrangThai)
│   └── TheLoai.cs                  <-- Thực thể Thể loại sách (MaTheLoai, TenTheLoai, MoTa)
│
├── ViewModels/
│   └── DangKyViewModel.cs          <-- DTO xác thực form Đăng ký dành riêng cho độc giả
│
├── Views/
│   ├── TaiKhoan/
│   │   ├── DangNhap.cshtml         <-- Giao diện đăng nhập hiện đại, tiện ích thành viên
│   │   ├── DangKy.cshtml           <-- Form đăng ký sinh viên mới
│   │   ├── Index.cshtml            <-- Quản lý danh sách tài khoản (Dành cho Admin)
│   │   └── KhongCoQuyen.cshtml     <-- Trang thông báo chặn truy cập trái phép 403
│   └── TheLoai/
│       ├── Index.cshtml            <-- Danh sách thể loại
│       ├── Create.cshtml           <-- Thêm thể loại mới
│       ├── Edit.cshtml             <-- Chỉnh sửa thể loại
│       ├── Details.cshtml          <-- Chi tiết thể loại
│       └── Delete.cshtml           <-- Xóa thể loại
│
├── Database/
│   ├── CSDL_QuanLyThuVien_SSMS.sql <-- File SQL khởi tạo CSDL chuẩn cho SSMS 22
│   └── TAI_KHOAN_DANG_NHAP.txt     <-- Danh sách tài khoản mẫu (Admin & Sinh viên)
│
├── GHEP_CODE_MODULE_1.bat          <-- Script 1-click tự động ghép code vào dự án & chạy test
└── HUONG_DAN_MODULE_1_TRUONG_TRONG_CUONG.md
```

---

### 2. Các nghiệp vụ trọng tâm bạn cần code & kiểm thử:
1. **Chức năng Đăng ký tài khoản (`/TaiKhoan/DangKy`)**:
   * Kiểm tra trùng lặp `TenDangNhap`, `Email`, `SoDienThoai` bằng LINQ.
   * Khi đăng ký thành công: Tự động gán cứng `VaiTro = "DocGia"` và tạo bản ghi tương ứng trong bảng `DocGias`.
2. **Chức năng Đăng nhập (`/TaiKhoan/DangNhap`)**:
   * Kiểm tra mật khẩu và trạng thái hoạt động (`TrangThai == "Hoạt động"`).
   * Lưu thông tin vào Session: `MaTaiKhoan`, `TenDangNhap`, `HoTen`, `VaiTro`.
   * Phân luồng điều hướng: Admin chuyển sang `/ThongKe`, Sinh viên chuyển sang `/Sach`.
3. **Phân quyền truy cập RBAC**:
   * Kiểm tra phương thức `LaAdmin()`. Nếu độc giả truy cập trái phép trang quản trị thì Redirect sang `/TaiKhoan/KhongCoQuyen`.
4. **Quản lý Thể loại sách (`/TheLoai`)**:
   * Đầy đủ chức năng Thêm/Sửa/Xóa/Xem thể loại và bắt lỗi dữ liệu đầu vào.

---

### 3. Cách ghép code vào dự án:
* Sau khi bạn chỉnh sửa bất kỳ file nào trong thư mục của mình:
* Chỉ cần **nháy đúp chuột vào file `GHEP_CODE_MODULE_1.bat`**.
* File bat sẽ tự động đồng bộ mã nguồn sang thư mục `quanlythuvien`, kiểm tra lỗi biên dịch `dotnet build` và khởi chạy web để bạn kiểm tra kết quả ngay lập tức!
