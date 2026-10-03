using QuanLyThuVien.Models;

namespace QuanLyThuVien.ViewModels
{
    public class SachIndexViewModel
    {

        public List<Sach> DanhSachSach { get; set; } = new List<Sach>();

        public string? TuKhoa { get; set; }

        public int? MaTheLoai { get; set; }
        public string? NhaXuatBan { get; set; }
        public string? TrangThai { get; set; }
        public int? TuNam { get; set; }
        public int? DenNam { get; set; }

        public string? SapXep { get; set; } = "ten_asc";

        public int TrangHienTai { get; set; } = 1;
        public int TongSoTrang { get; set; } = 1;
        public int TongSoBanGhi { get; set; } = 0;
        public int KichThuocTrang { get; set; } = 6;

        public List<TheLoai> DanhSachTheLoai { get; set; } = new List<TheLoai>();
        public List<string> DanhSachNhaXuatBan { get; set; } = new List<string>();
    }
}
