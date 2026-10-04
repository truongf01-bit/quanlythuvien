using System.ComponentModel.DataAnnotations;

namespace QuanLyThuVien.ViewModels
{
    public class ThanhToanViewModel
    {
        [Display(Name = "Mã phiếu mượn (nếu có)")]
        public int? MaPhieuMuon { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hướng dòng tiền")]
        [Display(Name = "Hướng dòng tiền")]
        public string HuongDongTien { get; set; } = "Nguồn vào"; // "Nguồn vào" (Thu tiền) | "Nguồn ra" (Chi / Hoàn tiền)

        [Required(ErrorMessage = "Vui lòng chọn loại phí / mục đích")]
        [Display(Name = "Mục đích thanh toán")]
        public string LoaiThanhToan { get; set; } = "TienCoc"; // TienCoc, PhatQuaHan, HoanCoc, BoiThuong, PhiThe, Khac

        [Required(ErrorMessage = "Số tiền không được để trống")]
        [Range(1000, 100000000, ErrorMessage = "Số tiền thanh toán từ 1.000đ đến 100.000.000đ")]
        [Display(Name = "Số tiền (VNĐ)")]
        public decimal SoTien { get; set; } = 100000;

        [Required(ErrorMessage = "Vui lòng chỉ định nguồn ra của tiền")]
        [Display(Name = "Nguồn ra của tiền (Tiền trích từ)")]
        public string NguonTienRa { get; set; } = "Tài khoản cá nhân độc giả (Chuyển khoản QR)";

        [Required(ErrorMessage = "Vui lòng chỉ định nguồn vào của tiền")]
        [Display(Name = "Nguồn vào của tiền (Tiền nhận vào)")]
        public string NguonTienVao { get; set; } = "Tài khoản MB Bank thư viện (21611611116 - NGUYEN XUAN TRUONG)";

        [Required(ErrorMessage = "Vui lòng nhập họ và tên người nộp / nhận")]
        [Display(Name = "Họ và tên người thực hiện")]
        public string HoTen { get; set; } = string.Empty;

        [Display(Name = "Mã độc giả")]
        public int? MaDocGia { get; set; }

        [Display(Name = "Số điện thoại liên hệ")]
        public string SoDienThoai { get; set; } = string.Empty;

        [Display(Name = "Thông tin tài khoản ngân hàng độc giả (STK / Ngân hàng nhận)")]
        public string? ThongTinTaiKhoanDoiUng { get; set; }

        [Display(Name = "Nội dung chuyển khoản / Ghi chú")]
        public string NoiDungChuyenKhoan { get; set; } = string.Empty;

        [Display(Name = "Phương thức thanh toán")]
        public string PhuongThuc { get; set; } = "MBBank"; // MBBank, MoMo, TienMat

        [Display(Name = "Trạng thái giao dịch")]
        public string TrangThai { get; set; } = "Đã hoàn thành";

        public string? MaGiaoDichCode { get; set; }
        public DateTime? NgayGiaoDich { get; set; }

        // Cấu hình thụ hưởng của Thư viện UNETI
        public string TenNguoiThuHuong { get; set; } = "NGUYEN XUAN TRUONG";
        public string SoTaiKhoanMB { get; set; } = "21611611116";
        public string NganHang { get; set; } = "MB Bank (Ngân hàng TMCP Quân Đội)";
        public string MoMoSDT { get; set; } = "09******498";
    }
}
