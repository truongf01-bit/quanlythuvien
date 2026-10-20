# TÀI LIỆU HƯỚNG DẪN BÁO CÁO, THUYẾT TRÌNH & BẢO VỆ ĐỒ ÁN
## ĐỀ TÀI 02: HỆ THỐNG QUẢN LÝ THƯ VIỆN VÀ MƯỢN TRẢ SÁCH (ASP.NET CORE 10 MVC)

---

## 1. HƯỚNG DẪN CHẠY CHƯƠNG TRÌNH NHANH

Mở Terminal / PowerShell tại thư mục `quanlythuvien` và chạy lệnh:
```powershell
dotnet run
```
Sau đó mở trình duyệt truy cập địa chỉ hiển thị trên màn hình (ví dụ `http://localhost:5000` hoặc `https://localhost:7xxx`).
> **Lưu ý:** Khi ứng dụng khởi chạy lần đầu, hàm `DbInitializer.Initialize(context)` trong `Program.cs` sẽ **tự động chạy Migration tạo Cơ sở dữ liệu SQL Server (`QuanLyThuVienDb`) và tự động nạp đầy đủ Dữ liệu mẫu** (06 thể loại, 22 đầu sách, 02 Admin, 15 Độc giả, 18 Phiếu mượn kèm chi tiết).

---

## 2. DANH SÁCH TÀI KHOẢN MẪU ĐỂ DEMO KHI THUYẾT TRÌNH

| Vai trò | Tên đăng nhập | Mật khẩu | Trạng thái | Mục đích kiểm thử khi bảo vệ |
| :--- | :--- | :--- | :--- | :--- |
| **Admin chính** | `admin` | `123456` | Hoạt động | Quản lý Thể loại, Sách, Độc giả, Duyệt mượn/trả, Quá hạn, Dashboard, Thống kê |
| **Admin phụ** | `admin2` | `123456` | Hoạt động | Tài khoản thủ thư quản trị thứ 2 |
| **Độc giả 1** | `docgia1` | `123456` | Hoạt động | Xem sách, Đăng ký mượn, có sẵn cả phiếu đang mượn quá hạn & lịch sử trả |
| **Độc giả 2..14** | `docgia2` .. `docgia14` | `123456` | Hoạt động | Kiểm thử đăng ký mượn mới, xem lịch sử cá nhân |
| **Độc giả 15** | `docgia15` | `123456` | **Bị khóa** | Kiểm thử trường hợp đăng nhập tài khoản bị khóa (Mục 21.1) |

---

## 3. BẢNG PHÂN CÔNG 04 THÀNH VIÊN & CÁC FILE PHỤ TRÁCH (MỤC 4 & MỤC 17)

> 💡 **Mẹo thay tên thành viên thật của nhóm:** Mở VS Code / Visual Studio, bấm `Ctrl + Shift + H` (Find & Replace in Files) để thay tên và mã sinh viên mẫu dưới đây bằng tên thật của 4 bạn trong nhóm:

| STT | Họ và Tên | Vai trò | Module phụ trách | Các file mã nguồn phụ trách chính |
| :---: | :--- | :--- | :--- | :--- |
| **1** | **Trương Trọng Cường** | **Trưởng nhóm** | **Module 1:** Tài khoản, Đăng nhập/Đăng ký, Phân quyền Session & Quản lý Thể loại | `Models/TaiKhoan.cs`, `Models/TheLoai.cs`, `Controllers/TaiKhoanController.cs`, `Controllers/TheLoaiController.cs`, `Views/TaiKhoan/*`, `Views/TheLoai/*` |
| **2** | **Nguyễn Xuân Trường** | **Thành viên** | **Module 2:** Quản lý Sách (CRUD), Tìm kiếm, Lọc, Sắp xếp & Phân trang LINQ | `Models/Sach.cs`, `ViewModels/SachIndexViewModel.cs`, `Controllers/SachController.cs`, `Controllers/HomeController.cs`, `Views/Sach/*`, `Views/Home/*` |
| **3** | **Nguyễn Viết Hồng** | **Thành viên** | **Module 3:** Quản lý Độc giả, Thông tin cá nhân, Đăng ký mượn & Chi tiết phiếu mượn | `Models/DocGia.cs`, `Models/ChiTietPhieuMuon.cs`, `ViewModels/DangKyMuonViewModel.cs`, `Controllers/DocGiaController.cs`, `Controllers/MuonSachController.cs`, `Views/DocGia/*`, `Views/MuonSach/*` |
| **4** | **Đặng Xuân Tình** | **Thành viên** | **Module 4:** Quản lý Mượn trả, Xác nhận mượn/trả, Quá hạn, Dashboard & Thống kê LINQ | `Models/PhieuMuon.cs`, `ViewModels/DashboardViewModel.cs`, `ViewModels/ThongKeViewModel.cs`, `Controllers/PhieuMuonController.cs`, `Controllers/ThongKeController.cs`, `Views/PhieuMuon/*`, `Views/ThongKe/*` |

