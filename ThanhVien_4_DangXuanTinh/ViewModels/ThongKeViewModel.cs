namespace QuanLyThuVien.ViewModels
{
    public class ThongKeTheLoaiItem
    {
        public int MaTheLoai { get; set; }
        public string TenTheLoai { get; set; } = string.Empty;
        public int SoDauSach { get; set; }
        public int TongSoBanSach { get; set; }
        public int TongSoSachCon { get; set; }
    }

    public class ThongKeSachMuonItem
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; } = string.Empty;
        public string TacGia { get; set; } = string.Empty;
        public int SoLuotPhieuMuon { get; set; }
        public int TongSoCuonDuocMuon { get; set; }
    }

    public class ThongKeTrangThaiPhieuItem
    {
        public string TrangThai { get; set; } = string.Empty;
        public int SoLuongPhieu { get; set; }
    }

    public class ThongKeDocGiaMuonItem
    {
        public int MaDocGia { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public int SoPhieuMuon { get; set; }
        public int TongSachDaMuon { get; set; }
    }

    public class ThongKeTheoThangItem
    {
        public int Nam { get; set; }
        public int Thang { get; set; }
        public int SoPhieuMuon { get; set; }
        public int TongSoSachMuon { get; set; }
    }

    public class ThongKeDongTienItem
    {
        public string LoaiThanhToan { get; set; } = string.Empty;
        public string HuongDongTien { get; set; } = string.Empty;
        public int SoGiaoDich { get; set; }
        public decimal TongTien { get; set; }
    }

    public class ThongKeViewModel
    {
        public List<ThongKeTheLoaiItem> SachTheoTheLoai { get; set; } = new List<ThongKeTheLoaiItem>();
        public List<ThongKeSachMuonItem> LuotMuonTheoSach { get; set; } = new List<ThongKeSachMuonItem>();
        public List<ThongKeTrangThaiPhieuItem> PhieuTheoTrangThai { get; set; } = new List<ThongKeTrangThaiPhieuItem>();
        public List<ThongKeSachMuonItem> SachMuonNhieuNhat { get; set; } = new List<ThongKeSachMuonItem>();
        public List<ThongKeDocGiaMuonItem> LuotMuonTheoDocGia { get; set; } = new List<ThongKeDocGiaMuonItem>();
        public List<ThongKeTheoThangItem> PhieuMuonTheoThang { get; set; } = new List<ThongKeTheoThangItem>();

        // Thống kê tài chính dòng tiền
        public decimal TongNguonVao { get; set; }
        public decimal TongNguonRa { get; set; }
        public decimal TinhTonQuyTien { get; set; }
        public List<ThongKeDongTienItem> DongTienTheoMucDich { get; set; } = new List<ThongKeDongTienItem>();
    }
}
