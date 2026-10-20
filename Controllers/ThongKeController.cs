using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.ViewModels;

namespace QuanLyThuVien.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly ThuVienDbContext _context;

        public ThongKeController(ThuVienDbContext context)
        {
            _context = context;
        }

        private bool LaAdmin()
        {
            return HttpContext.Session.GetString("VaiTro") == "Admin";
        }

        public IActionResult Dashboard()
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            DateTime homNay = DateTime.Today;

            var vm = new DashboardViewModel
            {

                TongDauSach = _context.Sachs.Count(),
                TongTheLoai = _context.TheLoais.Count(),
                TongDocGia = _context.DocGias.Count(),
                TongPhieuMuon = _context.PhieuMuons.Count(),
                PhieuChoXacNhan = _context.PhieuMuons.Count(pm => pm.TrangThai == "Chờ xác nhận"),
                PhieuDangMuon = _context.PhieuMuons.Count(pm => pm.TrangThai == "Đang mượn"),
                TruongHopQuaHan = _context.PhieuMuons.Count(pm => pm.TrangThai == "Đang mượn" && pm.HanTra.Date < homNay),
                TongSachConCoTheMuon = _context.Sachs
                    .Where(s => s.TrangThai == "Đang phục vụ")
                    .Sum(s => (int?)s.SoLuongCon) ?? 0,

                DanhSachChoXacNhan = _context.PhieuMuons
                    .Include(pm => pm.DocGia)
                    .Include(pm => pm.ChiTietPhieuMuons)
                    .Where(pm => pm.TrangThai == "Chờ xác nhận")
                    .OrderByDescending(pm => pm.NgayMuon)
                    .Take(5)
                    .ToList(),

                DanhSachQuaHan = _context.PhieuMuons
                    .Include(pm => pm.DocGia)
                    .Include(pm => pm.ChiTietPhieuMuons)
                    .Where(pm => pm.TrangThai == "Đang mượn" && pm.HanTra.Date < homNay)
                    .OrderBy(pm => pm.HanTra)
                    .Take(5)
                    .ToList()
            };

            return View(vm);
        }

        public IActionResult Index()
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            var sachTheoTheLoai = _context.TheLoais
                .Include(tl => tl.Sachs)
                .Select(tl => new ThongKeTheLoaiItem
                {
                    MaTheLoai = tl.MaTheLoai,
                    TenTheLoai = tl.TenTheLoai,
                    SoDauSach = tl.Sachs.Count(),
                    TongSoBanSach = tl.Sachs.Sum(s => (int?)s.SoLuong) ?? 0,
                    TongSoSachCon = tl.Sachs.Sum(s => (int?)s.SoLuongCon) ?? 0
                })
                .OrderByDescending(x => x.SoDauSach)
                .ToList();

            var luotMuonTheoSach = _context.ChiTietPhieuMuons
                .Include(ct => ct.Sach)
                .Include(ct => ct.PhieuMuon)
                .Where(ct => ct.PhieuMuon != null && ct.PhieuMuon.TrangThai != "Đã hủy" && ct.Sach != null)
                .GroupBy(ct => new { ct.MaSach, ct.Sach!.TenSach, ct.Sach.TacGia })
                .Select(g => new ThongKeSachMuonItem
                {
                    MaSach = g.Key.MaSach,
                    TenSach = g.Key.TenSach,
                    TacGia = g.Key.TacGia,
                    SoLuotPhieuMuon = g.Count(),
                    TongSoCuonDuocMuon = g.Sum(x => x.SoLuongMuon)
                })
                .OrderByDescending(x => x.TongSoCuonDuocMuon)
                .ThenBy(x => x.TenSach)
                .ToList();

            var phieuTheoTrangThai = _context.PhieuMuons
                .GroupBy(pm => pm.TrangThai)
                .Select(g => new ThongKeTrangThaiPhieuItem
                {
                    TrangThai = g.Key,
                    SoLuongPhieu = g.Count()
                })
                .OrderByDescending(x => x.SoLuongPhieu)
                .ToList();

            var sachMuonNhieuNhat = luotMuonTheoSach
                .OrderByDescending(x => x.TongSoCuonDuocMuon)
                .ThenByDescending(x => x.SoLuotPhieuMuon)
                .Take(5)
                .ToList();

            var luotMuonTheoDocGia = _context.PhieuMuons
                .Include(pm => pm.DocGia)
                .Include(pm => pm.ChiTietPhieuMuons)
                .Where(pm => pm.TrangThai != "Đã hủy" && pm.DocGia != null)
                .GroupBy(pm => new { pm.MaDocGia, pm.DocGia!.HoTen, pm.DocGia.SoDienThoai })
                .Select(g => new ThongKeDocGiaMuonItem
                {
                    MaDocGia = g.Key.MaDocGia,
                    HoTen = g.Key.HoTen,
                    SoDienThoai = g.Key.SoDienThoai,
                    SoPhieuMuon = g.Count(),
                    TongSachDaMuon = g.SelectMany(pm => pm.ChiTietPhieuMuons).Sum(ct => ct.SoLuongMuon)
                })
                .OrderByDescending(x => x.SoPhieuMuon)
                .ThenByDescending(x => x.TongSachDaMuon)
                .ToList();

            var phieuMuonTheoThang = _context.PhieuMuons
                .Include(pm => pm.ChiTietPhieuMuons)
                .GroupBy(pm => new { Nam = pm.NgayMuon.Year, Thang = pm.NgayMuon.Month })
                .Select(g => new ThongKeTheoThangItem
                {
                    Nam = g.Key.Nam,
                    Thang = g.Key.Thang,
                    SoPhieuMuon = g.Count(),
                    TongSoSachMuon = g.SelectMany(pm => pm.ChiTietPhieuMuons).Sum(ct => ct.SoLuongMuon)
                })
                .OrderByDescending(x => x.Nam)
                .ThenByDescending(x => x.Thang)
                .ToList();

            var vm = new ThongKeViewModel
            {
                SachTheoTheLoai = sachTheoTheLoai,
                LuotMuonTheoSach = luotMuonTheoSach,
                PhieuTheoTrangThai = phieuTheoTrangThai,
                SachMuonNhieuNhat = sachMuonNhieuNhat,
                LuotMuonTheoDocGia = luotMuonTheoDocGia,
                PhieuMuonTheoThang = phieuMuonTheoThang
            };

            return View(vm);
        }
    }
}
