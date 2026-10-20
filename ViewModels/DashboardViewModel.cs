using QuanLyThuVien.Models;

namespace QuanLyThuVien.ViewModels
{
    public class DashboardViewModel
    {

        public int TongDauSach { get; set; }
        public int TongTheLoai { get; set; }
        public int TongDocGia { get; set; }
        public int TongPhieuMuon { get; set; }
        public int PhieuChoXacNhan { get; set; }
        public int PhieuDangMuon { get; set; }
        public int TruongHopQuaHan { get; set; }
        public int TongSachConCoTheMuon { get; set; }

        public List<PhieuMuon> DanhSachChoXacNhan { get; set; } = new List<PhieuMuon>();
        public List<PhieuMuon> DanhSachQuaHan { get; set; } = new List<PhieuMuon>();
    }
}
