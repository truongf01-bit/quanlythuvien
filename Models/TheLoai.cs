using System.ComponentModel.DataAnnotations;

namespace QuanLyThuVien.Models
{
    public class TheLoai
    {
        [Key]
        [Display(Name = "Mã thể loại")]
        public int MaTheLoai { get; set; }

        [Required(ErrorMessage = "Tên thể loại bắt buộc nhập")]
        [StringLength(100, ErrorMessage = "Tên thể loại tối đa 100 ký tự")]
        [Display(Name = "Tên thể loại")]
        public string TenTheLoai { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Mô tả tối đa 500 ký tự")]
        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        [StringLength(50)]
        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "Đang sử dụng";

        public ICollection<Sach> Sachs { get; set; } = new List<Sach>();
    }
}
