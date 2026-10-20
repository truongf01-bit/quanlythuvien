using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class Sach
    {
        [Key]
        [Display(Name = "Mã sách")]
        public int MaSach { get; set; }

        [Required(ErrorMessage = "Tên sách không được để trống")]
        [StringLength(200, ErrorMessage = "Tên sách tối đa 200 ký tự")]
        [Display(Name = "Tên sách")]
        public string TenSach { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn thể loại sách")]
        [Display(Name = "Thể loại")]
        public int MaTheLoai { get; set; }

        [Required(ErrorMessage = "Tác giả không được để trống")]
        [StringLength(150, ErrorMessage = "Tên tác giả tối đa 150 ký tự")]
        [Display(Name = "Tác giả")]
        public string TacGia { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nhà xuất bản không được để trống")]
        [StringLength(150, ErrorMessage = "Nhà xuất bản tối đa 150 ký tự")]
        [Display(Name = "Nhà xuất bản")]
        public string NhaXuatBan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập năm xuất bản")]
        [Range(1900, 2026, ErrorMessage = "Năm xuất bản phải từ 1900 đến 2026")]
        [Display(Name = "Năm xuất bản")]
        public int NamXuatBan { get; set; } = 2024;

        [Required(ErrorMessage = "Vui lòng nhập tổng số lượng")]
        [Range(0, 10000, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Tổng số lượng")]
        public int SoLuong { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng còn")]
        [Range(0, 10000, ErrorMessage = "Số lượng còn phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Số lượng còn")]
        public int SoLuongCon { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá sách")]
        [Range(1000, 100000000, ErrorMessage = "Giá sách phải lớn hơn 0 (tối thiểu 1.000 VNĐ)")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá sách (VNĐ)")]
        public decimal GiaSach { get; set; }

        [StringLength(1000, ErrorMessage = "Mô tả tối đa 1000 ký tự")]
        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [StringLength(500, ErrorMessage = "Đường dẫn ảnh tối đa 500 ký tự")]
        [Display(Name = "Ảnh bìa sách")]
        public string? HinhAnh { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái phục vụ")]
        [StringLength(50)]
        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "Đang phục vụ";

        [ForeignKey("MaTheLoai")]
        public TheLoai? TheLoai { get; set; }

        public ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();
    }
}
