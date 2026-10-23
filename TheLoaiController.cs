// Họ và tên: Nguyễn Văn An
// Mã sinh viên: 22103100001
// Nội dung thực hiện: Module 1 - Quản lý thể loại sách (CRUD, kiểm tra trùng tên, ràng buộc xóa)

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

        // Hàm kiểm tra quyền Admin tại Controller (Mục 5.4)
        private bool LaAdmin()
        {
            return HttpContext.Session.GetString("VaiTro") == "Admin";
        }

        // GET: /TheLoai
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

        // GET: /TheLoai/Details/5
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

        // GET: /TheLoai/Create
        public IActionResult Create()
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }
            return View(new TheLoai());
        }

        // POST: /TheLoai/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TheLoai theLoai)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            // Kiểm tra nghiệp vụ: Tên thể loại không được trùng (dùng LINQ)
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

        // GET: /TheLoai/Edit/5
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

        // POST: /TheLoai/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TheLoai theLoai)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            if (id != theLoai.MaTheLoai) return NotFound();

            // Kiểm tra trùng tên thể loại với các bản ghi khác
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

        // GET: /TheLoai/Delete/5
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

        // POST: /TheLoai/Delete/5
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

            // Mục 5.5: Không được xóa thể loại nếu đang có sách liên quan
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
