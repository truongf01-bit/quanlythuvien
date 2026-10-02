# QUY TRÌNH PHÂN CHIA CODE & GHÉP CODE (04 THÀNH VIÊN)
**Đồ án Môn học:** Thực hành Lập trình .NET (ASP.NET Core 10 MVC + SQL Server 2022)  
**Đề tài 02:** Hệ thống Quản lý Thư viện & Mượn trả sách trực tuyến (Trường Đại học Kinh tế - Kỹ thuật Công nghiệp - UNETI)  
**Nhóm thực hiện:**
1. **Trương Trọng Cường** (Trưởng nhóm) - 25% Đóng góp
2. **Nguyễn Xuân Trường** (Thành viên) - 25% Đóng góp
3. **Nguyễn Viết Hồng** (Thành viên) - 25% Đóng góp
4. **Đặng Xuân Tình** (Thành viên) - 25% Đóng góp

---

## I. CẤU TRÚC THƯ MỤC PHÂN CHIA CÔNG VIỆC
Dự án đã được bóc tách thành 4 thư mục module độc lập trong thư mục `MODULES_4_THANH_VIEN/`:

```
MODULES_4_THANH_VIEN/
│
├── ThanhVien_1_TruongTrongCuong/        <-- MODULE 1: Tài khoản, Đăng nhập, Thể loại, CSDL SSMS
│   ├── Controllers/ (TaiKhoanController.cs, TheLoaiController.cs)
│   ├── Models/ (TaiKhoan.cs, TheLoai.cs)
│   ├── ViewModels/ (DangKyViewModel.cs)
│   ├── Views/ (TaiKhoan, TheLoai)
│   ├── Database/ (CSDL_QuanLyThuVien_SSMS.sql, TAI_KHOAN_DANG_NHAP.txt)
│   ├── GHEP_CODE_MODULE_1.bat           <-- Ghép riêng Module 1 vào dự án & chạy test
│   └── HUONG_DAN_MODULE_1_TRUONG_TRONG_CUONG.md
│
├── ThanhVien_2_NguyenXuanTruong/        <-- MODULE 2: Sách, Tìm kiếm, Lọc, Phân trang LINQ, Giao diện
│   ├── Controllers/ (SachController.cs, HomeController.cs)
│   ├── Models/ (Sach.cs)
│   ├── ViewModels/ (SachIndexViewModel.cs)
│   ├── Views/ (Sach, Home)
│   ├── Shared_Layout/ (_Layout.cshtml)
│   ├── wwwroot_css/ (site.css)
│   ├── GHEP_CODE_MODULE_2.bat           <-- Ghép riêng Module 2 vào dự án & chạy test
│   └── HUONG_DAN_MODULE_2_NGUYEN_XUAN_TRUONG.md
│
├── ThanhVien_3_NguyenVietHong/          <-- MODULE 3: Độc giả, Khóa thẻ, Đăng ký mượn online
│   ├── Controllers/ (DocGiaController.cs, MuonSachController.cs)
│   ├── Models/ (DocGia.cs, ChiTietPhieuMuon.cs)
│   ├── ViewModels/ (DangKyMuonViewModel.cs)
│   ├── Views/ (DocGia, MuonSach)
│   ├── GHEP_CODE_MODULE_3.bat           <-- Ghép riêng Module 3 vào dự án & chạy test
│   └── HUONG_DAN_MODULE_3_NGUYEN_VIET_HONG.md
│
├── ThanhVien_4_DangXuanTinh/            <-- MODULE 4: Phiếu mượn/trả, Quá hạn, QR MB Bank/MoMo, Thống kê
│   ├── Controllers/ (PhieuMuonController.cs, ThongKeController.cs, ThanhToanController.cs)
│   ├── Models/ (PhieuMuon.cs)
│   ├── ViewModels/ (DashboardViewModel.cs, ThongKeViewModel.cs, ThanhToanViewModel.cs)
│   ├── Views/ (PhieuMuon, ThongKe, ThanhToan)
│   ├── wwwroot_images_payments/ (qr-mbbank.png, qr-momo.png)
│   ├── GHEP_CODE_MODULE_4.bat           <-- Ghép riêng Module 4 vào dự án & chạy test
│   └── HUONG_DAN_MODULE_4_DANG_XUAN_TINH.md
│
├── GHEP_TAT_CA_MODULE.bat               <-- [CHÍNH] Ghép toàn bộ cả 4 module cùng lúc và chạy web
└── QUY_TRINH_PHAN_CHIA_VA_GHEP_CODE.md
```

