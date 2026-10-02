using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using QuanLyThuVien.ViewModels;

namespace QuanLyThuVien.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly ThuVienDbContext _context;

        public TaiKhoanController(ThuVienDbContext context)
        {
            _context = context;
        }

        private bool LaAdmin()
        {
            return HttpContext.Session.GetString("VaiTro") == "Admin";
        }

        [HttpGet]
        public IActionResult DangNhap(string? tenDangNhap = null, string? returnUrl = null)
        {
            if (HttpContext.Session.GetInt32("MaTaiKhoan") != null)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            ViewBag.TenDangNhap = tenDangNhap;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpGet]
        public IActionResult DangKy()
        {
            if (HttpContext.Session.GetInt32("MaTaiKhoan") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new DangKyViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangKy(DangKyViewModel model)
        {
            if (model.NgaySinh >= DateTime.Today.AddYears(-10))
            {
                ModelState.AddModelError("NgaySinh", "Ngày sinh không hợp lệ (độc giả phải từ 10 tuổi trở lên)!");
            }

            if (_context.TaiKhoans.Any(tk => tk.TenDangNhap.ToLower() == model.TenDangNhap.Trim().ToLower()))
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập này đã được sử dụng. Vui lòng chọn tên đăng nhập khác!");
            }

            if (_context.DocGias.Any(dg => dg.Email.ToLower() == model.Email.Trim().ToLower()))
            {
                ModelState.AddModelError("Email", "Địa chỉ Email này đã được đăng ký cho độc giả khác!");
            }

            if (_context.DocGias.Any(dg => dg.SoDienThoai == model.SoDienThoai.Trim()))
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại này đã được đăng ký cho độc giả khác!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = model.TenDangNhap.Trim(),
                MatKhau = model.MatKhau,
                HoTen = model.HoTen.Trim(),
                VaiTro = "DocGia",
                TrangThai = "Hoạt động"
            };

            _context.TaiKhoans.Add(taiKhoan);
            _context.SaveChanges();

            var docGia = new DocGia
            {
                MaTaiKhoan = taiKhoan.MaTaiKhoan,
                HoTen = model.HoTen.Trim(),
                NgaySinh = model.NgaySinh,
                GioiTinh = model.GioiTinh,
                SoDienThoai = model.SoDienThoai.Trim(),
                Email = model.Email.Trim(),
                DiaChi = model.DiaChi.Trim(),
                NgayDangKy = DateTime.Today,
                TrangThai = "Hoạt động"
            };

            _context.DocGias.Add(docGia);
            _context.SaveChanges();

            TempData["ThongBao"] = $"Chúc mừng bạn {taiKhoan.HoTen} đã đăng ký tài khoản Độc giả thành công! Vui lòng đăng nhập.";
            return RedirectToAction("DangNhap", new { tenDangNhap = taiKhoan.TenDangNhap });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangNhap(string tenDangNhap, string matKhau, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(matKhau))
            {
                ViewBag.Loi = "Vui lòng nhập đầy đủ Tên đăng nhập và Mật khẩu!";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            var taiKhoan = _context.TaiKhoans
                .Include(tk => tk.DocGia)
                .FirstOrDefault(tk => tk.TenDangNhap == tenDangNhap.Trim() && tk.MatKhau == matKhau);

            if (taiKhoan == null)
            {
                ViewBag.Loi = "Tên đăng nhập hoặc mật khẩu không chính xác!";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            if (taiKhoan.TrangThai != "Hoạt động")
            {
                ViewBag.Loi = "Tài khoản của bạn hiện đang bị khóa. Vui lòng liên hệ Quản trị viên!";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            HttpContext.Session.SetInt32("MaTaiKhoan", taiKhoan.MaTaiKhoan);
            HttpContext.Session.SetString("TenDangNhap", taiKhoan.TenDangNhap);
            HttpContext.Session.SetString("HoTen", taiKhoan.HoTen);
            HttpContext.Session.SetString("VaiTro", taiKhoan.VaiTro);

            if (taiKhoan.DocGia != null)
            {
                HttpContext.Session.SetInt32("MaDocGia", taiKhoan.DocGia.MaDocGia);
            }

            TempData["ThongBao"] = $"Xin chào {taiKhoan.HoTen}! Đăng nhập thành công.";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (taiKhoan.VaiTro == "Admin")
            {
                return RedirectToAction("Dashboard", "ThongKe");
            }
            return RedirectToAction("Index", "Sach");
        }

        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            TempData["ThongBao"] = "Bạn đã đăng xuất khỏi hệ thống.";
            return RedirectToAction("DangNhap");
        }

        public IActionResult KhongCoQuyen()
        {
            return View();
        }

        public IActionResult Index()
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen");
            }

            var danhSach = _context.TaiKhoans
                .Include(tk => tk.DocGia)
                .OrderBy(tk => tk.VaiTro)
                .ThenBy(tk => tk.MaTaiKhoan)
                .ToList();

            return View(danhSach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DoiTrangThai(int id)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen");
            }

            var taiKhoan = _context.TaiKhoans
                .Include(tk => tk.DocGia)
                .FirstOrDefault(tk => tk.MaTaiKhoan == id);

            if (taiKhoan == null)
            {
                return NotFound();
            }

            var maDangNhap = HttpContext.Session.GetInt32("MaTaiKhoan");
            if (taiKhoan.MaTaiKhoan == maDangNhap)
            {
                TempData["Loi"] = "Không thể tự khóa tài khoản Admin đang đăng nhập!";
                return RedirectToAction(nameof(Index));
            }

            taiKhoan.TrangThai = (taiKhoan.TrangThai == "Hoạt động") ? "Bị khóa" : "Hoạt động";
            if (taiKhoan.DocGia != null)
            {
                taiKhoan.DocGia.TrangThai = taiKhoan.TrangThai;
            }

            _context.SaveChanges();
            TempData["ThongBao"] = $"Đã cập nhật trạng thái tài khoản '{taiKhoan.TenDangNhap}' thành '{taiKhoan.TrangThai}'.";
            return RedirectToAction(nameof(Index));
        }
    }
}
