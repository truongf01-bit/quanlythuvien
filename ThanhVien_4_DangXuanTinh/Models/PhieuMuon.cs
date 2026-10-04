using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class PhieuMuon
    {
        [Key]
        [Display(Name = "Mã phiếu mượn")]
        public int MaPhieuMuon { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn độc giả")]
        [Display(Name = "Độc giả")]
        public int MaDocGia { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày mượn")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày mượn")]
        public DateTime NgayMuon { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Vui lòng chọn hạn trả")]
        [DataType(DataType.Date)]
        [Display(Name = "Hạn trả")]
        public DateTime HanTra { get; set; } = DateTime.Today.AddDays(14);

        [Required(ErrorMessage = "Vui lòng chọn trạng thái phiếu")]
        [StringLength(50)]
        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "Chờ xác nhận";

        [ForeignKey("MaDocGia")]
        public DocGia? DocGia { get; set; }

        public ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();

        public ICollection<GiaoDichThanhToan> GiaoDichThanhToans { get; set; } = new List<GiaoDichThanhToan>();
    }
}
