using System.ComponentModel.DataAnnotations;

namespace QuanLyThuVien.Models
{
    public class TaiKhoan
    {
        [Key]
        [Display(Name = "Mã tài khoản")]
        public int MaTaiKhoan { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3 đến 50 ký tự")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Mật khẩu phải từ 4 đến 100 ký tự")]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ và tên tối đa 100 ký tự")]
        [Display(Name = "Họ và tên")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn vai trò")]
        [StringLength(20)]
        [Display(Name = "Vai trò")]
        public string VaiTro { get; set; } = "DocGia";

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        [StringLength(30)]
        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "Hoạt động";

        public DocGia? DocGia { get; set; }
    }
}
