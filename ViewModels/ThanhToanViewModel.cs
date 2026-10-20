using System.ComponentModel.DataAnnotations;

namespace QuanLyThuVien.ViewModels
{
    public class ThanhToanViewModel
    {
        [Display(Name = "Mã phiếu mượn (nếu có)")]
        public int? MaPhieuMuon { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại phí")]
        [Display(Name = "Loại phí thanh toán")]
        public string LoaiThanhToan { get; set; } = "TienCoc";

        [Required(ErrorMessage = "Số tiền không được để trống")]
        [Range(1000, 10000000, ErrorMessage = "Số tiền thanh toán từ 1.000đ đến 10.000.000đ")]
        [Display(Name = "Số tiền (VNĐ)")]
        public decimal SoTien { get; set; } = 50000;

        [Required(ErrorMessage = "Vui lòng nhập họ và tên người nộp")]
        [Display(Name = "Họ và tên sinh viên")]
        public string HoTen { get; set; } = string.Empty;

        [Display(Name = "Mã độc giả")]
        public int? MaDocGia { get; set; }

        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; } = string.Empty;

        [Display(Name = "Nội dung chuyển khoản")]
        public string NoiDungChuyenKhoan { get; set; } = string.Empty;

        [Display(Name = "Phương thức thanh toán")]
        public string PhuongThuc { get; set; } = "MBBank";

        public string TenNguoiThuHuong { get; set; } = "NGUYEN XUAN TRUONG";
        public string SoTaiKhoanMB { get; set; } = "21611611116";
        public string NganHang { get; set; } = "MB Bank (Ngân hàng TMCP Quân Đội)";
        public string MoMoSDT { get; set; } = "09******498";
    }
}