*(Các file dùng chung gồm `Data/ThuVienDbContext.cs`, `Data/DbInitializer.cs`, `Program.cs`, `Views/Shared/_Layout.cshtml` đã ghi đầy đủ cả 4 sinh viên ở đầu file theo đúng Mục 17).*

---

## 4. THIẾT KẾ CƠ SỞ DỮ LIỆU & QUAN HỆ ENTITY (MỤC 9)

Hệ thống gồm **06 Entity** xây dựng theo hướng **Code First** với **Entity Framework Core 10**:

1. **`TaiKhoan` (1) — (1) `DocGia`**:
   - Khóa chính: `MaTaiKhoan`
   - Khóa ngoại duy nhất (Unique Foreign Key): `DocGia.MaTaiKhoan` liên kết 1–1 với `TaiKhoan`.
2. **`TheLoai` (1) — (n) `Sach`**:
   - Một thể loại có nhiều đầu sách (`ICollection<Sach> Sachs`).
   - Bảng `Sach` chứa khóa ngoại `MaTheLoai`.
3. **`DocGia` (1) — (n) `PhieuMuon`**:
   - Một độc giả có thể tạo nhiều phiếu mượn (`ICollection<PhieuMuon> PhieuMuons`).
   - Bảng `PhieuMuon` chứa khóa ngoại `MaDocGia`.
4. **`PhieuMuon` (1) — (n) `ChiTietPhieuMuon` và `Sach` (1) — (n) `ChiTietPhieuMuon`**:
   - Biểu diễn quan hệ nhiều – nhiều (`n - n`) giữa `PhieuMuon` và `Sach`.
   - Bảng `ChiTietPhieuMuon` sử dụng **Khóa chính phức hợp (Composite Primary Key)** gồm `(MaPhieuMuon, MaSach)`, lưu `SoLuongMuon`, `NgayTra` và `TinhTrangTra`.

---

## 5. CÂU HỎI & CÁCH GIẢI THÍCH CODE KHI BẢO VỆ CHO TỪNG THÀNH VIÊN

### 👤 Sinh viên 1 (Module 1: Đăng nhập, Session, Phân quyền & Thể loại)
1. **Giải thích cơ chế Đăng nhập và lưu Session (`TaiKhoanController.cs`):**
   - Dùng LINQ `.FirstOrDefault(tk => tk.TenDangNhap == tenDangNhap && tk.MatKhau == matKhau)` để kiểm tra tài khoản.
   - Kiểm tra `taiKhoan.TrangThai != "Hoạt động"` để chặn tài khoản bị khóa.
   - Lưu `MaTaiKhoan`, `HoTen`, `VaiTro` vào `HttpContext.Session.SetInt32(...)` và `SetString(...)`.
2. **Giải thích Phân quyền tại Controller:**
   - Tạo hàm `LaAdmin()` kiểm tra `HttpContext.Session.GetString("VaiTro") == "Admin"`.
   - Ở đầu mỗi Action của Admin đều gọi `if (!LaAdmin()) return RedirectToAction("KhongCoQuyen", "TaiKhoan");` — đảm bảo Độc giả gõ trực tiếp URL Admin cũng bị chặn ở phía Server.
3. **Giải thích kiểm tra trùng tên và ràng buộc xóa Thể loại (`TheLoaiController.cs`):**
   - Kiểm tra trùng tên: `_context.TheLoais.Any(tl => tl.TenTheLoai.ToLower() == ten.ToLower())`.
   - Chặn xóa khi có sách: `if (theLoai.Sachs.Any())` thì báo lỗi không cho xóa để bảo toàn khóa ngoại.

### 👤 Sinh viên 2 (Module 2: Quản lý Sách, Tìm kiếm, Lọc, Sắp xếp, Phân trang)
1. **Giải thích quy trình 5 bước nối chuỗi `IQueryable<Sach>` trong `SachController.Index`:**
   - Nhờ dùng `IQueryable<Sach>`, các lệnh `.Where(...)`, `.OrderBy(...)`, `.Skip(...)`, `.Take(...)` được cộng dồn lại và chỉ thực thi **1 câu lệnh SQL duy nhất** xuống SQL Server khi gọi `.ToList()`.
   - Thứ tự đúng chuẩn đề bài:
     1. Truy vấn gốc kèm `.Include(s => s.TheLoai)`
     2. Tìm kiếm theo `TenSach` hoặc `TacGia`
     3. Lọc theo `MaTheLoai`, `NhaXuatBan`, `TrangThai`, `TuNam`, `DenNam`
     4. Sắp xếp bằng `switch (sapXep)` (`OrderBy` / `OrderByDescending`)
     5. Phân trang bằng `.Skip((trang - 1) * kichThuocTrang).Take(kichThuocTrang).ToList()`
