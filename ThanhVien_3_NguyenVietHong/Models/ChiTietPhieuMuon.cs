using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class ChiTietPhieuMuon
    {
        [Required]
        [Display(Name = "Mã phiếu mượn")]
        public int MaPhieuMuon { get; set; }

        [Required]
        [Display(Name = "Sách")]
        public int MaSach { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng mượn")]
        [Range(1, 20, ErrorMessage = "Số lượng mượn phải lớn hơn 0")]
        [Display(Name = "Số lượng mượn")]
        public int SoLuongMuon { get; set; } = 1;

        [DataType(DataType.Date)]
        [Display(Name = "Ngày trả thực tế")]
        public DateTime? NgayTra { get; set; }

        [StringLength(100)]
        [Display(Name = "Tình trạng trả")]
        public string? TinhTrangTra { get; set; } = "Chưa trả";

        [ForeignKey("MaPhieuMuon")]
        public PhieuMuon? PhieuMuon { get; set; }

        [ForeignKey("MaSach")]
        public Sach? Sach { get; set; }
    }
}
