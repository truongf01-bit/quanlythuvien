using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using QuanLyThuVien.ViewModels;

namespace QuanLyThuVien.Controllers
{
    public class SachController : Controller
    {
        private readonly ThuVienDbContext _context;

        public SachController(ThuVienDbContext context)
        {
            _context = context;
        }

        private bool LaAdmin()
        {
            return HttpContext.Session.GetString("VaiTro") == "Admin";
        }

        public IActionResult Index(
            string? tuKhoa,
            int? maTheLoai,
            string? nhaXuatBan,
            string? trangThai,
            int? tuNam,
            int? denNam,
            string? sapXep = "ten_asc",
            int trang = 1)
        {
            int kichThuocTrang = 6;
            if (trang < 1) trang = 1;

            IQueryable<Sach> query = _context.Sachs
                .Include(s => s.TheLoai)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string kw = tuKhoa.Trim().ToLower();
                query = query.Where(s =>
                    s.TenSach.ToLower().Contains(kw) ||
                    s.TacGia.ToLower().Contains(kw));
            }

            if (maTheLoai.HasValue && maTheLoai.Value > 0)
            {
                query = query.Where(s => s.MaTheLoai == maTheLoai.Value);
            }

            if (!string.IsNullOrWhiteSpace(nhaXuatBan))
            {
                query = query.Where(s => s.NhaXuatBan == nhaXuatBan);
            }

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(s => s.TrangThai == trangThai);
            }

            if (tuNam.HasValue)
            {
                query = query.Where(s => s.NamXuatBan >= tuNam.Value);
            }
            if (denNam.HasValue)
            {
                query = query.Where(s => s.NamXuatBan <= denNam.Value);
            }

            switch (sapXep)
            {
                case "ten_desc":
                    query = query.OrderByDescending(s => s.TenSach);
                    break;
                case "nam_asc":
                    query = query.OrderBy(s => s.NamXuatBan);
                    break;
                case "nam_desc":
                    query = query.OrderByDescending(s => s.NamXuatBan);
                    break;
                case "gia_asc":
                    query = query.OrderBy(s => s.GiaSach);
                    break;
                case "gia_desc":
                    query = query.OrderByDescending(s => s.GiaSach);
                    break;
                case "ten_asc":
                default:
                    query = query.OrderBy(s => s.TenSach);
                    break;
            }

            int tongSoBanGhi = query.Count();
            int tongSoTrang = (int)Math.Ceiling(tongSoBanGhi / (double)kichThuocTrang);
            if (tongSoTrang < 1) tongSoTrang = 1;
            if (trang > tongSoTrang) trang = tongSoTrang;

            var danhSachSach = query
                .Skip((trang - 1) * kichThuocTrang)
                .Take(kichThuocTrang)
                .ToList();

            var viewModel = new SachIndexViewModel
            {
                DanhSachSach = danhSachSach,
                TuKhoa = tuKhoa,
                MaTheLoai = maTheLoai,
                NhaXuatBan = nhaXuatBan,
                TrangThai = trangThai,
                TuNam = tuNam,
                DenNam = denNam,
                SapXep = sapXep,
                TrangHienTai = trang,
                TongSoTrang = tongSoTrang,
                TongSoBanGhi = tongSoBanGhi,
                KichThuocTrang = kichThuocTrang,
                DanhSachTheLoai = _context.TheLoais.OrderBy(tl => tl.TenTheLoai).ToList(),
                DanhSachNhaXuatBan = _context.Sachs
                    .Select(s => s.NhaXuatBan)
                    .Distinct()
                    .OrderBy(nxb => nxb)
                    .ToList()
            };

            return View(viewModel);
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var sach = _context.Sachs
                .Include(s => s.TheLoai)
                .FirstOrDefault(s => s.MaSach == id);

            if (sach == null) return NotFound();

            return View(sach);
        }

        public IActionResult Create()
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            ViewBag.DanhSachTheLoai = new SelectList(
                _context.TheLoais.Where(tl => tl.TrangThai == "Đang sử dụng").ToList(),
                "MaTheLoai",
                "TenTheLoai");

            return View(new Sach { NamXuatBan = DateTime.Now.Year, SoLuong = 10, SoLuongCon = 10, GiaSach = 100000 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sach sach)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (sach.SoLuongCon > sach.SoLuong)
            {
                ModelState.AddModelError("SoLuongCon", "Số lượng còn không được lớn hơn tổng số lượng sách!");
            }

            if (string.IsNullOrWhiteSpace(sach.HinhAnh))
            {
                sach.HinhAnh = "/images/books/default-book.svg";
            }

            if (ModelState.IsValid)
            {
                _context.Sachs.Add(sach);
                _context.SaveChanges();
                TempData["ThongBao"] = $"Đã thêm đầu sách '{sach.TenSach}' thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.DanhSachTheLoai = new SelectList(
                _context.TheLoais.ToList(),
                "MaTheLoai",
                "TenTheLoai",
                sach.MaTheLoai);

            return View(sach);
        }

        public IActionResult Edit(int? id)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (id == null) return NotFound();

            var sach = _context.Sachs.Find(id);
            if (sach == null) return NotFound();

            ViewBag.DanhSachTheLoai = new SelectList(
                _context.TheLoais.ToList(),
                "MaTheLoai",
                "TenTheLoai",
                sach.MaTheLoai);

            return View(sach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Sach sach)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (id != sach.MaSach) return NotFound();

            if (sach.SoLuongCon > sach.SoLuong)
            {
                ModelState.AddModelError("SoLuongCon", "Số lượng còn không được lớn hơn tổng số lượng sách!");
            }

            if (string.IsNullOrWhiteSpace(sach.HinhAnh))
            {
                sach.HinhAnh = "/images/books/default-book.svg";
            }

            if (ModelState.IsValid)
            {
                _context.Sachs.Update(sach);
                _context.SaveChanges();
                TempData["ThongBao"] = $"Đã cập nhật thông tin sách '{sach.TenSach}' thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.DanhSachTheLoai = new SelectList(
                _context.TheLoais.ToList(),
                "MaTheLoai",
                "TenTheLoai",
                sach.MaTheLoai);

            return View(sach);
        }

        public IActionResult Delete(int? id)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (id == null) return NotFound();

            var sach = _context.Sachs
                .Include(s => s.TheLoai)
                .Include(s => s.ChiTietPhieuMuons)
                .FirstOrDefault(s => s.MaSach == id);

            if (sach == null) return NotFound();

            return View(sach);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            var sach = _context.Sachs
                .Include(s => s.ChiTietPhieuMuons)
                .FirstOrDefault(s => s.MaSach == id);

            if (sach == null) return NotFound();

            if (sach.ChiTietPhieuMuons.Any())
            {
                TempData["Loi"] = $"Không thể xóa sách '{sach.TenSach}' vì sách đã có trong lịch sử phiếu mượn! Vui lòng chuyển trạng thái sang 'Ngừng phục vụ'.";
                return RedirectToAction(nameof(Index));
            }

            _context.Sachs.Remove(sach);
            _context.SaveChanges();
            TempData["ThongBao"] = $"Đã xóa sách '{sach.TenSach}' khỏi hệ thống!";
            return RedirectToAction(nameof(Index));
        }
    }
}
