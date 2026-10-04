# HƯỚNG DẪN CODE VÀ GHÉP CODE - MODULE 4
**Người phụ trách:** Đặng Xuân Tình (Thành viên - 25% Đóng góp)  
**Chuyên đề:** Quy trình Lưu thông Mượn/Trả, Xử lý Quá hạn, Tích hợp Thanh toán QR Code MB Bank & MoMo, Báo cáo Thống kê LINQ.

---

### 1. Cấu trúc thư mục code của bạn:
```
ThanhVien_4_DangXuanTinh/
│
├── Controllers/
│   ├── PhieuMuonController.cs      <-- Duyệt mượn, Xác nhận trả sách, Quản lý phiếu
│   ├── ThongKeController.cs        <-- Dashboard tổng quan và Báo cáo chuyên sâu
│   └── ThanhToanController.cs      <-- Thanh toán phí phạt trễ hạn, tiền cọc qua VietQR MB Bank & MoMo
│
├── Models/
│   └── PhieuMuon.cs                <-- Thực thể Phiếu mượn (MaPhieuMuon, NgayMuon, HanTra, NgayTraThucTe, TrangThai, TienCoc, TienPhat)
│
├── ViewModels/
│   ├── DashboardViewModel.cs       <-- DTO số liệu thẻ thống kê nhanh trên Admin Dashboard
│   ├── ThongKeViewModel.cs         <-- DTO biểu đồ báo cáo: Top 5 sách mượn nhiều nhất, Tỷ lệ theo thể loại
│   └── ThanhToanViewModel.cs       <-- DTO thông tin thanh toán QR Code (Số tài khoản, Số tiền, Mã nội dung CK)
│
├── Views/
│   ├── PhieuMuon/
│   │   ├── Index.cshtml            <-- Danh sách phiếu mượn (lọc Chờ duyệt, Đang mượn, Đã trả)
│   │   ├── Details.cshtml          <-- Chi tiết phiếu mượn và danh sách sách mượn
│   │   ├── TraSach.cshtml          <-- Form xác nhận nhận lại sách từ sinh viên, kiểm tra hư hại & phạt
│   │   └── QuaHan.cshtml           <-- Danh sách các phiếu quá hạn cần gửi cảnh báo / thu phạt
│   ├── ThongKe/
│   │   └── Index.cshtml            <-- Báo cáo thống kê trực quan với bảng biểu và thanh tỷ lệ
│   └── ThanhToan/
│       ├── Index.cshtml            <-- Màn hình thanh toán trực tiếp qua QR MB Bank (21611611116) hoặc Ví MoMo
│       └── ThanhCong.cshtml        <-- Biên lai điện tử xác nhận thanh toán hoàn tất
│
├── wwwroot_images_payments/
│   ├── qr-mbbank.png               <-- Mã QR MB Bank chuẩn VietQR Napas 24/7
│   └── qr-momo.png                 <-- Mã QR Ví điện tử MoMo
│
├── GHEP_CODE_MODULE_4.bat          <-- Script 1-click tự động ghép code vào dự án & chạy test
└── HUONG_DAN_MODULE_4_DANG_XUAN_TINH.md
```

---

### 2. Các nghiệp vụ trọng tâm bạn cần code & kiểm thử:
1. **Quy trình Duyệt mượn & Trả sách hoàn kho**:
   * **Khi thủ thư bấm Duyệt mượn**: Chuyển `TrangThai = "Đang mượn"`, tự động trừ số lượng sách còn trong kho (`SoLuongCon -= chiTiet.SoLuongMuon`).
   * **Khi thủ thư bấm Xác nhận trả sách**: Ghi nhận `NgayTraThucTe = DateTime.Now`, chuyển `TrangThai = "Đã trả"`, tự động hoàn trả số lượng sách vào kho (`SoLuongCon += chiTiet.SoLuongMuon`).
2. **Xử lý Quá hạn & Phạt trễ hạn**:
   * Lọc các phiếu có `TrangThai == "Đang mượn"` và `HanTra < DateTime.Now`.
   * Tự động tính tiền phạt trễ hạn: `(Ngày hiện tại - Hạn trả) * 5.000 VNĐ/ngày`.
3. **Cổng thanh toán QR Code MB Bank & MoMo (`/ThanhToan`)**:
   * Tích hợp thanh toán quét mã QR MB Bank (STK: `21611611116`) và Ví MoMo (Chủ tài khoản: `NGUYEN XUAN TRUONG`).
   * Sinh mã nội dung chuyển khoản tự động kèm mã phiếu mượn và số tiền.
   * Cấp biên lai xác nhận giao dịch điện tử (`ThanhCong.cshtml`).
4. **Báo cáo Thống kê LINQ**:
   * Tính toán Top 5 sách mượn nhiều nhất bằng `GroupBy` và `Sum`.
   * Thống kê tỷ lệ mượn theo thể loại sách.

---

### 3. Cách ghép code vào dự án:
* Sau khi chỉnh sửa code trong thư mục của mình:
* Chỉ cần **nháy đúp chuột vào file `GHEP_CODE_MODULE_4.bat`**.
* File bat sẽ tự động sao chép Controllers, Views, hình ảnh sang `quanlythuvien`, biên dịch dự án và chạy ứng dụng để bạn kiểm tra!
