using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    public class PhieuMuonController : Controller
    {
        private readonly ThuVienDbContext _context;

        public PhieuMuonController(ThuVienDbContext context)
        {
            _context = context;
        }

        private bool LaAdmin()
        {
            return HttpContext.Session.GetString("VaiTro") == "Admin";
        }

        public IActionResult Index(string? tenDocGia, string? trangThai, DateTime? tuNgay, DateTime? denNgay)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            IQueryable<PhieuMuon> query = _context.PhieuMuons
                .Include(pm => pm.DocGia)
                .Include(pm => pm.ChiTietPhieuMuons)
                    .ThenInclude(ct => ct.Sach)
                .Include(pm => pm.GiaoDichThanhToans)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tenDocGia))
            {
                string kw = tenDocGia.Trim().ToLower();
                query = query.Where(pm => pm.DocGia != null && pm.DocGia.HoTen.ToLower().Contains(kw));
            }

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(pm => pm.TrangThai == trangThai);
            }

            if (tuNgay.HasValue)
            {
                query = query.Where(pm => pm.NgayMuon.Date >= tuNgay.Value.Date);
            }
            if (denNgay.HasValue)
            {
                query = query.Where(pm => pm.NgayMuon.Date <= denNgay.Value.Date);
            }

            ViewBag.TenDocGia = tenDocGia;
            ViewBag.TrangThai = trangThai;
            ViewBag.TuNgay = tuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay?.ToString("yyyy-MM-dd");

            var danhSach = query
                .OrderByDescending(pm => pm.NgayMuon)
                .ThenByDescending(pm => pm.MaPhieuMuon)
                .ToList();

            return View(danhSach);
        }

        public IActionResult Details(int? id, string? returnUrl = null)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (id == null) return NotFound();

            var phieu = _context.PhieuMuons
                .Include(pm => pm.DocGia)
                .Include(pm => pm.ChiTietPhieuMuons)
                    .ThenInclude(ct => ct.Sach)
                        .ThenInclude(s => s!.TheLoai)
                .Include(pm => pm.GiaoDichThanhToans)
                .FirstOrDefault(pm => pm.MaPhieuMuon == id);

            if (phieu == null) return NotFound();

            var gds = phieu.GiaoDichThanhToans ?? new List<GiaoDichThanhToan>();

            // Tổng hợp dòng tiền Nguồn vào & Nguồn ra của phiếu này
            decimal tongNguonVao = gds.Where(g => g.HuongDongTien == "Nguồn vào" && g.TrangThai != "Đã hủy").Sum(g => g.SoTien);
            decimal tongNguonRa = gds.Where(g => g.HuongDongTien == "Nguồn ra" && g.TrangThai != "Đã hủy").Sum(g => g.SoTien);
            decimal tienCocHienTai = tongNguonVao - tongNguonRa;

            ViewBag.TongNguonVao = tongNguonVao;
            ViewBag.TongNguonRa = tongNguonRa;
            ViewBag.TienCocHienTai = tienCocHienTai;
            ViewBag.ReturnUrl = returnUrl;

            return View(phieu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XacNhanMuon(int id, string? returnUrl = null)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            var phieu = _context.PhieuMuons
                .Include(pm => pm.ChiTietPhieuMuons)
                    .ThenInclude(ct => ct.Sach)
                .FirstOrDefault(pm => pm.MaPhieuMuon == id);

            if (phieu == null) return NotFound();

            if (phieu.TrangThai != "Chờ xác nhận")
            {
                TempData["Loi"] = "Chỉ có thể xác nhận mượn đối với phiếu đang ở trạng thái 'Chờ xác nhận'!";
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                return RedirectToAction(nameof(Details), new { id, returnUrl });
            }

            if (phieu.ChiTietPhieuMuons == null || !phieu.ChiTietPhieuMuons.Any())
            {
                TempData["Loi"] = "Phiếu mượn không có đầu sách nào để xác nhận!";
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                return RedirectToAction(nameof(Details), new { id, returnUrl });
            }

            foreach (var ct in phieu.ChiTietPhieuMuons)
            {
                var sach = _context.Sachs.FirstOrDefault(s => s.MaSach == ct.MaSach);
                if (sach == null)
                {
                    TempData["Loi"] = "Dữ liệu sách trong phiếu không hợp lệ!";
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                    return RedirectToAction(nameof(Details), new { id, returnUrl });
                }

                if (sach.TrangThai != "Đang phục vụ")
                {
                    TempData["Loi"] = $"Không thể xác nhận! Sách '{sach.TenSach}' hiện đang '{sach.TrangThai}'.";
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                    return RedirectToAction(nameof(Details), new { id, returnUrl });
                }

                if (sach.SoLuongCon < ct.SoLuongMuon)
                {
                    TempData["Loi"] = $"Không thể xác nhận! Sách '{sach.TenSach}' chỉ còn {sach.SoLuongCon} cuốn (yêu cầu mượn {ct.SoLuongMuon} cuốn).";
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                    return RedirectToAction(nameof(Details), new { id, returnUrl });
                }
            }

            foreach (var ct in phieu.ChiTietPhieuMuons)
            {
                var sach = _context.Sachs.FirstOrDefault(s => s.MaSach == ct.MaSach);
                if (sach != null)
                {
                    sach.SoLuongCon -= ct.SoLuongMuon;
                    if (sach.SoLuongCon < 0) sach.SoLuongCon = 0;
                    _context.Entry(sach).State = EntityState.Modified;
                }
                ct.TinhTrangTra = "Chưa trả";
            }

            phieu.TrangThai = "Đang mượn";
            _context.SaveChanges();

            TempData["ThongBao"] = $"Đã duyệt cho mượn phiếu #{phieu.MaPhieuMuon} thành công! Số lượng sách tồn kho đã được cập nhật.";
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction(nameof(Details), new { id, returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HuyPhieu(int id, string? returnUrl = null)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            var phieu = _context.PhieuMuons
                .Include(pm => pm.ChiTietPhieuMuons)
                .FirstOrDefault(pm => pm.MaPhieuMuon == id);

            if (phieu == null) return NotFound();

            if (phieu.TrangThai != "Chờ xác nhận" && phieu.TrangThai != "Đang mượn")
            {
                TempData["Loi"] = "Chỉ có thể hủy phiếu khi đang ở trạng thái 'Chờ xác nhận' hoặc 'Đang mượn'!";
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                return RedirectToAction(nameof(Details), new { id, returnUrl });
            }

            if (phieu.TrangThai == "Đang mượn")
            {
                foreach (var ct in phieu.ChiTietPhieuMuons)
                {
                    var sach = _context.Sachs.FirstOrDefault(s => s.MaSach == ct.MaSach);
                    if (sach != null)
                    {
                        sach.SoLuongCon += ct.SoLuongMuon;
                        if (sach.SoLuongCon > sach.SoLuong) sach.SoLuongCon = sach.SoLuong;
                        _context.Entry(sach).State = EntityState.Modified;
                    }
                }
            }

            phieu.TrangThai = "Đã hủy";
            foreach (var ct in phieu.ChiTietPhieuMuons)
            {
                ct.TinhTrangTra = "Đã hủy";
            }

            _context.SaveChanges();
            TempData["ThongBao"] = $"Đã hủy yêu cầu mượn sách phiếu #{phieu.MaPhieuMuon}.";
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult TraSach(int? id, string? returnUrl = null)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (id == null) return NotFound();

            var phieu = _context.PhieuMuons
                .Include(pm => pm.DocGia)
                .Include(pm => pm.ChiTietPhieuMuons)
                    .ThenInclude(ct => ct.Sach)
                .Include(pm => pm.GiaoDichThanhToans)
                .FirstOrDefault(pm => pm.MaPhieuMuon == id);

            if (phieu == null) return NotFound();

            if (phieu.TrangThai != "Đang mượn")
            {
                TempData["Loi"] = "Chỉ có thể thực hiện trả sách cho phiếu đang ở trạng thái 'Đang mượn'!";
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                return RedirectToAction(nameof(Details), new { id, returnUrl });
            }

            var gds = phieu.GiaoDichThanhToans ?? new List<GiaoDichThanhToan>();

            // Tính toán cọc và phạt
            decimal cocDaNop = gds
                .Where(g => g.HuongDongTien == "Nguồn vào" && (g.LoaiThanhToan.Contains("cọc") || g.LoaiThanhToan.Contains("Coc")))
                .Sum(g => g.SoTien);

            decimal cocDaHoan = gds
                .Where(g => g.HuongDongTien == "Nguồn ra" && (g.LoaiThanhToan.Contains("cọc") || g.LoaiThanhToan.Contains("Coc")))
                .Sum(g => g.SoTien);

            decimal tienCocConLai = cocDaNop - cocDaHoan;
            if (tienCocConLai <= 0 && cocDaNop == 0)
            {
                tienCocConLai = 100000; // Mức cọc tiêu chuẩn mặc định
            }

            int soNgayTre = (DateTime.Today - phieu.HanTra.Date).Days;
            decimal tienPhat = (soNgayTre > 0) ? (soNgayTre * 5000) : 0;

            ViewBag.TienCocConLai = tienCocConLai;
            ViewBag.SoNgayTre = soNgayTre;
            ViewBag.TienPhat = tienPhat;
            ViewBag.ReturnUrl = returnUrl;

            return View(phieu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TraSach(int id, DateTime ngayTraThucTe, List<int>? maSachs, List<string>? tinhTrangTras,
            bool hoanTraTienCoc = false, decimal? soTienHoanCoc = null, string? nguonRaHoanCoc = null, string? nguonVaoHoanCoc = null,
            bool thuTienPhat = false, decimal? soTienPhat = null, string? nguonRaThuPhat = null, string? nguonVaoThuPhat = null,
            string? returnUrl = null)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            var phieu = _context.PhieuMuons
                .Include(pm => pm.DocGia)
                .Include(pm => pm.ChiTietPhieuMuons)
                    .ThenInclude(ct => ct.Sach)
                .Include(pm => pm.GiaoDichThanhToans)
                .FirstOrDefault(pm => pm.MaPhieuMuon == id);

            if (phieu == null) return NotFound();

            if (phieu.TrangThai != "Đang mượn")
            {
                TempData["Loi"] = "Phiếu mượn này không ở trạng thái 'Đang mượn'!";
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                return RedirectToAction(nameof(Details), new { id, returnUrl });
            }

            if (ngayTraThucTe == default(DateTime))
            {
                ngayTraThucTe = DateTime.Today;
            }

            if (ngayTraThucTe.Date < phieu.NgayMuon.Date)
            {
                TempData["Loi"] = "Ngày trả thực tế không được nhỏ hơn ngày mượn!";
                return RedirectToAction(nameof(TraSach), new { id, returnUrl });
            }

            for (int i = 0; i < phieu.ChiTietPhieuMuons.Count; i++)
            {
                var ct = phieu.ChiTietPhieuMuons.ElementAt(i);
                ct.NgayTra = ngayTraThucTe.Date;

                int idx = (maSachs != null) ? maSachs.IndexOf(ct.MaSach) : i;
                string tinhTrang = (idx >= 0 && tinhTrangTras != null && idx < tinhTrangTras.Count && !string.IsNullOrWhiteSpace(tinhTrangTras[idx]))
                    ? tinhTrangTras[idx]
                    : "Tốt";
                ct.TinhTrangTra = tinhTrang;

                // Cập nhật hoàn trả số lượng sách tồn kho
                var sach = _context.Sachs.FirstOrDefault(s => s.MaSach == ct.MaSach);
                if (sach != null && tinhTrang != "Mất sách")
                {
                    sach.SoLuongCon += ct.SoLuongMuon;
                    if (sach.SoLuongCon > sach.SoLuong)
                    {
                        sach.SoLuongCon = sach.SoLuong;
                    }
                    _context.Entry(sach).State = EntityState.Modified;
                }
            }

            phieu.TrangThai = "Đã trả";

            // Xử lý Giao dịch Nguồn ra: Hoàn trả tiền cọc cho độc giả
            string thongBaoTaiChinh = "";
            if (hoanTraTienCoc && soTienHoanCoc.HasValue && soTienHoanCoc.Value > 0)
            {
                var gdHoanCoc = new GiaoDichThanhToan
                {
                    MaGiaoDichCode = $"CHI{DateTime.Now:yyyyMMddHHmmss}_PM{phieu.MaPhieuMuon}",
                    MaPhieuMuon = phieu.MaPhieuMuon,
                    MaDocGia = phieu.MaDocGia,
                    HuongDongTien = "Nguồn ra",
                    LoaiThanhToan = "Hoàn trả tiền cọc",
                    SoTien = soTienHoanCoc.Value,
                    NguonTienRa = !string.IsNullOrWhiteSpace(nguonRaHoanCoc) ? nguonRaHoanCoc : "Quỹ tiền mặt quầy thủ thư",
                    NguonTienVao = !string.IsNullOrWhiteSpace(nguonVaoHoanCoc) ? nguonVaoHoanCoc : "Tiền mặt độc giả nhận tại quầy",
                    PhuongThuc = "Tiền mặt tại quầy",
                    NguoiThucHien = phieu.DocGia?.HoTen ?? "Độc giả",
                    SoDienThoai = phieu.DocGia?.SoDienThoai,
                    NoiDung = $"Hoàn trả tiền cọc phiếu mượn #{phieu.MaPhieuMuon}",
                    NgayGiaoDich = DateTime.Now,
                    TrangThai = "Đã hoàn thành"
                };
                _context.GiaoDichThanhToans.Add(gdHoanCoc);
                thongBaoTaiChinh += $" [Đã xuất chi nguồn ra: {gdHoanCoc.SoTien:N0}đ hoàn cọc]";
            }

            // Xử lý Giao dịch Nguồn vào: Thu tiền phạt quá hạn nếu có
            if (thuTienPhat && soTienPhat.HasValue && soTienPhat.Value > 0)
            {
                var gdThuPhat = new GiaoDichThanhToan
                {
                    MaGiaoDichCode = $"THU{DateTime.Now:yyyyMMddHHmmss}_PM{phieu.MaPhieuMuon}",
                    MaPhieuMuon = phieu.MaPhieuMuon,
                    MaDocGia = phieu.MaDocGia,
                    HuongDongTien = "Nguồn vào",
                    LoaiThanhToan = "Phạt trễ hạn trả sách",
                    SoTien = soTienPhat.Value,
                    NguonTienRa = !string.IsNullOrWhiteSpace(nguonRaThuPhat) ? nguonRaThuPhat : "Tiền mặt độc giả nộp tại quầy",
                    NguonTienVao = !string.IsNullOrWhiteSpace(nguonVaoThuPhat) ? nguonVaoThuPhat : "Quỹ tiền mặt quầy thủ thư",
                    PhuongThuc = "Tiền mặt tại quầy",
                    NguoiThucHien = phieu.DocGia?.HoTen ?? "Độc giả",
                    SoDienThoai = phieu.DocGia?.SoDienThoai,
                    NoiDung = $"Thu phạt trễ hạn phiếu mượn #{phieu.MaPhieuMuon}",
                    NgayGiaoDich = DateTime.Now,
                    TrangThai = "Đã hoàn thành"
                };
                _context.GiaoDichThanhToans.Add(gdThuPhat);
                thongBaoTaiChinh += $" [Đã ghi nhận nguồn vào: {gdThuPhat.SoTien:N0}đ tiền phạt]";
            }

            _context.SaveChanges();

            TempData["ThongBao"] = $"Đã xác nhận trả sách cho phiếu #{phieu.MaPhieuMuon} thành công! Số lượng sách trong kho đã được hoàn trả.{thongBaoTaiChinh}";
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction(nameof(Details), new { id, returnUrl });
        }

        // Action alias cho chức năng Xử lý (GET: xem & xử lý, POST: duyệt mượn)
        [HttpGet]
        public IActionResult XuLy(int? id, string? returnUrl = null)
        {
            if (id == null) return NotFound();
            return RedirectToAction(nameof(Details), new { id = id.Value, returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XuLy(int id, string? returnUrl = null)
        {
            return XacNhanMuon(id, returnUrl);
        }

        // Action alias cho chức năng Thu hồi (GET: mở form nhận trả/thu hồi, POST: thu hồi nhanh)
        [HttpGet]
        public IActionResult ThuHoi(int? id, string? returnUrl = null)
        {
            if (id == null) return NotFound();
            return RedirectToAction(nameof(TraSach), new { id = id.Value, returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThuHoi(int id, string? returnUrl = null)
        {
            return ThuHoiNhanh(id, returnUrl);
        }

        // Chức năng Thu hồi nhanh hoàn kho tài liệu quá hạn trực tiếp từ Dashboard hoặc danh sách
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThuHoiNhanh(int id, string? returnUrl = null)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            var phieu = _context.PhieuMuons
                .Include(pm => pm.DocGia)
                .Include(pm => pm.ChiTietPhieuMuons)
                    .ThenInclude(ct => ct.Sach)
                .FirstOrDefault(pm => pm.MaPhieuMuon == id);

            if (phieu == null) return NotFound();

            if (phieu.TrangThai != "Đang mượn")
            {
                TempData["Loi"] = "Chỉ có thể thu hồi đối với phiếu đang ở trạng thái 'Đang mượn'!";
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                return RedirectToAction(nameof(Details), new { id, returnUrl });
            }

            foreach (var ct in phieu.ChiTietPhieuMuons)
            {
                ct.NgayTra = DateTime.Today;
                ct.TinhTrangTra = "Đã thu hồi (Tốt)";

                var sach = _context.Sachs.FirstOrDefault(s => s.MaSach == ct.MaSach);
                if (sach != null)
                {
                    sach.SoLuongCon += ct.SoLuongMuon;
                    if (sach.SoLuongCon > sach.SoLuong)
                    {
                        sach.SoLuongCon = sach.SoLuong;
                    }
                    _context.Entry(sach).State = EntityState.Modified;
                }
            }

            phieu.TrangThai = "Đã trả";
            _context.SaveChanges();

            TempData["ThongBao"] = $"Đã thu hồi tài liệu và hoàn trả toàn bộ số lượng sách của phiếu #{phieu.MaPhieuMuon} về kho thư viện thành công!";
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction(nameof(Details), new { id, returnUrl });
        }

        public IActionResult QuaHan()
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            DateTime homNay = DateTime.Today;

            var danhSachQuaHan = _context.PhieuMuons
                .Include(pm => pm.DocGia)
                .Include(pm => pm.ChiTietPhieuMuons)
                    .ThenInclude(ct => ct.Sach)
                .Include(pm => pm.GiaoDichThanhToans)
                .Where(pm => pm.TrangThai == "Đang mượn" && pm.HanTra.Date < homNay)
                .OrderBy(pm => pm.HanTra)
                .ToList();

            return View(danhSachQuaHan);
        }
    }
}
