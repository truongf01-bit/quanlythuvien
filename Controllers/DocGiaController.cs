using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    public class DocGiaController : Controller
    {
        private readonly ThuVienDbContext _context;

        public DocGiaController(ThuVienDbContext context)
        {
            _context = context;
        }

        private bool LaAdmin()
        {
            return HttpContext.Session.GetString("VaiTro") == "Admin";
        }

        public IActionResult Index(string? tuKhoa, string? trangThai)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            IQueryable<DocGia> query = _context.DocGias
                .Include(dg => dg.TaiKhoan)
                .Include(dg => dg.PhieuMuons)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string kw = tuKhoa.Trim().ToLower();
                query = query.Where(dg =>
                    dg.HoTen.ToLower().Contains(kw) ||
                    dg.SoDienThoai.Contains(kw) ||
                    dg.Email.ToLower().Contains(kw));
            }

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(dg => dg.TrangThai == trangThai);
            }

            ViewBag.TuKhoa = tuKhoa;
            ViewBag.TrangThai = trangThai;

            var danhSach = query.OrderBy(dg => dg.MaDocGia).ToList();
            return View(danhSach);
        }

        public IActionResult Details(int? id)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (id == null) return NotFound();

            var docGia = _context.DocGias
                .Include(dg => dg.TaiKhoan)
                .Include(dg => dg.PhieuMuons)
                    .ThenInclude(pm => pm.ChiTietPhieuMuons)
                        .ThenInclude(ct => ct.Sach)
                .FirstOrDefault(dg => dg.MaDocGia == id);

            if (docGia == null) return NotFound();

            return View(docGia);
        }

        public IActionResult Create()
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            return View(new DocGia
            {
                NgaySinh = new DateTime(2004, 1, 1),
                NgayDangKy = DateTime.Today,
                TrangThai = "Hoạt động"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DocGia docGia, string tenDangNhap, string matKhau)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (string.IsNullOrWhiteSpace(tenDangNhap) || tenDangNhap.Trim().Length < 3)
            {
                ModelState.AddModelError("", "Tên đăng nhập cho độc giả phải có từ 3 ký tự trở lên!");
            }
            else if (_context.TaiKhoans.Any(tk => tk.TenDangNhap.ToLower() == tenDangNhap.Trim().ToLower()))
            {
                ModelState.AddModelError("", "Tên đăng nhập này đã tồn tại trong hệ thống!");
            }

            if (string.IsNullOrWhiteSpace(matKhau) || matKhau.Length < 4)
            {
                ModelState.AddModelError("", "Mật khẩu phải có từ 4 ký tự trở lên!");
            }

            if (docGia.NgaySinh >= DateTime.Today)
            {
                ModelState.AddModelError("NgaySinh", "Ngày sinh phải nhỏ hơn ngày hiện tại!");
            }

            if (ModelState.IsValid)
            {

                var taiKhoanMoi = new TaiKhoan
                {
                    TenDangNhap = tenDangNhap.Trim(),
                    MatKhau = matKhau,
                    HoTen = docGia.HoTen.Trim(),
                    VaiTro = "DocGia",
                    TrangThai = docGia.TrangThai
                };
                _context.TaiKhoans.Add(taiKhoanMoi);
                _context.SaveChanges();

                docGia.MaTaiKhoan = taiKhoanMoi.MaTaiKhoan;
                docGia.NgayDangKy = DateTime.Today;
                _context.DocGias.Add(docGia);
                _context.SaveChanges();

                TempData["ThongBao"] = $"Đã thêm độc giả '{docGia.HoTen}' (Tài khoản: {taiKhoanMoi.TenDangNhap}) thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TenDangNhap = tenDangNhap;
            return View(docGia);
        }

        public IActionResult Edit(int? id)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (id == null) return NotFound();

            var docGia = _context.DocGias
                .Include(dg => dg.TaiKhoan)
                .FirstOrDefault(dg => dg.MaDocGia == id);

            if (docGia == null) return NotFound();

            return View(docGia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DocGia docGia)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (id != docGia.MaDocGia) return NotFound();

            if (docGia.NgaySinh >= DateTime.Today)
            {
                ModelState.AddModelError("NgaySinh", "Ngày sinh phải nhỏ hơn ngày hiện tại!");
            }

            if (ModelState.IsValid)
            {
                _context.DocGias.Update(docGia);

                var taiKhoan = _context.TaiKhoans.Find(docGia.MaTaiKhoan);
                if (taiKhoan != null)
                {
                    taiKhoan.HoTen = docGia.HoTen;
                    taiKhoan.TrangThai = docGia.TrangThai;
                }

                _context.SaveChanges();
                TempData["ThongBao"] = $"Đã cập nhật thông tin độc giả '{docGia.HoTen}' thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(docGia);
        }

        [HttpGet]
        public IActionResult ThongTinCaNhan()
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            if (maTaiKhoan == null)
            {
                TempData["Loi"] = "Vui lòng đăng nhập để xem thông tin cá nhân!";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var docGia = _context.DocGias
                .Include(dg => dg.TaiKhoan)
                .FirstOrDefault(dg => dg.MaTaiKhoan == maTaiKhoan.Value);

            if (docGia == null)
            {
                TempData["Loi"] = "Tài khoản Quản trị viên không có hồ sơ Độc giả cá nhân.";
                return RedirectToAction("Index", "Home");
            }

            return View(docGia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThongTinCaNhan(DocGia model)
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            if (maTaiKhoan == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var docGiaHienTai = _context.DocGias
                .Include(dg => dg.TaiKhoan)
                .FirstOrDefault(dg => dg.MaTaiKhoan == maTaiKhoan.Value);

            if (docGiaHienTai == null) return NotFound();

            if (model.NgaySinh >= DateTime.Today)
            {
                ModelState.AddModelError("NgaySinh", "Ngày sinh phải nhỏ hơn ngày hiện tại!");
            }

            if (ModelState.IsValid)
            {

                docGiaHienTai.HoTen = model.HoTen.Trim();
                docGiaHienTai.NgaySinh = model.NgaySinh;
                docGiaHienTai.GioiTinh = model.GioiTinh;
                docGiaHienTai.SoDienThoai = model.SoDienThoai.Trim();
                docGiaHienTai.Email = model.Email.Trim();
                docGiaHienTai.DiaChi = model.DiaChi.Trim();

                if (docGiaHienTai.TaiKhoan != null)
                {
                    docGiaHienTai.TaiKhoan.HoTen = docGiaHienTai.HoTen;
                }

                _context.SaveChanges();
                HttpContext.Session.SetString("HoTen", docGiaHienTai.HoTen);

                TempData["ThongBao"] = "Cập nhật thông tin cá nhân thành công!";
                return RedirectToAction(nameof(ThongTinCaNhan));
            }

            return View(model);
        }
    }
}
