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

        // Tài chính dòng tiền
        public decimal TongNguonVao { get; set; }
        public decimal TongNguonRa { get; set; }
        public decimal TinhTonQuyTien { get; set; }
        public int TongGiaoDichThanhToan { get; set; }

        public List<PhieuMuon> DanhSachChoXacNhan { get; set; } = new List<PhieuMuon>();
        public List<PhieuMuon> DanhSachQuaHan { get; set; } = new List<PhieuMuon>();
        public List<GiaoDichThanhToan> GiaoDichMoiNhat { get; set; } = new List<GiaoDichThanhToan>();
    }
}
