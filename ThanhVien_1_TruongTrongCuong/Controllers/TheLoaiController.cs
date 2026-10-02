using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    public class TheLoaiController : Controller
    {
        private readonly ThuVienDbContext _context;

        public TheLoaiController(ThuVienDbContext context)
        {
            _context = context;
        }

        private bool LaAdmin()
        {
            return HttpContext.Session.GetString("VaiTro") == "Admin";
        }

        public IActionResult Index()
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            var danhSach = _context.TheLoais
                .Include(tl => tl.Sachs)
                .OrderBy(tl => tl.MaTheLoai)
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

            var theLoai = _context.TheLoais
                .Include(tl => tl.Sachs)
                .FirstOrDefault(tl => tl.MaTheLoai == id);

            if (theLoai == null) return NotFound();

            return View(theLoai);
        }

        public IActionResult Create()
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }
            return View(new TheLoai());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TheLoai theLoai)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (!string.IsNullOrWhiteSpace(theLoai.TenTheLoai))
            {
                bool daTonTai = _context.TheLoais
                    .Any(tl => tl.TenTheLoai.ToLower() == theLoai.TenTheLoai.Trim().ToLower());
                if (daTonTai)
                {
                    ModelState.AddModelError("TenTheLoai", "Tên thể loại này đã tồn tại trong hệ thống!");
                }
            }

            if (ModelState.IsValid)
            {
                theLoai.TenTheLoai = theLoai.TenTheLoai.Trim();
                _context.TheLoais.Add(theLoai);
                _context.SaveChanges();
                TempData["ThongBao"] = $"Đã thêm thể loại '{theLoai.TenTheLoai}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(theLoai);
        }

        public IActionResult Edit(int? id)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (id == null) return NotFound();

            var theLoai = _context.TheLoais.Find(id);
            if (theLoai == null) return NotFound();

            return View(theLoai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TheLoai theLoai)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (id != theLoai.MaTheLoai) return NotFound();

            if (!string.IsNullOrWhiteSpace(theLoai.TenTheLoai))
            {
                bool daTonTai = _context.TheLoais
                    .Any(tl => tl.MaTheLoai != id && tl.TenTheLoai.ToLower() == theLoai.TenTheLoai.Trim().ToLower());
                if (daTonTai)
                {
                    ModelState.AddModelError("TenTheLoai", "Tên thể loại này đã tồn tại ở mã thể loại khác!");
                }
            }

            if (ModelState.IsValid)
            {
                theLoai.TenTheLoai = theLoai.TenTheLoai.Trim();
                _context.TheLoais.Update(theLoai);
                _context.SaveChanges();
                TempData["ThongBao"] = $"Đã cập nhật thể loại '{theLoai.TenTheLoai}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(theLoai);
        }

        public IActionResult Delete(int? id)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (id == null) return NotFound();

            var theLoai = _context.TheLoais
                .Include(tl => tl.Sachs)
                .FirstOrDefault(tl => tl.MaTheLoai == id);

            if (theLoai == null) return NotFound();

            return View(theLoai);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            var theLoai = _context.TheLoais
                .Include(tl => tl.Sachs)
                .FirstOrDefault(tl => tl.MaTheLoai == id);

            if (theLoai == null) return NotFound();

            if (theLoai.Sachs.Any())
            {
                TempData["Loi"] = $"Không thể xóa thể loại '{theLoai.TenTheLoai}' vì đang có {theLoai.Sachs.Count} đầu sách thuộc thể loại này! Vui lòng chuyển trạng thái sang 'Ngừng sử dụng'.";
                return RedirectToAction(nameof(Index));
            }

            _context.TheLoais.Remove(theLoai);
            _context.SaveChanges();
            TempData["ThongBao"] = $"Đã xóa thể loại '{theLoai.TenTheLoai}' thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