2. **Giải thích cách giữ điều kiện khi chuyển trang trên `Views/Sach/Index.cshtml`:**
   - Trên các thẻ `<a class="page-link">` của thanh phân trang, sử dụng Tag Helper `asp-route-tuKhoa`, `asp-route-maTheLoai`, `asp-route-nhaXuatBan`, `asp-route-trangThai`, `asp-route-tuNam`, `asp-route-denNam`, `asp-route-sapXep`, `asp-route-trang` để truyền lại toàn bộ tham số lọc hiện tại.

### 👤 Sinh viên 3 (Module 3: Độc giả, Thông tin cá nhân & Đăng ký mượn sách)
1. **Giải thích bảo mật Phân quyền dữ liệu cá nhân (`DocGiaController` & `MuonSachController`):**
   - Luôn lấy `MaTaiKhoan` từ `Session` của người đang đăng nhập (`LayDocGiaHienTai()`), sau đó lọc `.Where(pm => pm.MaDocGia == docGia.MaDocGia)`. Độc giả tuyệt đối không thể xem được phiếu mượn hay sửa thông tin của độc giả khác.
2. **Giải thích các điều kiện kiểm tra khi Đăng ký mượn (`MuonSachController.DangKyMuon`):**
   - Kiểm tra `HanTra > NgayMuon`.
   - Kiểm tra sách tồn tại và `sach.TrangThai == "Đang phục vụ"`.
   - Kiểm tra `SoLuongMuon > 0` và `SoLuongMuon <= sach.SoLuongCon`.
   - Kiểm tra không tạo yêu cầu trùng: dùng `.Any(...)` kiểm tra xem độc giả đã có phiếu `"Chờ xác nhận"` hoặc `"Đang mượn"` cho cuốn sách đó chưa.

### 👤 Sinh viên 4 (Module 4: Xác nhận mượn/trả, Quá hạn, Dashboard & Thống kê LINQ)
1. **Giải thích nghiệp vụ Xác nhận mượn (`PhieuMuonController.XacNhanMuon`):**
   - Kiểm tra lại `ct.Sach.SoLuongCon >= ct.SoLuongMuon` tại thời điểm Admin bấm duyệt.
   - Trừ số lượng tồn: `ct.Sach.SoLuongCon -= ct.SoLuongMuon;` và đổi `phieu.TrangThai = "Đang mượn"`.
2. **Giải thích nghiệp vụ Trả sách (`PhieuMuonController.TraSach`):**
   - Ghi nhận `ct.NgayTra = ngayTraThucTe`, cập nhật `ct.TinhTrangTra`.
   - Cộng lại `ct.Sach.SoLuongCon += ct.SoLuongMuon` (nếu không mất sách) và kiểm tra chặn trần `if (ct.Sach.SoLuongCon > ct.Sach.SoLuong) ct.Sach.SoLuongCon = ct.Sach.SoLuong;`.
3. **Giải thích truy vấn Quá hạn & Thống kê LINQ (`ThongKeController.cs`):**
   - Quá hạn: `.Where(pm => pm.TrangThai == "Đang mượn" && pm.HanTra.Date < DateTime.Today)`.
   - Thống kê sử dụng `GroupBy()`, `Count()`, `Sum()`, `OrderByDescending()` rõ ràng cho cả 6 bảng thống kê.

---

## 6. MẪU LỊCH SỬ COMMIT GITHUB CHO 04 THÀNH VIÊN (MỤC 20)

Khi đẩy code lên GitHub nhóm, các thành viên sử dụng mẫu Commit chuẩn sau:
- `[22103100001] [TaiKhoan-TheLoai] Thiet ke Entity TaiKhoan, TheLoai va chuc nang Dang nhap Session`
- `[22103100001] [TheLoai] Xay dung CRUD Quan ly The loai va kiem tra rang buoc xoa`
- `[22103100002] [Sach] Thiet ke Entity Sach va xay dung CRUD Quan ly Sach`
- `[22103100002] [Sach] Them chuc nang Tim kiem, Loc, Sap xep va Phan trang LINQ`
- `[22103100003] [DocGia] Thiet ke Entity DocGia, ChiTietPhieuMuon va Quan ly Doc gia`
- `[22103100003] [MuonSach] Xay dung chuc nang Dang ky muon sach va Lich su muon tra`
- `[22103100004] [PhieuMuon] Thiet ke Entity PhieuMuon, Xac nhan muon tra va Theo doi qua han`
- `[22103100004] [ThongKe] Xay dung Dashboard va 6 bang Bao cao Thong ke LINQ`
