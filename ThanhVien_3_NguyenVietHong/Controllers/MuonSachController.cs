using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using QuanLyThuVien.ViewModels;

namespace QuanLyThuVien.Controllers
{
    public class MuonSachController : Controller
    {
        private readonly ThuVienDbContext _context;

        public MuonSachController(ThuVienDbContext context)
        {
            _context = context;
        }

        private DocGia? LayDocGiaHienTai()
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            if (maTaiKhoan == null) return null;

            return _context.DocGias
                .Include(dg => dg.TaiKhoan)
                .FirstOrDefault(dg => dg.MaTaiKhoan == maTaiKhoan.Value);
        }

        [HttpGet]
        public IActionResult DangKyMuon(int? maSach)
        {
            var docGia = LayDocGiaHienTai();
            if (docGia == null)
            {
                TempData["Loi"] = "Vui lòng đăng nhập bằng tài khoản Độc giả để đăng ký mượn sách!";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            if (docGia.TrangThai != "Hoạt động" || docGia.TaiKhoan?.TrangThai != "Hoạt động")
            {
                TempData["Loi"] = "Tài khoản độc giả của bạn đang bị khóa, không thể đăng ký mượn sách!";
                return RedirectToAction("Index", "Sach");
            }

            var vm = new DangKyMuonViewModel
            {
                NgayMuon = DateTime.Today,
                HanTra = DateTime.Today.AddDays(14),
                DocGiaHienTai = docGia,
                DanhSachSachPhucVu = _context.Sachs
                    .Include(s => s.TheLoai)
                    .Where(s => s.TrangThai == "Đang phục vụ" && s.SoLuongCon > 0)
                    .OrderBy(s => s.TenSach)
                    .ToList()
            };

            if (maSach.HasValue)
            {
                vm.DanhSachChon[0].MaSach = maSach.Value;
                vm.DanhSachChon[0].SoLuongMuon = 1;
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangKyMuon(DangKyMuonViewModel vm)
        {
            var docGia = LayDocGiaHienTai();
            if (docGia == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            vm.DocGiaHienTai = docGia;
            vm.DanhSachSachPhucVu = _context.Sachs
                .Include(s => s.TheLoai)
                .Where(s => s.TrangThai == "Đang phục vụ" && s.SoLuongCon > 0)
                .OrderBy(s => s.TenSach)
                .ToList();

            if (docGia.TrangThai != "Hoạt động" || docGia.TaiKhoan?.TrangThai != "Hoạt động")
            {
                ModelState.AddModelError("", "Hồ sơ độc giả của bạn đang bị khóa, không thể mượn sách!");
            }

            DateTime ngayMuon = DateTime.Today;
            if (vm.HanTra.Date <= ngayMuon)
            {
                ModelState.AddModelError("HanTra", "Hạn trả phải lớn hơn ngày đăng ký mượn hôm nay!");
            }
            else if ((vm.HanTra.Date - ngayMuon).TotalDays > 30)
            {
                ModelState.AddModelError("HanTra", "Thời hạn mượn tối đa là 30 ngày!");
            }

            var cacDongChon = vm.DanhSachChon
                .Where(x => x.MaSach.HasValue && x.MaSach.Value > 0)
                .ToList();

            if (!cacDongChon.Any())
            {
                ModelState.AddModelError("", "Vui lòng chọn ít nhất 01 đầu sách để đăng ký mượn!");
            }

            var maSachTrungTrongPhieu = cacDongChon
                .GroupBy(x => x.MaSach!.Value)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (maSachTrungTrongPhieu.Any())
            {
                ModelState.AddModelError("", "Không được chọn trùng cùng một đầu sách trên nhiều dòng! Vui lòng tăng số lượng mượn ở cùng 1 dòng.");
            }

            foreach (var item in cacDongChon)
            {
                int maSach = item.MaSach!.Value;
                var sach = _context.Sachs.Find(maSach);

                if (sach == null)
                {
                    ModelState.AddModelError("", $"Mã sách #{maSach} không tồn tại trong hệ thống!");
                    continue;
                }

                if (sach.TrangThai != "Đang phục vụ")
                {
                    ModelState.AddModelError("", $"Sách '{sach.TenSach}' hiện đang ở trạng thái '{sach.TrangThai}', không thể mượn!");
                }

                if (item.SoLuongMuon <= 0)
                {
                    ModelState.AddModelError("", $"Số lượng mượn của sách '{sach.TenSach}' phải lớn hơn 0!");
                }
                else if (item.SoLuongMuon > sach.SoLuongCon)
                {
                    ModelState.AddModelError("", $"Sách '{sach.TenSach}' chỉ còn {sach.SoLuongCon} cuốn, không đủ số lượng yêu cầu ({item.SoLuongMuon} cuốn)!");
                }

                bool dangMuonSachNay = _context.ChiTietPhieuMuons
                    .Include(ct => ct.PhieuMuon)
                    .Any(ct => ct.MaSach == maSach &&
                               ct.PhieuMuon!.MaDocGia == docGia.MaDocGia &&
                               (ct.PhieuMuon.TrangThai == "Chờ xác nhận" || ct.PhieuMuon.TrangThai == "Đang mượn"));

                if (dangMuonSachNay)
                {
                    ModelState.AddModelError("", $"Bạn đã có phiếu đang mượn hoặc đang chờ xác nhận cho sách '{sach.TenSach}'. Không thể tạo yêu cầu trùng!");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var phieuMoi = new PhieuMuon
            {
                MaDocGia = docGia.MaDocGia,
                NgayMuon = ngayMuon,
                HanTra = vm.HanTra.Date,
                TrangThai = "Chờ xác nhận"
            };

            _context.PhieuMuons.Add(phieuMoi);
            _context.SaveChanges();

            foreach (var item in cacDongChon)
            {
                var chiTiet = new ChiTietPhieuMuon
                {
                    MaPhieuMuon = phieuMoi.MaPhieuMuon,
                    MaSach = item.MaSach!.Value,
                    SoLuongMuon = item.SoLuongMuon,
                    NgayTra = null,
                    TinhTrangTra = "Chưa trả"
                };
                _context.ChiTietPhieuMuons.Add(chiTiet);
            }
            _context.SaveChanges();

            TempData["ThongBao"] = $"Đã gửi yêu cầu đăng ký mượn sách thành công (Mã phiếu: #{phieuMoi.MaPhieuMuon})! Vui lòng chờ Admin xác nhận.";
            return RedirectToAction(nameof(SachDangMuon));
        }

        public IActionResult SachDangMuon()
        {
            var docGia = LayDocGiaHienTai();
            if (docGia == null)
            {
                TempData["Loi"] = "Vui lòng đăng nhập để xem sách đang mượn!";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var danhSach = _context.PhieuMuons
                .Include(pm => pm.ChiTietPhieuMuons)
                    .ThenInclude(ct => ct.Sach)
                .Where(pm => pm.MaDocGia == docGia.MaDocGia &&
                            (pm.TrangThai == "Chờ xác nhận" || pm.TrangThai == "Đang mượn"))
                .OrderByDescending(pm => pm.NgayMuon)
                .ThenByDescending(pm => pm.MaPhieuMuon)
                .ToList();

            return View(danhSach);
        }

        public IActionResult LichSuMuonTra()
        {
            var docGia = LayDocGiaHienTai();
            if (docGia == null)
            {
                TempData["Loi"] = "Vui lòng đăng nhập để xem lịch sử mượn trả!";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var lichSu = _context.PhieuMuons
                .Include(pm => pm.ChiTietPhieuMuons)
                    .ThenInclude(ct => ct.Sach)
                .Where(pm => pm.MaDocGia == docGia.MaDocGia)
                .OrderByDescending(pm => pm.NgayMuon)
                .ThenByDescending(pm => pm.MaPhieuMuon)
                .ToList();

            return View(lichSu);
        }

        public IActionResult ChiTietPhieu(int? id)
        {
            var docGia = LayDocGiaHienTai();
            if (docGia == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            if (id == null) return NotFound();

            var phieu = _context.PhieuMuons
                .Include(pm => pm.DocGia)
                .Include(pm => pm.ChiTietPhieuMuons)
                    .ThenInclude(ct => ct.Sach)
                        .ThenInclude(s => s!.TheLoai)
                .Include(pm => pm.GiaoDichThanhToans)
                .FirstOrDefault(pm => pm.MaPhieuMuon == id && pm.MaDocGia == docGia.MaDocGia);

            if (phieu == null)
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            return View(phieu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HuyYeuCau(int id)
        {
            var docGia = LayDocGiaHienTai();
            if (docGia == null) return RedirectToAction("DangNhap", "TaiKhoan");

            var phieu = _context.PhieuMuons
                .Include(pm => pm.ChiTietPhieuMuons)
                .FirstOrDefault(pm => pm.MaPhieuMuon == id && pm.MaDocGia == docGia.MaDocGia);

            if (phieu == null) return RedirectToAction("KhongCoQuyen", "TaiKhoan");

            if (phieu.TrangThai != "Chờ xác nhận")
            {
                TempData["Loi"] = "Chỉ có thể hủy yêu cầu khi phiếu đang ở trạng thái 'Chờ xác nhận'!";
                return RedirectToAction(nameof(SachDangMuon));
            }

            phieu.TrangThai = "Đã hủy";
            foreach (var ct in phieu.ChiTietPhieuMuons)
            {
                ct.TinhTrangTra = "Đã hủy";
            }
            _context.SaveChanges();

            TempData["ThongBao"] = $"Đã hủy yêu cầu phiếu mượn #{phieu.MaPhieuMuon} thành công.";
            return RedirectToAction(nameof(LichSuMuonTra));
        }
    }
}
