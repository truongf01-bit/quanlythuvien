using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ThuVienDbContext context, string? webRootPath = null)
        {

            context.Database.Migrate();

            AssetGenerator.TaoTaiNguyenAnhSan(webRootPath ?? "");

            var sachHienTai = context.Sachs.ToList();
            if (sachHienTai.Any())
            {
                bool coCapNhat = false;
                for (int i = 0; i < sachHienTai.Count; i++)
                {
                    var s = sachHienTai[i];
                    if (string.IsNullOrEmpty(s.HinhAnh))
                    {
                        int maAnh = (i % 22) + 1;
                        s.HinhAnh = $"/images/books/book-{maAnh}.svg";
                        coCapNhat = true;
                    }
                }
                if (coCapNhat)
                {
                    context.SaveChanges();
                }
            }

            if (context.TaiKhoans.Any())
            {
                return;
            }

            var taiKhoans = new List<TaiKhoan>
            {
                new TaiKhoan { TenDangNhap = "admin", MatKhau = "123456", HoTen = "Quản Trị Viên Chính", VaiTro = "Admin", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "admin2", MatKhau = "123456", HoTen = "Thủ Thư Nguyễn Thị Mai", VaiTro = "Admin", TrangThai = "Hoạt động" },

                new TaiKhoan { TenDangNhap = "docgia1", MatKhau = "123456", HoTen = "Nguyễn Minh Tuấn", VaiTro = "DocGia", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "docgia2", MatKhau = "123456", HoTen = "Trần Thu Hà", VaiTro = "DocGia", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "docgia3", MatKhau = "123456", HoTen = "Lê Văn Hùng", VaiTro = "DocGia", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "docgia4", MatKhau = "123456", HoTen = "Phạm Thị Lan Anh", VaiTro = "DocGia", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "docgia5", MatKhau = "123456", HoTen = "Hoàng Đức Mạnh", VaiTro = "DocGia", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "docgia6", MatKhau = "123456", HoTen = "Vũ Thị Ngọc Bích", VaiTro = "DocGia", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "docgia7", MatKhau = "123456", HoTen = "Đặng Quốc Khánh", VaiTro = "DocGia", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "docgia8", MatKhau = "123456", HoTen = "Bùi Thanh Thảo", VaiTro = "DocGia", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "docgia9", MatKhau = "123456", HoTen = "Đỗ Hải Nam", VaiTro = "DocGia", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "docgia10", MatKhau = "123456", HoTen = "Ngô Thị Kim Ngân", VaiTro = "DocGia", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "docgia11", MatKhau = "123456", HoTen = "Dương Văn Phúc", VaiTro = "DocGia", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "docgia12", MatKhau = "123456", HoTen = "Lý Thị Tuyết Mai", VaiTro = "DocGia", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "docgia13", MatKhau = "123456", HoTen = "Trương Công Vinh", VaiTro = "DocGia", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "docgia14", MatKhau = "123456", HoTen = "Phan Thị Hồng Nhung", VaiTro = "DocGia", TrangThai = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "docgia15", MatKhau = "123456", HoTen = "Đinh Trọng Nghĩa", VaiTro = "DocGia", TrangThai = "Bị khóa" }
            };
            context.TaiKhoans.AddRange(taiKhoans);
            context.SaveChanges();

            var theLoais = new List<TheLoai>
            {
                new TheLoai { TenTheLoai = "Công nghệ thông tin", MoTa = "Sách lập trình C#, .NET, cơ sở dữ liệu, trí tuệ nhân tạo và mạng máy tính.", TrangThai = "Đang sử dụng" },
                new TheLoai { TenTheLoai = "Kinh tế - Quản trị", MoTa = "Sách quản trị kinh doanh, tài chính, kế toán, khởi nghiệp và marketing.", TrangThai = "Đang sử dụng" },
                new TheLoai { TenTheLoai = "Văn học Việt Nam", MoTa = "Tiểu thuyết, truyện ngắn, thơ ca và tác phẩm văn học kinh điển Việt Nam.", TrangThai = "Đang sử dụng" },
                new TheLoai { TenTheLoai = "Ngoại ngữ - Kỹ năng", MoTa = "Sách luyện thi tiếng Anh, giao tiếp và kỹ năng mềm cho sinh viên.", TrangThai = "Đang sử dụng" },
                new TheLoai { TenTheLoai = "Khoa học cơ bản", MoTa = "Giáo trình Toán cao cấp, Vật lý đại cương, Xác suất thống kê ứng dụng.", TrangThai = "Đang sử dụng" },
                new TheLoai { TenTheLoai = "Lịch sử - Văn hóa", MoTa = "Sách nghiên cứu lịch sử dân tộc, văn hóa truyền thống và địa lý.", TrangThai = "Đang sử dụng" }
            };
            context.TheLoais.AddRange(theLoais);
            context.SaveChanges();

            var sachs = new List<Sach>
            {

                new Sach { TenSach = "Lập trình Web với ASP.NET Core 10 MVC", MaTheLoai = theLoais[0].MaTheLoai, TacGia = "TS. Nguyễn Văn Hòa", NhaXuatBan = "NXB Khoa học Kỹ thuật", NamXuatBan = 2025, SoLuong = 20, SoLuongCon = 17, GiaSach = 185000, MoTa = "Giáo trình thực hành xây dựng ứng dụng Web chuẩn MVC trên nền tảng .NET 10 và EF Core 10.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-1.svg" },
                new Sach { TenSach = "Làm chủ C# và Lập trình Hướng đối tượng", MaTheLoai = theLoais[0].MaTheLoai, TacGia = "PGS.TS. Trần Minh Tuấn", NhaXuatBan = "NXB Giáo dục", NamXuatBan = 2024, SoLuong = 25, SoLuongCon = 22, GiaSach = 155000, MoTa = "Kiến thức nền tảng và nâng cao về ngôn ngữ C#, LINQ và xử lý tập hợp.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-2.svg" },
                new Sach { TenSach = "Hệ quản trị Cơ sở dữ liệu SQL Server", MaTheLoai = theLoais[0].MaTheLoai, TacGia = "ThS. Lê Hoàng Việt", NhaXuatBan = "NXB Khoa học Kỹ thuật", NamXuatBan = 2023, SoLuong = 18, SoLuongCon = 16, GiaSach = 140000, MoTa = "Thiết kế cơ sở dữ liệu quan hệ, truy vấn T-SQL và tối ưu hóa hiệu năng.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-3.svg" },
                new Sach { TenSach = "Cấu trúc dữ liệu và Giải thuật", MaTheLoai = theLoais[0].MaTheLoai, TacGia = "TS. Phạm Văn Ất", NhaXuatBan = "NXB Giáo dục", NamXuatBan = 2022, SoLuong = 15, SoLuongCon = 14, GiaSach = 130000, MoTa = "Các cấu trúc dữ liệu danh sách, cây, đồ thị và thuật toán sắp xếp, tìm kiếm.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-4.svg" },
                new Sach { TenSach = "Nhập môn Trí tuệ Nhân tạo và Học máy", MaTheLoai = theLoais[0].MaTheLoai, TacGia = "TS. Đỗ Quang Vinh", NhaXuatBan = "NXB Trẻ", NamXuatBan = 2025, SoLuong = 12, SoLuongCon = 11, GiaSach = 210000, MoTa = "Tổng quan về AI, các thuật toán học có giám sát và mạng nơ-ron nhân tạo.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-5.svg" },
                new Sach { TenSach = "An toàn và Bảo mật Hệ thống Thông tin", MaTheLoai = theLoais[0].MaTheLoai, TacGia = "ThS. Vũ Đình Khoa", NhaXuatBan = "NXB Thông tin và Truyền thông", NamXuatBan = 2021, SoLuong = 10, SoLuongCon = 10, GiaSach = 125000, MoTa = "Các nguyên lý mã hóa, phòng chống tấn công mạng và bảo mật ứng dụng Web.", TrangThai = "Tạm ngừng", HinhAnh = "/images/books/book-6.svg" },

                new Sach { TenSach = "Nguyên lý Quản trị Kinh doanh Hiện đại", MaTheLoai = theLoais[1].MaTheLoai, TacGia = "GS.TS. Nguyễn Thành Độ", NhaXuatBan = "NXB Lao Động", NamXuatBan = 2023, SoLuong = 20, SoLuongCon = 19, GiaSach = 145000, MoTa = "Hoạch định chiến lược, tổ chức nhân sự và điều hành doanh nghiệp thời đại số.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-7.svg" },
                new Sach { TenSach = "Marketing Căn bản và Thực hành", MaTheLoai = theLoais[1].MaTheLoai, TacGia = "TS. Trần Thị Phương", NhaXuatBan = "NXB Tổng hợp TP.HCM", NamXuatBan = 2024, SoLuong = 16, SoLuongCon = 15, GiaSach = 160000, MoTa = "Nghiên cứu thị trường, hành vi người tiêu dùng và chiến lược Marketing 4P.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-8.svg" },
                new Sach { TenSach = "Kinh tế học Vi mô và Vĩ mô", MaTheLoai = theLoais[1].MaTheLoai, TacGia = "PGS.TS. Vũ Kim Dũng", NhaXuatBan = "NXB Giáo dục", NamXuatBan = 2021, SoLuong = 22, SoLuongCon = 22, GiaSach = 175000, MoTa = "Phân tích quy luật cung cầu, thị trường, lạm phát và chính sách tài khóa.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-9.svg" },
                new Sach { TenSach = "Phân tích Báo cáo Tài chính Doanh nghiệp", MaTheLoai = theLoais[1].MaTheLoai, TacGia = "TS. Nguyễn Năng Phúc", NhaXuatBan = "NXB Tài chính", NamXuatBan = 2022, SoLuong = 14, SoLuongCon = 13, GiaSach = 150000, MoTa = "Hướng dẫn đọc hiểu bảng cân đối kế toán, lưu chuyển tiền tệ và đánh giá rủi ro.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-10.svg" },

                new Sach { TenSach = "Mắt Biếc", MaTheLoai = theLoais[2].MaTheLoai, TacGia = "Nguyễn Nhật Ánh", NhaXuatBan = "NXB Trẻ", NamXuatBan = 2020, SoLuong = 30, SoLuongCon = 28, GiaSach = 110000, MoTa = "Tác phẩm truyện dài nổi tiếng viết về tuổi học trò và tình yêu quê hương.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-11.svg" },
                new Sach { TenSach = "Cho Tôi Xin Một Vé Đi Tuổi Thơ", MaTheLoai = theLoais[2].MaTheLoai, TacGia = "Nguyễn Nhật Ánh", NhaXuatBan = "NXB Trẻ", NamXuatBan = 2019, SoLuong = 25, SoLuongCon = 24, GiaSach = 95000, MoTa = "Những câu chuyện hồn nhiên, dí dỏm và đầy suy ngẫm về thế giới tuổi thơ.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-12.svg" },
                new Sach { TenSach = "Dế Mèn Phiêu Lưu Ký", MaTheLoai = theLoais[2].MaTheLoai, TacGia = "Tô Hoài", NhaXuatBan = "NXB Kim Đồng", NamXuatBan = 2021, SoLuong = 20, SoLuongCon = 20, GiaSach = 75000, MoTa = "Tác phẩm văn học thiếu nhi kinh điển của nhà văn Tô Hoài.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-13.svg" },
                new Sach { TenSach = "Tuyển tập Truyện ngắn Nam Cao", MaTheLoai = theLoais[2].MaTheLoai, TacGia = "Nam Cao", NhaXuatBan = "NXB Văn học", NamXuatBan = 2018, SoLuong = 15, SoLuongCon = 15, GiaSach = 88000, MoTa = "Tập hợp các truyện ngắn hiện thực xuất sắc: Chí Phèo, Lão Hạc, Đời thừa.", TrangThai = "Ngừng phục vụ", HinhAnh = "/images/books/book-14.svg" },

                new Sach { TenSach = "Luyện thi TOEIC 750+ Toàn diện", MaTheLoai = theLoais[3].MaTheLoai, TacGia = "ThS. Mai Lan Hương", NhaXuatBan = "NXB Tổng hợp TP.HCM", NamXuatBan = 2024, SoLuong = 28, SoLuongCon = 26, GiaSach = 195000, MoTa = "Hệ thống ngữ pháp, từ vựng trọng tâm và bộ đề luyện nghe đọc TOEIC chuẩn mới.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-15.svg" },
                new Sach { TenSach = "Tiếng Anh Chuyên ngành Công nghệ Thông tin", MaTheLoai = theLoais[3].MaTheLoai, TacGia = "ThS. Hoàng Thị Minh", NhaXuatBan = "NXB Giáo dục", NamXuatBan = 2023, SoLuong = 20, SoLuongCon = 19, GiaSach = 120000, MoTa = "Thuật ngữ tiếng Anh chuyên ngành phần mềm, phần cứng và đọc hiểu tài liệu kỹ thuật.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-16.svg" },
                new Sach { TenSach = "Kỹ năng Giao tiếp và Thuyết trình Hiệu quả", MaTheLoai = theLoais[3].MaTheLoai, TacGia = "TS. Lê Thẩm Dương", NhaXuatBan = "NXB Lao Động", NamXuatBan = 2022, SoLuong = 18, SoLuongCon = 18, GiaSach = 105000, MoTa = "Phương pháp xây dựng bài thuyết trình ấn tượng, làm việc nhóm và quản lý thời gian.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-17.svg" },
                new Sach { TenSach = "Tư duy Phản biện dành cho Sinh viên", MaTheLoai = theLoais[3].MaTheLoai, TacGia = "ThS. Nguyễn Hoàng Khắc Hiếu", NhaXuatBan = "NXB Trẻ", NamXuatBan = 2023, SoLuong = 15, SoLuongCon = 15, GiaSach = 98000, MoTa = "Rèn luyện khả năng phân tích vấn đề, lập luận logic và ra quyết định.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-18.svg" },

                new Sach { TenSach = "Giáo trình Toán Cao cấp Tập 1 - Đại số Tuyến tính", MaTheLoai = theLoais[4].MaTheLoai, TacGia = "GS. Nguyễn Đình Trí", NhaXuatBan = "NXB Giáo dục", NamXuatBan = 2020, SoLuong = 35, SoLuongCon = 34, GiaSach = 68000, MoTa = "Ma trận, định thức, hệ phương trình tuyến tính và không gian vectơ.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-19.svg" },
                new Sach { TenSach = "Xác suất Thống kê và Ứng dụng", MaTheLoai = theLoais[4].MaTheLoai, TacGia = "PGS.TS. Đào Hữu Hồ", NhaXuatBan = "NXB Giáo dục", NamXuatBan = 2022, SoLuong = 30, SoLuongCon = 29, GiaSach = 82000, MoTa = "Biến ngẫu nhiên, các quy luật phân phối xác suất, ước lượng và kiểm định giả thuyết.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-20.svg" },
                new Sach { TenSach = "Vật lý Đại cương Tập 2 - Điện và Từ học", MaTheLoai = theLoais[4].MaTheLoai, TacGia = "GS. Lương Duyên Bình", NhaXuatBan = "NXB Giáo dục", NamXuatBan = 2019, SoLuong = 25, SoLuongCon = 25, GiaSach = 72000, MoTa = "Trường tĩnh điện, từ trường và các định luật điện từ cơ bản.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-21.svg" },

                new Sach { TenSach = "Cơ sở Văn hóa Việt Nam", MaTheLoai = theLoais[5].MaTheLoai, TacGia = "GS.TS. Trần Ngọc Thêm", NhaXuatBan = "NXB Giáo dục", NamXuatBan = 2021, SoLuong = 20, SoLuongCon = 20, GiaSach = 115000, MoTa = "Nghiên cứu nguồn gốc, bản sắc và tiến trình văn hóa Việt Nam qua các thời kỳ.", TrangThai = "Đang phục vụ", HinhAnh = "/images/books/book-22.svg" }
            };
            context.Sachs.AddRange(sachs);
            context.SaveChanges();

            var docGias = new List<DocGia>
            {
                new DocGia { MaTaiKhoan = taiKhoans[2].MaTaiKhoan, HoTen = "Nguyễn Minh Tuấn", NgaySinh = new DateTime(2004, 3, 15), GioiTinh = "Nam", SoDienThoai = "0912345001", Email = "minhtuan01@sv.uneti.edu.vn", DiaChi = "218 Lĩnh Nam, Hoàng Mai, Hà Nội", NgayDangKy = new DateTime(2025, 9, 5), TrangThai = "Hoạt động" },
                new DocGia { MaTaiKhoan = taiKhoans[3].MaTaiKhoan, HoTen = "Trần Thu Hà", NgaySinh = new DateTime(2004, 7, 22), GioiTinh = "Nữ", SoDienThoai = "0912345002", Email = "thuha02@sv.uneti.edu.vn", DiaChi = "454 Minh Khai, Hai Bà Trưng, Hà Nội", NgayDangKy = new DateTime(2025, 9, 6), TrangThai = "Hoạt động" },
                new DocGia { MaTaiKhoan = taiKhoans[4].MaTaiKhoan, HoTen = "Lê Văn Hùng", NgaySinh = new DateTime(2003, 11, 10), GioiTinh = "Nam", SoDienThoai = "0912345003", Email = "vanhung03@sv.uneti.edu.vn", DiaChi = "120 Giải Phóng, Đống Đa, Hà Nội", NgayDangKy = new DateTime(2025, 9, 10), TrangThai = "Hoạt động" },
                new DocGia { MaTaiKhoan = taiKhoans[5].MaTaiKhoan, HoTen = "Phạm Thị Lan Anh", NgaySinh = new DateTime(2004, 5, 18), GioiTinh = "Nữ", SoDienThoai = "0912345004", Email = "lananh04@sv.uneti.edu.vn", DiaChi = "85 Tam Trinh, Hoàng Mai, Hà Nội", NgayDangKy = new DateTime(2025, 9, 12), TrangThai = "Hoạt động" },
                new DocGia { MaTaiKhoan = taiKhoans[6].MaTaiKhoan, HoTen = "Hoàng Đức Mạnh", NgaySinh = new DateTime(2004, 1, 25), GioiTinh = "Nam", SoDienThoai = "0912345005", Email = "ducmanh05@sv.uneti.edu.vn", DiaChi = "32 Vĩnh Tuy, Hai Bà Trưng, Hà Nội", NgayDangKy = new DateTime(2025, 9, 15), TrangThai = "Hoạt động" },
                new DocGia { MaTaiKhoan = taiKhoans[7].MaTaiKhoan, HoTen = "Vũ Thị Ngọc Bích", NgaySinh = new DateTime(2005, 2, 14), GioiTinh = "Nữ", SoDienThoai = "0912345006", Email = "ngocbich06@sv.uneti.edu.vn", DiaChi = "19 Kim Ngưu, Hai Bà Trưng, Hà Nội", NgayDangKy = new DateTime(2025, 9, 20), TrangThai = "Hoạt động" },
                new DocGia { MaTaiKhoan = taiKhoans[8].MaTaiKhoan, HoTen = "Đặng Quốc Khánh", NgaySinh = new DateTime(2004, 9, 2), GioiTinh = "Nam", SoDienThoai = "0912345007", Email = "quockhanh07@sv.uneti.edu.vn", DiaChi = "102 Trương Định, Hoàng Mai, Hà Nội", NgayDangKy = new DateTime(2025, 10, 1), TrangThai = "Hoạt động" },
                new DocGia { MaTaiKhoan = taiKhoans[9].MaTaiKhoan, HoTen = "Bùi Thanh Thảo", NgaySinh = new DateTime(2004, 12, 5), GioiTinh = "Nữ", SoDienThoai = "0912345008", Email = "thanhthao08@sv.uneti.edu.vn", DiaChi = "56 Đại La, Hai Bà Trưng, Hà Nội", NgayDangKy = new DateTime(2025, 10, 3), TrangThai = "Hoạt động" },
                new DocGia { MaTaiKhoan = taiKhoans[10].MaTaiKhoan, HoTen = "Đỗ Hải Nam", NgaySinh = new DateTime(2003, 6, 30), GioiTinh = "Nam", SoDienThoai = "0912345009", Email = "hainam09@sv.uneti.edu.vn", DiaChi = "78 Bạch Mai, Hai Bà Trưng, Hà Nội", NgayDangKy = new DateTime(2025, 10, 10), TrangThai = "Hoạt động" },
                new DocGia { MaTaiKhoan = taiKhoans[11].MaTaiKhoan, HoTen = "Ngô Thị Kim Ngân", NgaySinh = new DateTime(2005, 4, 19), GioiTinh = "Nữ", SoDienThoai = "0912345010", Email = "kimngan10@sv.uneti.edu.vn", DiaChi = "15 Tân Mai, Hoàng Mai, Hà Nội", NgayDangKy = new DateTime(2025, 10, 15), TrangThai = "Hoạt động" },
                new DocGia { MaTaiKhoan = taiKhoans[12].MaTaiKhoan, HoTen = "Dương Văn Phúc", NgaySinh = new DateTime(2004, 8, 12), GioiTinh = "Nam", SoDienThoai = "0912345011", Email = "vanphuc11@sv.uneti.edu.vn", DiaChi = "210 Nguyễn Khoái, Hoàng Mai, Hà Nội", NgayDangKy = new DateTime(2025, 11, 1), TrangThai = "Hoạt động" },
                new DocGia { MaTaiKhoan = taiKhoans[13].MaTaiKhoan, HoTen = "Lý Thị Tuyết Mai", NgaySinh = new DateTime(2004, 10, 28), GioiTinh = "Nữ", SoDienThoai = "0912345012", Email = "tuyetmai12@sv.uneti.edu.vn", DiaChi = "90 Lò Đúc, Hai Bà Trưng, Hà Nội", NgayDangKy = new DateTime(2025, 11, 5), TrangThai = "Hoạt động" },
                new DocGia { MaTaiKhoan = taiKhoans[14].MaTaiKhoan, HoTen = "Trương Công Vinh", NgaySinh = new DateTime(2003, 2, 9), GioiTinh = "Nam", SoDienThoai = "0912345013", Email = "congvinh13@sv.uneti.edu.vn", DiaChi = "45 Định Công, Hoàng Mai, Hà Nội", NgayDangKy = new DateTime(2025, 11, 12), TrangThai = "Hoạt động" },
                new DocGia { MaTaiKhoan = taiKhoans[15].MaTaiKhoan, HoTen = "Phan Thị Hồng Nhung", NgaySinh = new DateTime(2005, 1, 17), GioiTinh = "Nữ", SoDienThoai = "0912345014", Email = "hongnhung14@sv.uneti.edu.vn", DiaChi = "64 Phố Vọng, Đống Đa, Hà Nội", NgayDangKy = new DateTime(2025, 11, 20), TrangThai = "Hoạt động" },
                new DocGia { MaTaiKhoan = taiKhoans[16].MaTaiKhoan, HoTen = "Đinh Trọng Nghĩa", NgaySinh = new DateTime(2004, 4, 4), GioiTinh = "Nam", SoDienThoai = "0912345015", Email = "trongnghia15@sv.uneti.edu.vn", DiaChi = "12 Đền Lừ, Hoàng Mai, Hà Nội", NgayDangKy = new DateTime(2025, 12, 1), TrangThai = "Bị khóa" }
            };
            context.DocGias.AddRange(docGias);
            context.SaveChanges();

            var today = DateTime.Today;
            var phieuMuons = new List<PhieuMuon>
            {

                new PhieuMuon { MaDocGia = docGias[0].MaDocGia, NgayMuon = today.AddDays(-75), HanTra = today.AddDays(-61), TrangThai = "Đã trả" },
                new PhieuMuon { MaDocGia = docGias[1].MaDocGia, NgayMuon = today.AddDays(-70), HanTra = today.AddDays(-56), TrangThai = "Đã trả" },
                new PhieuMuon { MaDocGia = docGias[2].MaDocGia, NgayMuon = today.AddDays(-55), HanTra = today.AddDays(-41), TrangThai = "Đã trả" },
                new PhieuMuon { MaDocGia = docGias[3].MaDocGia, NgayMuon = today.AddDays(-45), HanTra = today.AddDays(-31), TrangThai = "Đã trả" },
                new PhieuMuon { MaDocGia = docGias[4].MaDocGia, NgayMuon = today.AddDays(-35), HanTra = today.AddDays(-21), TrangThai = "Đã trả" },
                new PhieuMuon { MaDocGia = docGias[5].MaDocGia, NgayMuon = today.AddDays(-28), HanTra = today.AddDays(-14), TrangThai = "Đã trả" },
                new PhieuMuon { MaDocGia = docGias[6].MaDocGia, NgayMuon = today.AddDays(-20), HanTra = today.AddDays(-6),  TrangThai = "Đã trả" },

                new PhieuMuon { MaDocGia = docGias[0].MaDocGia, NgayMuon = today.AddDays(-25), HanTra = today.AddDays(-11), TrangThai = "Đang mượn" },
                new PhieuMuon { MaDocGia = docGias[2].MaDocGia, NgayMuon = today.AddDays(-18), HanTra = today.AddDays(-4),  TrangThai = "Đang mượn" },
                new PhieuMuon { MaDocGia = docGias[7].MaDocGia, NgayMuon = today.AddDays(-16), HanTra = today.AddDays(-2),  TrangThai = "Đang mượn" },

                new PhieuMuon { MaDocGia = docGias[1].MaDocGia, NgayMuon = today.AddDays(-5),  HanTra = today.AddDays(9),   TrangThai = "Đang mượn" },
                new PhieuMuon { MaDocGia = docGias[8].MaDocGia, NgayMuon = today.AddDays(-3),  HanTra = today.AddDays(11),  TrangThai = "Đang mượn" },
                new PhieuMuon { MaDocGia = docGias[9].MaDocGia, NgayMuon = today.AddDays(-2),  HanTra = today.AddDays(12),  TrangThai = "Đang mượn" },

                new PhieuMuon { MaDocGia = docGias[3].MaDocGia, NgayMuon = today.AddDays(-1),  HanTra = today.AddDays(13),  TrangThai = "Chờ xác nhận" },
                new PhieuMuon { MaDocGia = docGias[10].MaDocGia, NgayMuon = today,             HanTra = today.AddDays(14),  TrangThai = "Chờ xác nhận" },
                new PhieuMuon { MaDocGia = docGias[11].MaDocGia, NgayMuon = today,             HanTra = today.AddDays(14),  TrangThai = "Chờ xác nhận" },

                new PhieuMuon { MaDocGia = docGias[12].MaDocGia, NgayMuon = today.AddDays(-15), HanTra = today.AddDays(-1), TrangThai = "Đã hủy" },
                new PhieuMuon { MaDocGia = docGias[13].MaDocGia, NgayMuon = today.AddDays(-8),  HanTra = today.AddDays(6),  TrangThai = "Đã hủy" }
            };
            context.PhieuMuons.AddRange(phieuMuons);
            context.SaveChanges();

            var chiTiets = new List<ChiTietPhieuMuon>
            {

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[0].MaPhieuMuon, MaSach = sachs[0].MaSach, SoLuongMuon = 1, NgayTra = today.AddDays(-63), TinhTrangTra = "Tốt" },
                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[0].MaPhieuMuon, MaSach = sachs[1].MaSach, SoLuongMuon = 1, NgayTra = today.AddDays(-63), TinhTrangTra = "Tốt" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[1].MaPhieuMuon, MaSach = sachs[2].MaSach, SoLuongMuon = 1, NgayTra = today.AddDays(-58), TinhTrangTra = "Tốt" },
                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[1].MaPhieuMuon, MaSach = sachs[10].MaSach, SoLuongMuon = 1, NgayTra = today.AddDays(-58), TinhTrangTra = "Tốt" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[2].MaPhieuMuon, MaSach = sachs[0].MaSach, SoLuongMuon = 1, NgayTra = today.AddDays(-42), TinhTrangTra = "Tốt" },
                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[2].MaPhieuMuon, MaSach = sachs[3].MaSach, SoLuongMuon = 1, NgayTra = today.AddDays(-42), TinhTrangTra = "Hư hỏng nhẹ" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[3].MaPhieuMuon, MaSach = sachs[6].MaSach, SoLuongMuon = 1, NgayTra = today.AddDays(-33), TinhTrangTra = "Tốt" },
                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[3].MaPhieuMuon, MaSach = sachs[7].MaSach, SoLuongMuon = 1, NgayTra = today.AddDays(-33), TinhTrangTra = "Tốt" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[4].MaPhieuMuon, MaSach = sachs[14].MaSach, SoLuongMuon = 2, NgayTra = today.AddDays(-22), TinhTrangTra = "Tốt" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[5].MaPhieuMuon, MaSach = sachs[1].MaSach, SoLuongMuon = 1, NgayTra = today.AddDays(-15), TinhTrangTra = "Tốt" },
                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[5].MaPhieuMuon, MaSach = sachs[18].MaSach, SoLuongMuon = 1, NgayTra = today.AddDays(-15), TinhTrangTra = "Tốt" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[6].MaPhieuMuon, MaSach = sachs[0].MaSach, SoLuongMuon = 1, NgayTra = today.AddDays(-7), TinhTrangTra = "Tốt" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[7].MaPhieuMuon, MaSach = sachs[0].MaSach, SoLuongMuon = 1, NgayTra = null, TinhTrangTra = "Chưa trả" },
                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[7].MaPhieuMuon, MaSach = sachs[2].MaSach, SoLuongMuon = 1, NgayTra = null, TinhTrangTra = "Chưa trả" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[8].MaPhieuMuon, MaSach = sachs[1].MaSach, SoLuongMuon = 1, NgayTra = null, TinhTrangTra = "Chưa trả" },
                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[8].MaPhieuMuon, MaSach = sachs[4].MaSach, SoLuongMuon = 1, NgayTra = null, TinhTrangTra = "Chưa trả" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[9].MaPhieuMuon, MaSach = sachs[10].MaSach, SoLuongMuon = 2, NgayTra = null, TinhTrangTra = "Chưa trả" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[10].MaPhieuMuon, MaSach = sachs[0].MaSach, SoLuongMuon = 2, NgayTra = null, TinhTrangTra = "Chưa trả" },
                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[10].MaPhieuMuon, MaSach = sachs[14].MaSach, SoLuongMuon = 2, NgayTra = null, TinhTrangTra = "Chưa trả" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[11].MaPhieuMuon, MaSach = sachs[1].MaSach, SoLuongMuon = 2, NgayTra = null, TinhTrangTra = "Chưa trả" },
                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[11].MaPhieuMuon, MaSach = sachs[6].MaSach, SoLuongMuon = 1, NgayTra = null, TinhTrangTra = "Chưa trả" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[12].MaPhieuMuon, MaSach = sachs[2].MaSach, SoLuongMuon = 1, NgayTra = null, TinhTrangTra = "Chưa trả" },
                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[12].MaPhieuMuon, MaSach = sachs[15].MaSach, SoLuongMuon = 1, NgayTra = null, TinhTrangTra = "Chưa trả" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[13].MaPhieuMuon, MaSach = sachs[3].MaSach, SoLuongMuon = 1, NgayTra = null, TinhTrangTra = "Chưa trả" },
                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[13].MaPhieuMuon, MaSach = sachs[18].MaSach, SoLuongMuon = 1, NgayTra = null, TinhTrangTra = "Chưa trả" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[14].MaPhieuMuon, MaSach = sachs[7].MaSach, SoLuongMuon = 1, NgayTra = null, TinhTrangTra = "Chưa trả" },
                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[14].MaPhieuMuon, MaSach = sachs[11].MaSach, SoLuongMuon = 1, NgayTra = null, TinhTrangTra = "Chưa trả" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[15].MaPhieuMuon, MaSach = sachs[19].MaSach, SoLuongMuon = 1, NgayTra = null, TinhTrangTra = "Chưa trả" },

                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[16].MaPhieuMuon, MaSach = sachs[9].MaSach, SoLuongMuon = 1, NgayTra = null, TinhTrangTra = "Đã hủy" },
                new ChiTietPhieuMuon { MaPhieuMuon = phieuMuons[17].MaPhieuMuon, MaSach = sachs[12].MaSach, SoLuongMuon = 1, NgayTra = null, TinhTrangTra = "Đã hủy" }
            };
            context.ChiTietPhieuMuons.AddRange(chiTiets);
            context.SaveChanges();
        }
    }
}
