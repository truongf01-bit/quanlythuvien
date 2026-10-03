using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    public class HomeController : Controller
    {
        private readonly ThuVienDbContext _context;

        public HomeController(ThuVienDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var sachMoiNhat = _context.Sachs
                .Include(s => s.TheLoai)
                .Where(s => s.TrangThai == "Đang phục vụ")
                .OrderByDescending(s => s.NamXuatBan)
                .ThenBy(s => s.TenSach)
                .Take(6)
                .ToList();

            ViewBag.TongDauSach = _context.Sachs.Count();
            ViewBag.TongTheLoai = _context.TheLoais.Count();
            ViewBag.TongDocGia = _context.DocGias.Count();
            ViewBag.DanhSachTheLoai = _context.TheLoais
                .Include(tl => tl.Sachs)
                .Where(tl => tl.TrangThai == "Đang sử dụng")
                .ToList();

            return View(sachMoiNhat);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
