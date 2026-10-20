using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using QuanLyThuVien.ViewModels;

namespace QuanLyThuVien.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly ThuVienDbContext _context;

        public ThanhToanController(ThuVienDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int? maPhieuMuon, string? loai, decimal? soTien)
        {
            var vm = new ThanhToanViewModel();

            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            DocGia? docGiaHienTai = null;

            if (maTaiKhoan != null)
            {
                docGiaHienTai = _context.DocGias
                    .Include(dg => dg.TaiKhoan)
                    .FirstOrDefault(dg => dg.MaTaiKhoan == maTaiKhoan.Value);

                if (docGiaHienTai != null)
                {
                    vm.MaDocGia = docGiaHienTai.MaDocGia;
                    vm.HoTen = docGiaHienTai.HoTen;
                    vm.SoDienThoai = docGiaHienTai.SoDienThoai;
                }
            }

            if (maPhieuMuon.HasValue)
            {
                var phieuMuon = _context.PhieuMuons
                    .Include(pm => pm.DocGia)
                    .Include(pm => pm.ChiTietPhieuMuons)
                        .ThenInclude(ct => ct.Sach)
                    .FirstOrDefault(pm => pm.MaPhieuMuon == maPhieuMuon.Value);

                if (phieuMuon != null)
                {
                    vm.MaPhieuMuon = phieuMuon.MaPhieuMuon;
                    if (string.IsNullOrEmpty(vm.HoTen) && phieuMuon.DocGia != null)
                    {
                        vm.HoTen = phieuMuon.DocGia.HoTen;
                        vm.MaDocGia = phieuMuon.DocGia.MaDocGia;
                        vm.SoDienThoai = phieuMuon.DocGia.SoDienThoai;
                    }

                    if (phieuMuon.TrangThai == "Đang mượn" && phieuMuon.HanTra < DateTime.Today)
                    {
                        int soNgayTre = (DateTime.Today - phieuMuon.HanTra).Days;
                        vm.LoaiThanhToan = "PhatQuaHan";
                        vm.SoTien = soNgayTre * 5000;
                        vm.NoiDungChuyenKhoan = $"UNETI PHAT PM{phieuMuon.MaPhieuMuon} DG{vm.MaDocGia}";
                    }
                    else
                    {
                        vm.LoaiThanhToan = "TienCoc";
                        vm.SoTien = 100000;
                        vm.NoiDungChuyenKhoan = $"UNETI COC PM{phieuMuon.MaPhieuMuon} DG{vm.MaDocGia}";
                    }
                }
            }

            if (!string.IsNullOrEmpty(loai))
            {
                vm.LoaiThanhToan = loai;
            }

            if (soTien.HasValue && soTien.Value > 0)
            {
                vm.SoTien = soTien.Value;
            }

            if (string.IsNullOrEmpty(vm.NoiDungChuyenKhoan))
            {
                string tag = vm.LoaiThanhToan switch
                {
                    "PhiThe" => "THE",
                    "PhatQuaHan" => "PHAT",
                    "TienCoc" => "COC",
                    _ => "PHI"
                };

                string maIdentifier = vm.MaDocGia.HasValue ? $"DG{vm.MaDocGia}" : "SV";
                vm.NoiDungChuyenKhoan = $"UNETI {tag} {maIdentifier} {DateTime.Now:ddMMHHmm}";
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XacNhan(ThanhToanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            ViewBag.MaGiaoDich = "GD" + DateTime.Now.ToString("yyyyMMddHHmmss");
            ViewBag.ThoiGian = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            TempData["ThongBao"] = "Xác nhận yêu cầu thanh toán thành công! Thủ thư sẽ kiểm tra và đối soát giao dịch trong ít phút.";
            return View("ThanhCong", model);
        }
    }
}
