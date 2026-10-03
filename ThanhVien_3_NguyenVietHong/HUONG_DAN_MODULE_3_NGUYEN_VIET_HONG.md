# HƯỚNG DẪN CODE VÀ GHÉP CODE - MODULE 3
**Người phụ trách:** Nguyễn Viết Hồng (Thành viên - 25% Đóng góp)  
**Chuyên đề:** Quản lý Độc giả, Hồ sơ cá nhân sinh viên, Dịch vụ Đăng ký mượn sách trực tuyến & Chi tiết phiếu mượn.

---

### 1. Cấu trúc thư mục code của bạn:
```
ThanhVien_3_NguyenVietHong/
│
├── Controllers/
│   ├── DocGiaController.cs         <-- Quản lý độc giả (CRUD, Khóa thẻ, Thông tin cá nhân)
│   └── MuonSachController.cs       <-- Đăng ký mượn online, Sách đang mượn, Lịch sử mượn
│
├── Models/
│   ├── DocGia.cs                   <-- Thực thể Độc giả (MaDocGia, HoTen, Email, SoDienThoai, TrangThai, MaTaiKhoan)
│   └── ChiTietPhieuMuon.cs         <-- Bảng liên kết n-n (MaPhieuMuon, MaSach, SoLuongMuon)
│
├── ViewModels/
│   └── DangKyMuonViewModel.cs      <-- DTO form đăng ký mượn (chọn độc giả, danh sách sách, ngày mượn, ghi chú)
│
├── Views/
│   ├── DocGia/
│   │   ├── Index.cshtml            <-- Danh sách độc giả (Admin xem, lọc trạng thái, khóa thẻ)
│   │   ├── Details.cshtml          <-- Xem hồ sơ độc giả và lịch sử các lần mượn
│   │   ├── Edit.cshtml             <-- Cập nhật thông tin độc giả
│   │   └── ThongTinCaNhan.cshtml   <-- Trang hồ sơ cá nhân để sinh viên tự đổi SĐT, Email
│   └── MuonSach/
│       ├── DangKyMuon.cshtml       <-- Form giỏ đăng ký mượn sách trực tuyến (chọn tối đa 3 cuốn)
│       ├── SachDangMuon.cshtml     <-- Danh sách các cuốn sách độc giả hiện đang cầm
│       ├── LichSuMuonTra.cshtml    <-- Lịch sử toàn bộ các phiếu mượn đã trả hoặc đang mượn
│       └── ChiTietPhieu.cshtml     <-- Xem chi tiết các cuốn sách trong 1 phiếu mượn
│
├── GHEP_CODE_MODULE_3.bat          <-- Script 1-click tự động ghép code vào dự án & chạy test
└── HUONG_DAN_MODULE_3_NGUYEN_VIET_HONG.md
```

---

### 2. Các nghiệp vụ trọng tâm bạn cần code & kiểm thử:
1. **Dịch vụ Đăng ký mượn sách Online (`/MuonSach/DangKyMuon`)**:
   * Kiểm tra điều kiện mượn: Độc giả phải có trạng thái `"Hoạt động"`. Nếu bị khóa thì từ chối và báo lỗi.
   * Ràng buộc số lượng sách mượn trong 1 phiếu: Tối thiểu 1 cuốn, tối đa không quá 3 cuốn.
   * Kiểm tra tồn kho sách: Chỉ cho phép chọn các sách có `SoLuongCon > 0`.
   * Tự động khởi tạo phiếu mượn với `TrangThai = "Chờ xác nhận"`.
2. **Quản lý Độc giả & Khóa thẻ (`/DocGia`)**:
   * Chức năng đổi trạng thái: `"Hoạt động"` <-> `"Bị khóa"`.
   * Tìm kiếm độc giả theo Tên, Email hoặc Số điện thoại.
3. **Trang cá nhân sinh viên (`/DocGia/ThongTinCaNhan`)**:
   * Độc giả sinh viên chỉ được xem và sửa thông tin của chính mình dựa vào `MaTaiKhoan` trong Session, không được can thiệp vào tài khoản khác.

---

### 3. Cách ghép code vào dự án:
* Sau khi chỉnh sửa code trong thư mục của mình:
* Chỉ cần **nháy đúp chuột vào file `GHEP_CODE_MODULE_3.bat`**.
* File bat sẽ tự động sao chép các file sang dự án chính `quanlythuvien`, biên dịch dự án và chạy ứng dụng để bạn kiểm tra!