---

## II. QUY TRÌNH LÀM VIỆC CỦA NHÓM (CÁCH CODE & GHÉP CODE)

### Cách 1: Làm việc qua USB / Zalo / Google Drive (Rất đơn giản, không cần Git)
1. **Phát thư mục cho từng người**:
   * Trưởng nhóm nén hoặc gửi thư mục của từng bạn cho bạn đó (Ví dụ: gửi thư mục `ThanhVien_2_NguyenXuanTruong` cho bạn Trường).
2. **Từng thành viên code độc lập**:
   * Mỗi bạn mở Visual Studio hoặc VS Code trỏ vào thư mục của mình để viết code, thêm giao diện, chỉnh sửa nghiệp vụ theo hướng dẫn trong file `HUONG_DAN_MODULE_x_...md`.
3. **Thành viên tự test phần của mình**:
   * Để test, bạn chỉ cần giữ nguyên cả project `quanlythuvien` và nháy đúp file `GHEP_CODE_MODULE_x.bat`. Code sẽ tự đẩy vào dự án và web sẽ bật lên tại `http://localhost:5000` để bạn duyệt chức năng.
4. **Trưởng nhóm ghép tổng hợp cuối cùng**:
   * Sau khi cả 4 bạn gửi lại code, Trưởng nhóm chỉ việc copy đè 4 thư mục vào `MODULES_4_THANH_VIEN/` rồi nháy đúp chuột vào file:
     👉 **`GHEP_TAT_CA_MODULE.bat`**
   * Script sẽ tự động ghép cả 4 phần, biên dịch sạch 0 lỗi và khởi chạy toàn bộ hệ thống!

---

### Cách 2: Làm việc qua Git / GitHub (Chuẩn Công nghệ Phần mềm)
1. **Tạo 4 nhánh (Branch) trên Git**:
   * `git checkout -b feature/module-1-cuong`
   * `git checkout -b feature/module-2-truong`
   * `git checkout -b feature/module-3-hong`
   * `git checkout -b feature/module-4-tinh`
2. **Không xung đột (Zero Merge Conflict)**:
   * Do mỗi thành viên phụ trách các Controller, Model và View riêng biệt hoàn toàn (không ai sửa chung Controller của ai), nên khi tạo Pull Request để gộp về nhánh `main` sẽ **100% tự động gộp thành công, không bao giờ bị conflict**!

---

## III. NGUYÊN TẮC VÀNG ĐỂ GHÉP CODE KHÔNG BAO GIỜ LỖI
1. **Giữ nguyên Namespace**:
   * Mọi Controller phải có: `namespace QuanLyThuVien.Controllers;`
   * Mọi Model phải có: `namespace QuanLyThuVien.Models;`
   * Mọi ViewModel phải có: `namespace QuanLyThuVien.ViewModels;`
2. **Không đổi tên các khóa ngoại kết nối giữa các bảng**:
   * Ví dụ: `Sach.MaTheLoai` liên kết `TheLoai.MaTheLoai`.
   * `DocGia.MaTaiKhoan` liên kết `TaiKhoan.MaTaiKhoan`.
   * `ChiTietPhieuMuon` liên kết `PhieuMuon.MaPhieuMuon` và `Sach.MaSach`.
3. **Không tự ý sửa `Data/ThuVienDbContext.cs`** mà nên thống nhất qua Trưởng nhóm (Trương Trọng Cường).
