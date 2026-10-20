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

        public IActionResult Details(int? id)
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
                .FirstOrDefault(pm => pm.MaPhieuMuon == id);

            if (phieu == null) return NotFound();

            return View(phieu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XacNhanMuon(int id)
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
                return RedirectToAction(nameof(Details), new { id });
            }

            foreach (var ct in phieu.ChiTietPhieuMuons)
            {
                if (ct.Sach == null)
                {
                    TempData["Loi"] = "Dữ liệu sách trong phiếu không hợp lệ!";
                    return RedirectToAction(nameof(Details), new { id });
                }

                if (ct.Sach.TrangThai != "Đang phục vụ")
                {
                    TempData["Loi"] = $"Không thể xác nhận! Sách '{ct.Sach.TenSach}' hiện đang '{ct.Sach.TrangThai}'.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                if (ct.Sach.SoLuongCon < ct.SoLuongMuon)
                {
                    TempData["Loi"] = $"Không thể xác nhận! Sách '{ct.Sach.TenSach}' chỉ còn {ct.Sach.SoLuongCon} cuốn (yêu cầu mượn {ct.SoLuongMuon} cuốn).";
                    return RedirectToAction(nameof(Details), new { id });
                }
            }

            foreach (var ct in phieu.ChiTietPhieuMuons)
            {
                ct.Sach!.SoLuongCon -= ct.SoLuongMuon;
                ct.TinhTrangTra = "Chưa trả";
            }

            phieu.TrangThai = "Đang mượn";
            _context.SaveChanges();

            TempData["ThongBao"] = $"Đã xác nhận cho mượn phiếu #{phieu.MaPhieuMuon} thành công! Số lượng sách tồn kho đã được cập nhật.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HuyPhieu(int id)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            var phieu = _context.PhieuMuons
                .Include(pm => pm.ChiTietPhieuMuons)
                .FirstOrDefault(pm => pm.MaPhieuMuon == id);

            if (phieu == null) return NotFound();

            if (phieu.TrangThai != "Chờ xác nhận")
            {
                TempData["Loi"] = "Chỉ có thể hủy phiếu khi đang ở trạng thái 'Chờ xác nhận'!";
                return RedirectToAction(nameof(Details), new { id });
            }

            phieu.TrangThai = "Đã hủy";
            foreach (var ct in phieu.ChiTietPhieuMuons)
            {
                ct.TinhTrangTra = "Đã hủy";
            }

            _context.SaveChanges();
            TempData["ThongBao"] = $"Đã hủy yêu cầu mượn sách phiếu #{phieu.MaPhieuMuon}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult TraSach(int? id)
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
                .FirstOrDefault(pm => pm.MaPhieuMuon == id);

            if (phieu == null) return NotFound();

            if (phieu.TrangThai != "Đang mượn")
            {
                TempData["Loi"] = "Chỉ có thể thực hiện trả sách cho phiếu đang ở trạng thái 'Đang mượn'!";
                return RedirectToAction(nameof(Details), new { id });
            }

            return View(phieu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TraSach(int id, DateTime ngayTraThucTe, List<int> maSachs, List<string> tinhTrangTras)
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

            if (phieu.TrangThai != "Đang mượn")
            {
                TempData["Loi"] = "Phiếu mượn này không ở trạng thái 'Đang mượn'!";
                return RedirectToAction(nameof(Details), new { id });
            }

            if (ngayTraThucTe.Date < phieu.NgayMuon.Date)
            {
                TempData["Loi"] = "Ngày trả thực tế không được nhỏ hơn ngày mượn!";
                return RedirectToAction(nameof(TraSach), new { id });
            }

            for (int i = 0; i < phieu.ChiTietPhieuMuons.Count; i++)
            {
                var ct = phieu.ChiTietPhieuMuons.ElementAt(i);
                ct.NgayTra = ngayTraThucTe.Date;

                int idx = maSachs.IndexOf(ct.MaSach);
                string tinhTrang = (idx >= 0 && idx < tinhTrangTras.Count && !string.IsNullOrWhiteSpace(tinhTrangTras[idx]))
                    ? tinhTrangTras[idx]
                    : "Tốt";
                ct.TinhTrangTra = tinhTrang;

                if (ct.Sach != null && tinhTrang != "Mất sách")
                {
                    ct.Sach.SoLuongCon += ct.SoLuongMuon;
                    if (ct.Sach.SoLuongCon > ct.Sach.SoLuong)
                    {
                        ct.Sach.SoLuongCon = ct.Sach.SoLuong;
                    }
                }
            }

            phieu.TrangThai = "Đã trả";
            _context.SaveChanges();

            TempData["ThongBao"] = $"Đã xác nhận trả sách cho phiếu #{phieu.MaPhieuMuon} thành công! Số lượng sách trong kho đã được cập nhật.";
            return RedirectToAction(nameof(Details), new { id });
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
                .Where(pm => pm.TrangThai == "Đang mượn" && pm.HanTra.Date < homNay)
                .OrderBy(pm => pm.HanTra)
                .ToList();

            return View(danhSachQuaHan);
        }
    }
}
