# HƯỚNG DẪN CODE VÀ GHÉP CODE - MODULE 2
**Người phụ trách:** Nguyễn Xuân Trường (Thành viên - 25% Đóng góp)  
**Chuyên đề:** Quản trị kho sách, Thuật toán Tìm kiếm, Lọc đa tiêu chí, Sắp xếp và Phân trang LINQ, Giao diện Trang chủ & Layout.

---

### 1. Cấu trúc thư mục code của bạn:
```
ThanhVien_2_NguyenXuanTruong/
│
├── Controllers/
│   ├── SachController.cs           <-- Tìm kiếm, Lọc, Phân trang, CRUD đầu sách
│   └── HomeController.cs           <-- Trang chủ, Banner, Danh mục nổi bật
│
├── Models/
│   └── Sach.cs                     <-- Thực thể Sách (MaSach, TenSach, TacGia, SoLuong, SoLuongCon, GiaTien...)
│
├── ViewModels/
│   └── SachIndexViewModel.cs       <-- DTO đóng gói tham số tìm kiếm, lọc, danh sách sách & dữ liệu phân trang
│
├── Views/
│   ├── Sach/
│   │   ├── Index.cshtml            <-- Giao diện tra cứu sách hiện đại (Lưới thẻ sách, Bộ lọc bên trái, Phân trang)
│   │   ├── Create.cshtml           <-- Thêm mới đầu sách vào kho
│   │   ├── Edit.cshtml             <-- Chỉnh sửa thông tin sách
│   │   ├── Details.cshtml          <-- Chi tiết sách, tình trạng còn/hết, nút mượn ngay
│   │   └── Delete.cshtml           <-- Xác nhận xóa sách
│   └── Home/
│       ├── Index.cshtml            <-- Trang chủ thư viện UNETI (Hero Banner, Thống kê nhanh, Sách mới)
│       └── Privacy.cshtml          <-- Chính sách mượn sách & Bản quyền đồ án
│
├── Shared_Layout/
│   └── _Layout.cshtml              <-- Khung giao diện dùng chung (Navbar, Footer ghi danh 4 thành viên)
├── wwwroot_css/
│   └── site.css                    <-- Tùy biến màu sắc, hiệu ứng card sách, nút bấm hiện đại
│
├── GHEP_CODE_MODULE_2.bat          <-- Script 1-click tự động ghép code vào dự án & chạy test
└── HUONG_DAN_MODULE_2_NGUYEN_XUAN_TRUONG.md
```

---

### 2. Các nghiệp vụ trọng tâm bạn cần code & kiểm thử:
1. **Thuật toán Tìm kiếm mờ (Fuzzy Search)**:
   * Tìm kiếm theo từ khóa trong `TenSach` hoặc `TacGia` không phân biệt hoa thường bằng LINQ:
     `s.TenSach.ToLower().Contains(kw) || s.TacGia.ToLower().Contains(kw)`
2. **Bộ lọc đa tiêu chí (Dynamic Filter Query)**:
   * Lọc kết hợp theo Thể loại (`maTheLoai`), Năm xuất bản, Trạng thái (Còn sách / Hết sách).
   * Giữ câu truy vấn dưới dạng `IQueryable<Sach>` và chỉ tải dữ liệu ở cuối bằng `.ToList()` để tối ưu hiệu năng SQL.
3. **Thuật toán Phân trang chuyên nghiệp (Pagination)**:
   * Sử dụng `.Skip((trang - 1) * kichThuocTrang).Take(kichThuocTrang)` với kích thước 6 hoặc 8 cuốn sách/trang.
   * Tính toán tổng số trang: `Math.Ceiling((double)tongSoBanGhi / kichThuocTrang)`.
4. **CRUD Đầu sách**:
   * Kiểm tra tính toàn vẹn: Không cho phép nhập `SoLuong < 0`, `GiaTien < 0`, tự động khởi tạo `SoLuongCon = SoLuong`.

---

### 3. Cách ghép code vào dự án:
* Sau khi chỉnh sửa code trong thư mục của mình:
* Chỉ cần **nháy đúp chuột vào file `GHEP_CODE_MODULE_2.bat`**.
* File bat sẽ tự động sao chép các Controller, View, CSS sang `quanlythuvien`, gọi lệnh `dotnet build` và mở trình duyệt để bạn kiểm thử ngay!
