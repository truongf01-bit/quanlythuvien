using System.ComponentModel.DataAnnotations;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.ViewModels
{
    public class SachMuonItem
    {
        public int? MaSach { get; set; }
        public int SoLuongMuon { get; set; } = 1;
    }

    public class DangKyMuonViewModel
    {
        [DataType(DataType.Date)]
        [Display(Name = "Ngày đăng ký mượn")]
        public DateTime NgayMuon { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Vui lòng chọn hạn trả dự kiến")]
        [DataType(DataType.Date)]
        [Display(Name = "Hạn trả dự kiến")]
        public DateTime HanTra { get; set; } = DateTime.Today.AddDays(14);

        public List<SachMuonItem> DanhSachChon { get; set; } = new List<SachMuonItem>
        {
            new SachMuonItem { SoLuongMuon = 1 },
            new SachMuonItem { SoLuongMuon = 1 },
            new SachMuonItem { SoLuongMuon = 1 }
        };

        public DocGia? DocGiaHienTai { get; set; }
        public List<Sach> DanhSachSachPhucVu { get; set; } = new List<Sach>();
    }
}
