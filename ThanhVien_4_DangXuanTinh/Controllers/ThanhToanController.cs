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

        private bool LaAdmin()
        {
            return HttpContext.Session.GetString("VaiTro") == "Admin";
        }

        [HttpGet]
        public IActionResult Index(int? maPhieuMuon, string? loai, decimal? soTien, string? huong)
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            if (maTaiKhoan == null)
            {
                TempData["Loi"] = "Bạn cần đăng nhập tài khoản trước khi thực hiện thanh toán!";
                string returnUrl = Url.Action("Index", "ThanhToan", new { maPhieuMuon, loai, soTien, huong }) ?? "/ThanhToan";
                return RedirectToAction("DangNhap", "TaiKhoan", new { returnUrl });
            }

            var vm = new ThanhToanViewModel();

            var docGiaHienTai = _context.DocGias
                .Include(dg => dg.TaiKhoan)
                .FirstOrDefault(dg => dg.MaTaiKhoan == maTaiKhoan.Value);

            if (docGiaHienTai != null)
            {
                vm.MaDocGia = docGiaHienTai.MaDocGia;
                vm.HoTen = docGiaHienTai.HoTen;
                vm.SoDienThoai = docGiaHienTai.SoDienThoai;
            }

            // Thiết lập hướng dòng tiền & nguồn ra / nguồn vào mặc định
            bool laNguonRa = (!string.IsNullOrEmpty(huong) && huong.Equals("NguonRa", StringComparison.OrdinalIgnoreCase))
                             || (!string.IsNullOrEmpty(loai) && loai.Equals("HoanCoc", StringComparison.OrdinalIgnoreCase));

            if (laNguonRa)
            {
                vm.HuongDongTien = "Nguồn ra";
                vm.LoaiThanhToan = "HoanCoc";
                vm.NguonTienRa = "Quỹ tiền mặt quầy thủ thư";
                vm.NguonTienVao = "Tiền mặt độc giả nhận tại quầy";
                vm.SoTien = 100000;
            }
            else
            {
                vm.HuongDongTien = "Nguồn vào";
                vm.LoaiThanhToan = "TienCoc";
                vm.NguonTienRa = "Tài khoản cá nhân độc giả (Chuyển khoản QR)";
                vm.NguonTienVao = "Tài khoản MB Bank thư viện (21611611116 - NGUYEN XUAN TRUONG)";
                vm.SoTien = 100000;
            }

            if (maPhieuMuon.HasValue)
            {
                var phieuMuon = _context.PhieuMuons
                    .Include(pm => pm.DocGia)
                    .Include(pm => pm.ChiTietPhieuMuons)
                        .ThenInclude(ct => ct.Sach)
                    .Include(pm => pm.GiaoDichThanhToans)
                    .FirstOrDefault(pm => pm.MaPhieuMuon == maPhieuMuon.Value);

                if (phieuMuon != null)
                {
                    vm.MaPhieuMuon = phieuMuon.MaPhieuMuon;
                    if (phieuMuon.DocGia != null)
                    {
                        vm.HoTen = phieuMuon.DocGia.HoTen;
                        vm.MaDocGia = phieuMuon.DocGia.MaDocGia;
                        vm.SoDienThoai = phieuMuon.DocGia.SoDienThoai;
                    }

                    // Nếu yêu cầu là Nguồn ra (Hoàn cọc) hoặc phiếu đã trả sách
                    if (laNguonRa || phieuMuon.TrangThai == "Đã trả")
                    {
                        vm.HuongDongTien = "Nguồn ra";
                        vm.LoaiThanhToan = "HoanCoc";
                        vm.NguonTienRa = "Quỹ tiền mặt quầy thủ thư";
                        vm.NguonTienVao = "Tiền mặt độc giả nhận tại quầy";

                        // Tính số tiền cọc đã nộp trước đó nếu có
                        var cocDaNop = phieuMuon.GiaoDichThanhToans
                            .Where(g => g.HuongDongTien == "Nguồn vào" && (g.LoaiThanhToan.Contains("cọc") || g.LoaiThanhToan.Contains("Coc")))
                            .Sum(g => g.SoTien);

                        vm.SoTien = cocDaNop > 0 ? cocDaNop : 100000;
                        vm.NoiDungChuyenKhoan = $"UNETI HOANCOC PM{phieuMuon.MaPhieuMuon} DG{vm.MaDocGia}";
                    }
                    else if (phieuMuon.TrangThai == "Đang mượn" && phieuMuon.HanTra < DateTime.Today)
                    {
                        int soNgayTre = (DateTime.Today - phieuMuon.HanTra).Days;
                        vm.HuongDongTien = "Nguồn vào";
                        vm.LoaiThanhToan = "PhatQuaHan";
                        vm.SoTien = soNgayTre * 5000;
                        vm.NoiDungChuyenKhoan = $"UNETI PHAT PM{phieuMuon.MaPhieuMuon} DG{vm.MaDocGia}";
                    }
                    else
                    {
                        vm.HuongDongTien = "Nguồn vào";
                        vm.LoaiThanhToan = "TienCoc";
                        vm.SoTien = 100000;
                        vm.NoiDungChuyenKhoan = $"UNETI COC PM{phieuMuon.MaPhieuMuon} DG{vm.MaDocGia}";
                    }
                }
            }

            if (!string.IsNullOrEmpty(loai))
            {
                vm.LoaiThanhToan = loai;
                if (loai == "HoanCoc")
                {
                    vm.HuongDongTien = "Nguồn ra";
                    vm.NguonTienRa = "Quỹ tiền mặt quầy thủ thư";
                    vm.NguonTienVao = "Tiền mặt độc giả nhận tại quầy";
                }
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
                    "HoanCoc" => "HOANCOC",
                    "BoiThuong" => "BOITHUONG",
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
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            if (maTaiKhoan == null)
            {
                TempData["Loi"] = "Bạn cần đăng nhập tài khoản trước khi thực hiện thanh toán!";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // Xác định tên hiển thị của loại thanh toán
            string tenLoaiThanhToan = model.LoaiThanhToan switch
            {
                "TienCoc" => "Tiền cọc mượn sách",
                "PhatQuaHan" => "Phạt trễ hạn trả sách",
                "HoanCoc" => "Hoàn trả tiền cọc",
                "BoiThuong" => "Bồi thường hư hỏng / mất sách",
                "PhiThe" => "Phí làm thẻ thư viện",
                _ => "Phí dịch vụ thư viện khác"
            };

            // Sinh mã giao dịch duy nhất
            string prefix = model.HuongDongTien == "Nguồn ra" ? "CHI" : "THU";
            string maGiaoDichCode = $"{prefix}{DateTime.Now:yyyyMMddHHmmss}" + (model.MaPhieuMuon.HasValue ? $"_PM{model.MaPhieuMuon}" : "");

            string tenPhuongThuc = model.PhuongThuc switch
            {
                "MBBank" => "Chuyển khoản QR (MB Bank)",
                "MoMo" => "Ví MoMo",
                "TienMat" => "Tiền mặt tại quầy",
                _ => model.PhuongThuc
            };

            // Trạng thái giao dịch
            string trangThai = (model.PhuongThuc == "TienMat" || LaAdmin()) ? "Đã hoàn thành" : "Chờ đối soát";

            // Tạo và lưu bản ghi GiaoDichThanhToan vào Database
            var giaoDich = new GiaoDichThanhToan
            {
                MaGiaoDichCode = maGiaoDichCode,
                MaPhieuMuon = model.MaPhieuMuon,
                MaDocGia = model.MaDocGia,
                HuongDongTien = model.HuongDongTien,
                LoaiThanhToan = tenLoaiThanhToan,
                SoTien = model.SoTien,
                NguonTienRa = model.NguonTienRa,
                NguonTienVao = model.NguonTienVao,
                PhuongThuc = tenPhuongThuc,
                NguoiThucHien = model.HoTen,
                SoDienThoai = model.SoDienThoai,
                ThongTinTaiKhoanDoiUng = model.ThongTinTaiKhoanDoiUng,
                NoiDung = model.NoiDungChuyenKhoan,
                NgayGiaoDich = DateTime.Now,
                TrangThai = trangThai
            };

            _context.GiaoDichThanhToans.Add(giaoDich);
            _context.SaveChanges();

            model.MaGiaoDichCode = maGiaoDichCode;
            model.NgayGiaoDich = giaoDich.NgayGiaoDich;
            model.TrangThai = trangThai;

            ViewBag.MaGiaoDich = maGiaoDichCode;
            ViewBag.ThoiGian = giaoDich.NgayGiaoDich.ToString("dd/MM/yyyy HH:mm:ss");

            TempData["ThongBao"] = model.HuongDongTien == "Nguồn ra"
                ? $"Đã thực hiện xuất chi / hoàn tiền #{maGiaoDichCode} thành công! Số tiền {model.SoTien:N0} VNĐ đã được chuyển từ '{model.NguonTienRa}' sang '{model.NguonTienVao}'."
                : $"Giao dịch nộp tiền #{maGiaoDichCode} thành công! Dòng tiền đã được ghi nhận: trích từ '{model.NguonTienRa}' vào '{model.NguonTienVao}'.";

            return View("ThanhCong", model);
        }

        [HttpGet]
        public IActionResult LichSu(string? huongDongTien, string? loaiThanhToan, int? maPhieuMuon, string? tuKhoa, DateTime? tuNgay, DateTime? denNgay)
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            if (maTaiKhoan == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            bool laAdmin = LaAdmin();
            DocGia? docGia = null;

            if (!laAdmin)
            {
                docGia = _context.DocGias.FirstOrDefault(d => d.MaTaiKhoan == maTaiKhoan.Value);
                if (docGia == null)
                {
                    TempData["Loi"] = "Không tìm thấy hồ sơ độc giả liên kết!";
                    return RedirectToAction("Index", "Home");
                }
            }

            IQueryable<GiaoDichThanhToan> query = _context.GiaoDichThanhToans
                .Include(g => g.PhieuMuon)
                .Include(g => g.DocGia)
                .AsQueryable();

            if (!laAdmin && docGia != null)
            {
                query = query.Where(g => g.MaDocGia == docGia.MaDocGia);
            }

            if (!string.IsNullOrWhiteSpace(huongDongTien))
            {
                query = query.Where(g => g.HuongDongTien == huongDongTien);
            }

            if (!string.IsNullOrWhiteSpace(loaiThanhToan))
            {
                query = query.Where(g => g.LoaiThanhToan.Contains(loaiThanhToan));
            }

            if (maPhieuMuon.HasValue)
            {
                query = query.Where(g => g.MaPhieuMuon == maPhieuMuon.Value);
            }

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string kw = tuKhoa.Trim().ToLower();
                query = query.Where(g => (g.NguoiThucHien != null && g.NguoiThucHien.ToLower().Contains(kw))
                                      || g.MaGiaoDichCode.ToLower().Contains(kw)
                                      || (g.NoiDung != null && g.NoiDung.ToLower().Contains(kw))
                                      || g.NguonTienRa.ToLower().Contains(kw)
                                      || g.NguonTienVao.ToLower().Contains(kw));
            }

            if (tuNgay.HasValue)
            {
                query = query.Where(g => g.NgayGiaoDich.Date >= tuNgay.Value.Date);
            }

            if (denNgay.HasValue)
            {
                query = query.Where(g => g.NgayGiaoDich.Date <= denNgay.Value.Date);
            }

            var danhSach = query
                .OrderByDescending(g => g.NgayGiaoDich)
                .ThenByDescending(g => g.MaGiaoDich)
                .ToList();

            // Tính toán tổng hợp KPI dòng tiền
            decimal tongNguonVao = danhSach.Where(g => g.HuongDongTien == "Nguồn vào" && g.TrangThai != "Đã hủy").Sum(g => g.SoTien);
            decimal tongNguonRa = danhSach.Where(g => g.HuongDongTien == "Nguồn ra" && g.TrangThai != "Đã hủy").Sum(g => g.SoTien);
            decimal chenhLechQuy = tongNguonVao - tongNguonRa;
            int soChoDoiSoat = danhSach.Count(g => g.TrangThai == "Chờ đối soát");

            ViewBag.TongNguonVao = tongNguonVao;
            ViewBag.TongNguonRa = tongNguonRa;
            ViewBag.ChenhLechQuy = chenhLechQuy;
            ViewBag.SoChoDoiSoat = soChoDoiSoat;

            ViewBag.HuongDongTien = huongDongTien;
            ViewBag.LoaiThanhToan = loaiThanhToan;
            ViewBag.MaPhieuMuon = maPhieuMuon;
            ViewBag.TuKhoa = tuKhoa;
            ViewBag.TuNgay = tuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay?.ToString("yyyy-MM-dd");
            ViewBag.LaAdmin = laAdmin;

            return View(danhSach);
        }

        [HttpGet]
        public IActionResult ChiTiet(int id)
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            if (maTaiKhoan == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var gd = _context.GiaoDichThanhToans
                .Include(g => g.PhieuMuon)
                    .ThenInclude(pm => pm!.DocGia)
                .Include(g => g.DocGia)
                .FirstOrDefault(g => g.MaGiaoDich == id);

            if (gd == null)
            {
                return NotFound();
            }

            bool laAdmin = LaAdmin();
            if (!laAdmin)
            {
                var docGia = _context.DocGias.FirstOrDefault(d => d.MaTaiKhoan == maTaiKhoan.Value);
                if (docGia == null || gd.MaDocGia != docGia.MaDocGia)
                {
                    return RedirectToAction("KhongCoQuyen", "TaiKhoan");
                }
            }

            return View(gd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XacNhanDoiSoat(int id)
        {
            if (!LaAdmin())
            {
                return RedirectToAction("KhongCoQuyen", "TaiKhoan");
            }

            var gd = _context.GiaoDichThanhToans.Find(id);
            if (gd == null)
            {
                return NotFound();
            }

            gd.TrangThai = "Đã hoàn thành";
            _context.SaveChanges();

            TempData["ThongBao"] = $"Đã xác nhận đối soát giao dịch #{gd.MaGiaoDichCode} thành công!";
            return RedirectToAction(nameof(LichSu));
        }
    }
}
