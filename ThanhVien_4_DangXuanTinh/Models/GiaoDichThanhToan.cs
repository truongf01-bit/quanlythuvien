using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class GiaoDichThanhToan
    {
        [Key]
        [Display(Name = "Mã giao dịch")]
        public int MaGiaoDich { get; set; }

        [Required]
        [Display(Name = "Mã chứng từ / GD")]
        [StringLength(50)]
        public string MaGiaoDichCode { get; set; } = string.Empty;

        [Display(Name = "Mã phiếu mượn")]
        public int? MaPhieuMuon { get; set; }

        [Display(Name = "Mã độc giả")]
        public int? MaDocGia { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hướng dòng tiền")]
        [Display(Name = "Hướng dòng tiền")]
        [StringLength(30)]
        public string HuongDongTien { get; set; } = "Nguồn vào";

        [Required(ErrorMessage = "Vui lòng chọn loại phí / mục đích")]
        [Display(Name = "Loại thanh toán / Mục đích")]
        [StringLength(100)]
        public string LoaiThanhToan { get; set; } = "Tiền cọc";

        [Required(ErrorMessage = "Số tiền không được để trống")]
        [Range(1000, 100000000, ErrorMessage = "Số tiền từ 1.000đ đến 100.000.000đ")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Số tiền (VNĐ)")]
        public decimal SoTien { get; set; }

        [Required(ErrorMessage = "Vui lòng chỉ định nguồn ra của tiền")]
        [Display(Name = "Nguồn ra của tiền (Tiền trích từ)")]
        [StringLength(150)]
        public string NguonTienRa { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chỉ định nguồn vào của tiền")]
        [Display(Name = "Nguồn vào của tiền (Tiền nhận vào)")]
        [StringLength(150)]
        public string NguonTienVao { get; set; } = string.Empty;

        [Display(Name = "Phương thức")]
        [StringLength(50)]
        public string PhuongThuc { get; set; } = "Chuyển khoản QR (MB Bank)";

        [Display(Name = "Người thực hiện / nộp")]
        [StringLength(100)]
        public string? NguoiThucHien { get; set; }

        [Display(Name = "Số điện thoại")]
        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        [Display(Name = "Thông tin tài khoản đối ứng (STK/Ngân hàng)")]
        [StringLength(255)]
        public string? ThongTinTaiKhoanDoiUng { get; set; }

        [Display(Name = "Nội dung chuyển khoản / Ghi chú")]
        [StringLength(255)]
        public string? NoiDung { get; set; }

        [Display(Name = "Thời gian giao dịch")]
        public DateTime NgayGiaoDich { get; set; } = DateTime.Now;

        [Display(Name = "Trạng thái giao dịch")]
        [StringLength(50)]
        public string TrangThai { get; set; } = "Đã hoàn thành";

        [ForeignKey("MaPhieuMuon")]
        public PhieuMuon? PhieuMuon { get; set; }

        [ForeignKey("MaDocGia")]
        public DocGia? DocGia { get; set; }
    }
}
