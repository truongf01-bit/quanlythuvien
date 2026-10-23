// Họ và tên: Nguyễn Văn An
// Mã sinh viên: 22103100001
// Nội dung thực hiện: Module 1 - Quản lý tài khoản, Đăng nhập, Đăng xuất và Phân quyền bằng Session

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

        // Kiểm tra quyền Admin tại Controller
        private bool LaAdmin()
        {
            return HttpContext.Session.GetString("VaiTro") == "Admin";
        }

        // GET: /TaiKhoan/DangNhap
        [HttpGet]
        public IActionResult DangNhap(string? tenDangNhap = null)
        {
            // Nếu đã đăng nhập rồi thì chuyển về Trang chủ
            if (HttpContext.Session.GetInt32("MaTaiKhoan") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.TenDangNhap = tenDangNhap;
            return View();
        }

        // GET: /TaiKhoan/DangKy (Đăng ký tài khoản Độc giả / Sinh viên)
        [HttpGet]
        public IActionResult DangKy()
        {
            // Nếu đã đăng nhập rồi thì chuyển về Trang chủ
            if (HttpContext.Session.GetInt32("MaTaiKhoan") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new DangKyViewModel());
        }

        // POST: /TaiKhoan/DangKy (Xử lý tạo tài khoản Độc giả & liên kết hồ sơ)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangKy(DangKyViewModel model)
        {
            // 1. Kiểm tra ngày sinh hợp lệ (phải nhỏ hơn ngày hiện tại ít nhất 10 tuổi)
            if (model.NgaySinh >= DateTime.Today.AddYears(-10))
            {
                ModelState.AddModelError("NgaySinh", "Ngày sinh không hợp lệ (độc giả phải từ 10 tuổi trở lên)!");
            }

            // 2. Kiểm tra trùng tên đăng nhập trong hệ thống
            if (_context.TaiKhoans.Any(tk => tk.TenDangNhap.ToLower() == model.TenDangNhap.Trim().ToLower()))
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập này đã được sử dụng. Vui lòng chọn tên đăng nhập khác!");
            }

            // 3. Kiểm tra trùng Email trong danh sách độc giả
            if (_context.DocGias.Any(dg => dg.Email.ToLower() == model.Email.Trim().ToLower()))
            {
                ModelState.AddModelError("Email", "Địa chỉ Email này đã được đăng ký cho độc giả khác!");
            }

            // 4. Kiểm tra trùng Số điện thoại trong danh sách độc giả
            if (_context.DocGias.Any(dg => dg.SoDienThoai == model.SoDienThoai.Trim()))
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại này đã được đăng ký cho độc giả khác!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 5. Tạo mới đối tượng Tài khoản (Mặc định phân quyền VaiTro = "DocGia")
            // Đảm bảo bảo mật: Người dùng tự đăng ký luôn mang vai trò Độc giả (User), không thể tự cấp quyền Admin!
            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = model.TenDangNhap.Trim(),
                MatKhau = model.MatKhau,
                HoTen = model.HoTen.Trim(),
                VaiTro = "DocGia", // Phân rõ: User là Độc giả (Sinh viên)
                TrangThai = "Hoạt động"
            };

            _context.TaiKhoans.Add(taiKhoan);
            _context.SaveChanges(); // Lưu để sinh tự động MaTaiKhoan (Identity)

            // 6. Tạo mới hồ sơ Độc giả liên kết 1 - 1 với Tài khoản vừa tạo
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

        // POST: /TaiKhoan/DangNhap
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangNhap(string tenDangNhap, string matKhau)
        {
            // 1. Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(matKhau))
            {
                ViewBag.Loi = "Vui lòng nhập đầy đủ Tên đăng nhập và Mật khẩu!";
                return View();
            }

            // 2. Truy vấn tài khoản bằng Entity Framework Core và LINQ
            var taiKhoan = _context.TaiKhoans
                .Include(tk => tk.DocGia)
                .FirstOrDefault(tk => tk.TenDangNhap == tenDangNhap.Trim() && tk.MatKhau == matKhau);

            if (taiKhoan == null)
            {
                ViewBag.Loi = "Tên đăng nhập hoặc mật khẩu không chính xác!";
                return View();
            }

            // 3. Kiểm tra trạng thái hoạt động của tài khoản
            if (taiKhoan.TrangThai != "Hoạt động")
            {
                ViewBag.Loi = "Tài khoản của bạn hiện đang bị khóa. Vui lòng liên hệ Quản trị viên!";
                return View();
            }

            // 4. Lưu thông tin tối thiểu (Mã tài khoản, Họ tên, Vai trò) vào Session
            HttpContext.Session.SetInt32("MaTaiKhoan", taiKhoan.MaTaiKhoan);
            HttpContext.Session.SetString("TenDangNhap", taiKhoan.TenDangNhap);
            HttpContext.Session.SetString("HoTen", taiKhoan.HoTen);
            HttpContext.Session.SetString("VaiTro", taiKhoan.VaiTro);

            if (taiKhoan.DocGia != null)
            {
                HttpContext.Session.SetInt32("MaDocGia", taiKhoan.DocGia.MaDocGia);
            }

            TempData["ThongBao"] = $"Xin chào {taiKhoan.HoTen}! Đăng nhập thành công.";

            // Nếu là Admin thì chuyển tới Dashboard, nếu là Độc giả thì chuyển tới Danh sách sách
            if (taiKhoan.VaiTro == "Admin")
            {
                return RedirectToAction("Dashboard", "ThongKe");
            }
            return RedirectToAction("Index", "Sach");
        }

        // GET: /TaiKhoan/DangXuat
        public IActionResult DangXuat()
        {
            // Xóa toàn bộ thông tin đăng nhập khỏi Session
            HttpContext.Session.Clear();
            TempData["ThongBao"] = "Bạn đã đăng xuất khỏi hệ thống.";
            return RedirectToAction("DangNhap");
        }

        // GET: /TaiKhoan/KhongCoQuyen
        public IActionResult KhongCoQuyen()
        {
            return View();
        }

        // GET: /TaiKhoan/Index (Chức năng Admin quản lý danh sách tài khoản)
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

        // POST: /TaiKhoan/DoiTrangThai/5 (Admin khóa hoặc mở khóa tài khoản)
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

            // Không cho tự khóa tài khoản Admin đang đăng nhập
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
