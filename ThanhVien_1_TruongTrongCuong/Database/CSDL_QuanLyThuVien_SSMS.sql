-- ====================================================================================================
-- TRƯỜNG ĐẠI HỌC KINH TẾ - KỸ THUẬT CÔNG NGHIỆP (UNETI)
-- KHOA CÔNG NGHỆ THÔNG TIN - BỘ MÔN CÔNG NGHỆ PHẦN MỀM
-- ĐỒ ÁN MÔN HỌC: THỰC HÀNH LẬP TRÌNH .NET (ASP.NET CORE 10 MVC & SQL SERVER)
-- ĐỀ TÀI 02: HỆ THỐNG QUẢN LÝ THƯ VIỆN VÀ MƯỢN TRẢ SÁCH
--
-- NHÓM SINH VIÊN NĂM 4 THỰC HIỆN:
-- 1. Trương Trọng Cường (Trưởng nhóm - Phân quyền Session & Quản lý Tài khoản, Thể loại)
-- 2. Nguyễn Xuân Trường (Thành viên - Quản lý Đầu sách & Giao diện Trang chủ, Tra cứu)
-- 3. Nguyễn Viết Hồng   (Thành viên - Quản lý Độc giả, Đăng ký mượn & Chi tiết phiếu mượn)
-- 4. Đặng Xuân Tình     (Thành viên - Quản lý Phiếu mượn/trả, Xử lý Quá hạn & Thống kê)
--
-- CÔNG CỤ: SQL Server Management Studio (SSMS 18 / 19 / 20 / 22) - T-SQL Microsoft SQL Server
-- ====================================================================================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLyThuVienDb')
BEGIN
    CREATE DATABASE QuanLyThuVienDb
    COLLATE Vietnamese_CI_AS;
    PRINT N'==> Đã tạo mới Database QuanLyThuVienDb thành công.';
END
GO

USE QuanLyThuVienDb;
GO

PRINT N'==> Đang làm việc trên Database: QuanLyThuVienDb';
GO

DROP TABLE IF EXISTS [dbo].[GiaoDichThanhToans];
DROP TABLE IF EXISTS [dbo].[ChiTietPhieuMuons];
DROP TABLE IF EXISTS [dbo].[PhieuMuons];
DROP TABLE IF EXISTS [dbo].[DocGias];
DROP TABLE IF EXISTS [dbo].[Sachs];
DROP TABLE IF EXISTS [dbo].[TheLoais];
DROP TABLE IF EXISTS [dbo].[TaiKhoans];
GO

CREATE TABLE [dbo].[TaiKhoans] (
    [MaTaiKhoan]   INT IDENTITY(1,1) NOT NULL,
    [TenDangNhap]  NVARCHAR(50)      NOT NULL,
    [MatKhau]      NVARCHAR(100)     NOT NULL,
    [HoTen]        NVARCHAR(100)     NOT NULL,
    [VaiTro]       NVARCHAR(20)      NOT NULL CONSTRAINT [DF_TaiKhoans_VaiTro] DEFAULT (N'DocGia'),
    [TrangThai]    NVARCHAR(30)      NOT NULL CONSTRAINT [DF_TaiKhoans_TrangThai] DEFAULT (N'Hoạt động'),

    CONSTRAINT [PK_TaiKhoans] PRIMARY KEY CLUSTERED ([MaTaiKhoan] ASC),
    CONSTRAINT [UQ_TaiKhoans_TenDangNhap] UNIQUE NONCLUSTERED ([TenDangNhap] ASC),
    CONSTRAINT [CK_TaiKhoans_VaiTro] CHECK ([VaiTro] IN (N'Admin', N'DocGia')),
    CONSTRAINT [CK_TaiKhoans_TrangThai] CHECK ([TrangThai] IN (N'Hoạt động', N'Bị khóa'))
);
GO

CREATE TABLE [dbo].[TheLoais] (
    [MaTheLoai]    INT IDENTITY(1,1) NOT NULL,
    [TenTheLoai]   NVARCHAR(100)     NOT NULL,
    [MoTa]         NVARCHAR(500)     NULL,
    [TrangThai]    NVARCHAR(30)      NOT NULL CONSTRAINT [DF_TheLoais_TrangThai] DEFAULT (N'Đang sử dụng'),

    CONSTRAINT [PK_TheLoais] PRIMARY KEY CLUSTERED ([MaTheLoai] ASC),
    CONSTRAINT [UQ_TheLoais_TenTheLoai] UNIQUE NONCLUSTERED ([TenTheLoai] ASC)
);
GO

CREATE TABLE [dbo].[Sachs] (
    [MaSach]       INT IDENTITY(1,1) NOT NULL,
    [TenSach]      NVARCHAR(200)     NOT NULL,
    [MaTheLoai]    INT               NOT NULL,
    [TacGia]       NVARCHAR(100)     NOT NULL,
    [NhaXuatBan]   NVARCHAR(150)     NOT NULL,
    [NamXuatBan]   INT               NOT NULL,
    [SoLuong]      INT               NOT NULL CONSTRAINT [DF_Sachs_SoLuong] DEFAULT (0),
    [SoLuongCon]   INT               NOT NULL CONSTRAINT [DF_Sachs_SoLuongCon] DEFAULT (0),
    [GiaSach]      DECIMAL(18, 2)    NOT NULL CONSTRAINT [DF_Sachs_GiaSach] DEFAULT (0),
    [MoTa]         NVARCHAR(1000)    NULL,
    [TrangThai]    NVARCHAR(30)      NOT NULL CONSTRAINT [DF_Sachs_TrangThai] DEFAULT (N'Đang phục vụ'),
    [HinhAnh]      NVARCHAR(255)     NULL,

    CONSTRAINT [PK_Sachs] PRIMARY KEY CLUSTERED ([MaSach] ASC),
    CONSTRAINT [FK_Sachs_TheLoais] FOREIGN KEY ([MaTheLoai]) REFERENCES [dbo].[TheLoais] ([MaTheLoai]) ON DELETE NO ACTION,
    CONSTRAINT [CK_Sachs_NamXuatBan] CHECK ([NamXuatBan] >= 1900 AND [NamXuatBan] <= 2100),
    CONSTRAINT [CK_Sachs_SoLuong] CHECK ([SoLuong] >= 0),
    CONSTRAINT [CK_Sachs_SoLuongCon] CHECK ([SoLuongCon] >= 0 AND [SoLuongCon] <= [SoLuong]),
    CONSTRAINT [CK_Sachs_GiaSach] CHECK ([GiaSach] >= 0)
);
GO

CREATE TABLE [dbo].[DocGias] (
    [MaDocGia]     INT IDENTITY(1,1) NOT NULL,
    [MaTaiKhoan]   INT               NOT NULL,
    [HoTen]        NVARCHAR(100)     NOT NULL,
    [NgaySinh]     DATE              NOT NULL,
    [GioiTinh]     NVARCHAR(20)      NOT NULL CONSTRAINT [DF_DocGias_GioiTinh] DEFAULT (N'Nam'),
    [SoDienThoai]  NVARCHAR(15)      NOT NULL,
    [Email]        NVARCHAR(100)     NOT NULL,
    [DiaChi]       NVARCHAR(250)     NOT NULL,
    [NgayDangKy]   DATE              NOT NULL CONSTRAINT [DF_DocGias_NgayDangKy] DEFAULT (CAST(GETDATE() AS DATE)),
    [TrangThai]    NVARCHAR(30)      NOT NULL CONSTRAINT [DF_DocGias_TrangThai] DEFAULT (N'Hoạt động'),

    CONSTRAINT [PK_DocGias] PRIMARY KEY CLUSTERED ([MaDocGia] ASC),
    CONSTRAINT [UQ_DocGias_MaTaiKhoan] UNIQUE NONCLUSTERED ([MaTaiKhoan] ASC),
    CONSTRAINT [FK_DocGias_TaiKhoans] FOREIGN KEY ([MaTaiKhoan]) REFERENCES [dbo].[TaiKhoans] ([MaTaiKhoan]) ON DELETE NO ACTION,
    CONSTRAINT [CK_DocGias_TrangThai] CHECK ([TrangThai] IN (N'Hoạt động', N'Bị khóa'))
);
GO

CREATE TABLE [dbo].[PhieuMuons] (
    [MaPhieuMuon]  INT IDENTITY(1,1) NOT NULL,
    [MaDocGia]     INT               NOT NULL,
    [NgayMuon]     DATE              NOT NULL CONSTRAINT [DF_PhieuMuons_NgayMuon] DEFAULT (CAST(GETDATE() AS DATE)),
    [HanTra]       DATE              NOT NULL,
    [TrangThai]    NVARCHAR(50)      NOT NULL CONSTRAINT [DF_PhieuMuons_TrangThai] DEFAULT (N'Chờ xác nhận'),

    CONSTRAINT [PK_PhieuMuons] PRIMARY KEY CLUSTERED ([MaPhieuMuon] ASC),
    CONSTRAINT [FK_PhieuMuons_DocGias] FOREIGN KEY ([MaDocGia]) REFERENCES [dbo].[DocGias] ([MaDocGia]) ON DELETE NO ACTION,
    CONSTRAINT [CK_PhieuMuons_TrangThai] CHECK ([TrangThai] IN (N'Chờ xác nhận', N'Đang mượn', N'Đã trả', N'Đã hủy')),
    CONSTRAINT [CK_PhieuMuons_HanTra] CHECK ([HanTra] >= [NgayMuon])
);
GO

CREATE TABLE [dbo].[ChiTietPhieuMuons] (
    [MaPhieuMuon]  INT           NOT NULL,
    [MaSach]       INT           NOT NULL,
    [SoLuongMuon]  INT           NOT NULL CONSTRAINT [DF_CTPM_SoLuongMuon] DEFAULT (1),
    [NgayTra]      DATE          NULL,
    [TinhTrangTra] NVARCHAR(100) NULL,

    CONSTRAINT [PK_ChiTietPhieuMuons] PRIMARY KEY CLUSTERED ([MaPhieuMuon] ASC, [MaSach] ASC),
    CONSTRAINT [FK_CTPM_PhieuMuons] FOREIGN KEY ([MaPhieuMuon]) REFERENCES [dbo].[PhieuMuons] ([MaPhieuMuon]) ON DELETE CASCADE,
    CONSTRAINT [FK_CTPM_Sachs] FOREIGN KEY ([MaSach]) REFERENCES [dbo].[Sachs] ([MaSach]) ON DELETE NO ACTION,
    CONSTRAINT [CK_CTPM_SoLuongMuon] CHECK ([SoLuongMuon] > 0)
);
GO

CREATE TABLE [dbo].[GiaoDichThanhToans] (
    [MaGiaoDich]               INT IDENTITY(1,1) NOT NULL,
    [MaGiaoDichCode]           NVARCHAR(50)      NOT NULL,
    [MaPhieuMuon]              INT               NULL,
    [MaDocGia]                 INT               NULL,
    [HuongDongTien]            NVARCHAR(30)      NOT NULL CONSTRAINT [DF_GDTT_HuongDongTien] DEFAULT (N'Nguồn vào'),
    [LoaiThanhToan]            NVARCHAR(100)     NOT NULL CONSTRAINT [DF_GDTT_LoaiThanhToan] DEFAULT (N'Tiền cọc mượn sách'),
    [SoTien]                   DECIMAL(18, 2)    NOT NULL CONSTRAINT [DF_GDTT_SoTien] DEFAULT (0),
    [NguonTienRa]              NVARCHAR(250)     NOT NULL,
    [NguonTienVao]             NVARCHAR(250)     NOT NULL,
    [PhuongThuc]               NVARCHAR(100)     NOT NULL CONSTRAINT [DF_GDTT_PhuongThuc] DEFAULT (N'Tiền mặt tại quầy'),
    [NguoiThucHien]            NVARCHAR(100)     NULL,
    [SoDienThoai]              NVARCHAR(20)      NULL,
    [ThongTinTaiKhoanDoiUng]   NVARCHAR(250)     NULL,
    [NoiDung]                  NVARCHAR(500)     NULL,
    [NgayGiaoDich]             DATETIME2(7)      NOT NULL CONSTRAINT [DF_GDTT_NgayGiaoDich] DEFAULT (GETDATE()),
    [TrangThai]                NVARCHAR(50)      NOT NULL CONSTRAINT [DF_GDTT_TrangThai] DEFAULT (N'Đã hoàn thành'),

    CONSTRAINT [PK_GiaoDichThanhToans] PRIMARY KEY CLUSTERED ([MaGiaoDich] ASC),
    CONSTRAINT [FK_GDTT_PhieuMuons] FOREIGN KEY ([MaPhieuMuon]) REFERENCES [dbo].[PhieuMuons] ([MaPhieuMuon]) ON DELETE SET NULL,
    CONSTRAINT [FK_GDTT_DocGias] FOREIGN KEY ([MaDocGia]) REFERENCES [dbo].[DocGias] ([MaDocGia]) ON DELETE SET NULL,
    CONSTRAINT [CK_GDTT_HuongDongTien] CHECK ([HuongDongTien] IN (N'Nguồn vào', N'Nguồn ra')),
    CONSTRAINT [CK_GDTT_SoTien] CHECK ([SoTien] >= 0)
);
GO

CREATE NONCLUSTERED INDEX [IX_Sachs_MaTheLoai] ON [dbo].[Sachs]([MaTheLoai] ASC);
CREATE NONCLUSTERED INDEX [IX_Sachs_TenSach] ON [dbo].[Sachs]([TenSach] ASC);
CREATE NONCLUSTERED INDEX [IX_DocGias_SoDienThoai] ON [dbo].[DocGias]([SoDienThoai] ASC);
CREATE NONCLUSTERED INDEX [IX_DocGias_Email] ON [dbo].[DocGias]([Email] ASC);
CREATE NONCLUSTERED INDEX [IX_PhieuMuons_MaDocGia] ON [dbo].[PhieuMuons]([MaDocGia] ASC);
CREATE NONCLUSTERED INDEX [IX_PhieuMuons_TrangThai] ON [dbo].[PhieuMuons]([TrangThai] ASC);
CREATE NONCLUSTERED INDEX [IX_PhieuMuons_HanTra] ON [dbo].[PhieuMuons]([HanTra] ASC);
CREATE NONCLUSTERED INDEX [IX_GiaoDichThanhToans_MaPhieuMuon] ON [dbo].[GiaoDichThanhToans]([MaPhieuMuon] ASC);
CREATE NONCLUSTERED INDEX [IX_GiaoDichThanhToans_MaDocGia] ON [dbo].[GiaoDichThanhToans]([MaDocGia] ASC);
CREATE NONCLUSTERED INDEX [IX_GiaoDichThanhToans_HuongDongTien] ON [dbo].[GiaoDichThanhToans]([HuongDongTien] ASC);
CREATE NONCLUSTERED INDEX [IX_GiaoDichThanhToans_NgayGiaoDich] ON [dbo].[GiaoDichThanhToans]([NgayGiaoDich] ASC);
CREATE NONCLUSTERED INDEX [IX_GiaoDichThanhToans_TrangThai] ON [dbo].[GiaoDichThanhToans]([TrangThai] ASC);
GO

SET IDENTITY_INSERT [dbo].[TaiKhoans] ON;
INSERT INTO [dbo].[TaiKhoans] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [VaiTro], [TrangThai]) VALUES
(1,  N'admin',    N'123456', N'Quản Trị Viên Chính',      N'Admin',  N'Hoạt động'),
(2,  N'admin2',   N'123456', N'Thủ Thư Nguyễn Thị Mai',   N'Admin',  N'Hoạt động'),
(3,  N'docgia1',  N'123456', N'Nguyễn Minh Tuấn',         N'DocGia', N'Hoạt động'),
(4,  N'docgia2',  N'123456', N'Trần Thu Hà',              N'DocGia', N'Hoạt động'),
(5,  N'docgia3',  N'123456', N'Lê Văn Hùng',              N'DocGia', N'Hoạt động'),
(6,  N'docgia4',  N'123456', N'Phạm Thị Lan Anh',         N'DocGia', N'Hoạt động'),
(7,  N'docgia5',  N'123456', N'Hoàng Đức Mạnh',           N'DocGia', N'Hoạt động'),
(8,  N'docgia6',  N'123456', N'Vũ Thị Ngọc Bích',         N'DocGia', N'Hoạt động'),
(9,  N'docgia7',  N'123456', N'Đặng Quốc Khánh',          N'DocGia', N'Hoạt động'),
(10, N'docgia8',  N'123456', N'Bùi Thanh Thảo',           N'DocGia', N'Hoạt động'),
(11, N'docgia9',  N'123456', N'Đỗ Hải Nam',               N'DocGia', N'Hoạt động'),
(12, N'docgia10', N'123456', N'Ngô Thị Kim Ngân',         N'DocGia', N'Hoạt động'),
(13, N'docgia11', N'123456', N'Dương Văn Phúc',          N'DocGia', N'Hoạt động'),
(14, N'docgia12', N'123456', N'Lý Thị Tuyết Mai',         N'DocGia', N'Hoạt động'),
(15, N'docgia13', N'123456', N'Trương Công Vinh',         N'DocGia', N'Hoạt động'),
(16, N'docgia14', N'123456', N'Phan Thị Hồng Nhung',      N'DocGia', N'Hoạt động'),
(17, N'docgia15', N'123456', N'Đinh Trọng Nghĩa',         N'DocGia', N'Bị khóa');
SET IDENTITY_INSERT [dbo].[TaiKhoans] OFF;
GO

SET IDENTITY_INSERT [dbo].[TheLoais] ON;
INSERT INTO [dbo].[TheLoais] ([MaTheLoai], [TenTheLoai], [MoTa], [TrangThai]) VALUES
(1, N'Công nghệ thông tin', N'Sách lập trình C#, .NET, cơ sở dữ liệu, trí tuệ nhân tạo và mạng máy tính.', N'Đang sử dụng'),
(2, N'Kinh tế - Quản trị', N'Sách quản trị kinh doanh, tài chính, kế toán, khởi nghiệp và marketing.', N'Đang sử dụng'),
(3, N'Văn học Việt Nam', N'Tiểu thuyết, truyện ngắn, thơ ca và tác phẩm văn học kinh điển Việt Nam.', N'Đang sử dụng'),
(4, N'Ngoại ngữ - Kỹ năng', N'Sách luyện thi tiếng Anh, giao tiếp và kỹ năng mềm cho sinh viên.', N'Đang sử dụng'),
(5, N'Khoa học cơ bản', N'Giáo trình Toán cao cấp, Vật lý đại cương, Xác suất thống kê ứng dụng.', N'Đang sử dụng'),
(6, N'Lịch sử - Văn hóa', N'Sách nghiên cứu lịch sử dân tộc, văn hóa truyền thống và địa lý.', N'Đang sử dụng');
SET IDENTITY_INSERT [dbo].[TheLoais] OFF;
GO

SET IDENTITY_INSERT [dbo].[Sachs] ON;
INSERT INTO [dbo].[Sachs] ([MaSach], [TenSach], [MaTheLoai], [TacGia], [NhaXuatBan], [NamXuatBan], [SoLuong], [SoLuongCon], [GiaSach], [MoTa], [TrangThai], [HinhAnh]) VALUES
(1,  N'Lập trình Web với ASP.NET Core 10 MVC', 1, N'TS. Nguyễn Văn Hòa', N'NXB Khoa học Kỹ thuật', 2025, 20, 17, 185000.00, N'Giáo trình thực hành xây dựng ứng dụng Web chuẩn MVC trên nền tảng .NET 10 và EF Core 10.', N'Đang phục vụ', N'/images/books/book-1.svg'),
(2,  N'Làm chủ C# và Lập trình Hướng đối tượng', 1, N'PGS.TS. Trần Minh Tuấn', N'NXB Giáo dục', 2024, 25, 22, 155000.00, N'Kiến thức nền tảng và nâng cao về ngôn ngữ C#, LINQ và xử lý tập hợp.', N'Đang phục vụ', N'/images/books/book-2.svg'),
(3,  N'Hệ quản trị Cơ sở dữ liệu SQL Server', 1, N'ThS. Lê Hoàng Việt', N'NXB Khoa học Kỹ thuật', 2023, 18, 16, 140000.00, N'Thiết kế cơ sở dữ liệu quan hệ, truy vấn T-SQL và tối ưu hóa hiệu năng.', N'Đang phục vụ', N'/images/books/book-3.svg'),
(4,  N'Cấu trúc dữ liệu và Giải thuật', 1, N'TS. Phạm Văn Ất', N'NXB Giáo dục', 2022, 15, 14, 130000.00, N'Các cấu trúc dữ liệu danh sách, cây, đồ thị và thuật toán sắp xếp, tìm kiếm.', N'Đang phục vụ', N'/images/books/book-4.svg'),
(5,  N'Nhập môn Trí tuệ Nhân tạo và Học máy', 1, N'TS. Đỗ Quang Vinh', N'NXB Trẻ', 2025, 12, 11, 210000.00, N'Tổng quan về AI, các thuật toán học có giám sát và mạng nơ-ron nhân tạo.', N'Đang phục vụ', N'/images/books/book-5.svg'),
(6,  N'An toàn và Bảo mật Hệ thống Thông tin', 1, N'ThS. Vũ Đình Khoa', N'NXB Thông tin và Truyền thông', 2021, 10, 10, 125000.00, N'Các nguyên lý mã hóa, phòng chống tấn công mạng và bảo mật ứng dụng Web.', N'Đang phục vụ', N'/images/books/book-6.svg'),
(7,  N'Nguyên lý Quản trị Kinh doanh Hiện đại', 2, N'GS.TS. Nguyễn Thành Độ', N'NXB Lao Động', 2023, 20, 19, 145000.00, N'Hoạch định chiến lược, tổ chức nhân sự và điều hành doanh nghiệp thời đại số.', N'Đang phục vụ', N'/images/books/book-7.svg'),
(8,  N'Marketing Căn bản và Thực hành', 2, N'TS. Trần Thị Phương', N'NXB Tổng hợp TP.HCM', 2024, 16, 15, 160000.00, N'Nghiên cứu thị trường, hành vi người tiêu dùng và chiến lược Marketing 4P.', N'Đang phục vụ', N'/images/books/book-8.svg'),
(9,  N'Kinh tế học Vi mô và Vĩ mô', 2, N'PGS.TS. Vũ Kim Dũng', N'NXB Giáo dục', 2021, 22, 22, 175000.00, N'Phân tích quy luật cung cầu, thị trường, lạm phát và chính sách tài khóa.', N'Đang phục vụ', N'/images/books/book-9.svg'),
(10, N'Phân tích Báo cáo Tài chính Doanh nghiệp', 2, N'TS. Nguyễn Năng Phúc', N'NXB Tài chính', 2022, 14, 13, 150000.00, N'Hướng dẫn đọc hiểu bảng cân đối kế toán, lưu chuyển tiền tệ và đánh giá rủi ro.', N'Đang phục vụ', N'/images/books/book-10.svg'),
(11, N'Mắt Biếc', 3, N'Nguyễn Nhật Ánh', N'NXB Trẻ', 2020, 30, 28, 110000.00, N'Tác phẩm truyện dài nổi tiếng viết về tuổi học trò và tình yêu quê hương.', N'Đang phục vụ', N'/images/books/book-11.svg'),
(12, N'Cho Tôi Xin Một Vé Đi Tuổi Thơ', 3, N'Nguyễn Nhật Ánh', N'NXB Trẻ', 2019, 25, 24, 95000.00, N'Những câu chuyện hồn nhiên, dí dỏm và đầy suy ngẫm về thế giới tuổi thơ.', N'Đang phục vụ', N'/images/books/book-12.svg'),
(13, N'Dế Mèn Phiêu Lưu Ký', 3, N'Tô Hoài', N'NXB Kim Đồng', 2021, 20, 20, 75000.00, N'Tác phẩm văn học thiếu nhi kinh điển của nhà văn Tô Hoài.', N'Đang phục vụ', N'/images/books/book-13.svg'),
(14, N'Tuyển tập Truyện ngắn Nam Cao', 3, N'Nam Cao', N'NXB Văn học', 2018, 15, 15, 88000.00, N'Tập hợp các truyện ngắn hiện thực xuất sắc: Chí Phèo, Lão Hạc, Đời thừa.', N'Đang phục vụ', N'/images/books/book-14.svg'),
(15, N'Luyện thi TOEIC 750+ Toàn diện', 4, N'ThS. Mai Lan Hương', N'NXB Tổng hợp TP.HCM', 2024, 28, 26, 195000.00, N'Hệ thống ngữ pháp, từ vựng trọng tâm và bộ đề luyện nghe đọc TOEIC chuẩn mới.', N'Đang phục vụ', N'/images/books/book-15.svg'),
(16, N'Tiếng Anh Chuyên ngành Công nghệ Thông tin', 4, N'ThS. Hoàng Thị Minh', N'NXB Giáo dục', 2023, 20, 19, 120000.00, N'Thuật ngữ tiếng Anh chuyên ngành phần mềm, phần cứng và đọc hiểu tài liệu kỹ thuật.', N'Đang phục vụ', N'/images/books/book-16.svg'),
(17, N'Kỹ năng Giao tiếp và Thuyết trình Hiệu quả', 4, N'TS. Lê Thẩm Dương', N'NXB Lao Động', 2022, 18, 18, 105000.00, N'Phương pháp xây dựng bài thuyết trình ấn tượng, làm việc nhóm và quản lý thời gian.', N'Đang phục vụ', N'/images/books/book-17.svg'),
(18, N'Tư duy Phản biện dành cho Sinh viên', 4, N'ThS. Nguyễn Hoàng Khắc Hiếu', N'NXB Trẻ', 2023, 15, 15, 98000.00, N'Rèn luyện khả năng phân tích vấn đề, lập luận logic và ra quyết định.', N'Đang phục vụ', N'/images/books/book-18.svg'),
(19, N'Giáo trình Toán Cao cấp Tập 1 - Đại số Tuyến tính', 5, N'GS. Nguyễn Đình Trí', N'NXB Giáo dục', 2020, 35, 34, 68000.00, N'Ma trận, định thức, hệ phương trình tuyến tính và không gian vectơ.', N'Đang phục vụ', N'/images/books/book-19.svg'),
20, N'Xác suất Thống kê và Ứng dụng', 5, N'PGS.TS. Đào Hữu Hồ', N'NXB Giáo dục', 2022, 30, 29, 82000.00, N'Biến ngẫu nhiên, các quy luật phân phối xác suất, ước lượng và kiểm định giả thuyết.', N'Đang phục vụ', N'/images/books/book-20.svg'),
(21, N'Vật lý Đại cương Tập 2 - Điện và Từ học', 5, N'GS. Lương Duyên Bình', N'NXB Giáo dục', 2019, 25, 25, 72000.00, N'Trường tĩnh điện, từ trường và các định luật điện từ cơ bản.', N'Đang phục vụ', N'/images/books/book-21.svg'),
(22, N'Cơ sở Văn hóa Việt Nam', 6, N'GS.TS. Trần Ngọc Thêm', N'NXB Giáo dục', 2021, 20, 20, 115000.00, N'Nghiên cứu nguồn gốc, bản sắc và tiến trình văn hóa Việt Nam qua các thời kỳ.', N'Đang phục vụ', N'/images/books/book-22.svg');
SET IDENTITY_INSERT [dbo].[Sachs] OFF;
GO

SET IDENTITY_INSERT [dbo].[DocGias] ON;
INSERT INTO [dbo].[DocGias] ([MaDocGia], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [SoDienThoai], [Email], [DiaChi], [NgayDangKy], [TrangThai]) VALUES
(1,  3,  N'Nguyễn Minh Tuấn',    '2004-03-15', N'Nam', N'0912345001', N'minhtuan01@sv.uneti.edu.vn',  N'218 Lĩnh Nam, Hoàng Mai, Hà Nội',     '2025-09-05', N'Hoạt động'),
(2,  4,  N'Trần Thu Hà',         '2004-07-22', N'Nữ',  N'0912345002', N'thuha02@sv.uneti.edu.vn',     N'454 Minh Khai, Hai Bà Trưng, Hà Nội', '2025-09-06', N'Hoạt động'),
(3,  5,  N'Lê Văn Hùng',         '2003-11-10', N'Nam', N'0912345003', N'vanhung03@sv.uneti.edu.vn',    N'120 Giải Phóng, Đống Đa, Hà Nội',     '2025-09-10', N'Hoạt động'),
(4,  6,  N'Phạm Thị Lan Anh',    '2004-05-18', N'Nữ',  N'0912345004', N'lananh04@sv.uneti.edu.vn',    N'85 Tam Trinh, Hoàng Mai, Hà Nội',     '2025-09-12', N'Hoạt động'),
(5,  7,  N'Hoàng Đức Mạnh',      '2004-01-25', N'Nam', N'0912345005', N'ducmanh05@sv.uneti.edu.vn',   N'32 Vĩnh Tuy, Hai Bà Trưng, Hà Nội',   '2025-09-15', N'Hoạt động'),
(6,  8,  N'Vũ Thị Ngọc Bích',    '2005-02-14', N'Nữ',  N'0912345006', N'ngocbich06@sv.uneti.edu.vn',  N'19 Kim Ngưu, Hai Bà Trưng, Hà Nội',   '2025-09-20', N'Hoạt động'),
(7,  9,  N'Đặng Quốc Khánh',     '2004-09-02', N'Nam', N'0912345007', N'quockhanh07@sv.uneti.edu.vn', N'102 Trương Định, Hoàng Mai, Hà Nội', '2025-10-01', N'Hoạt động'),
(8,  10, N'Bùi Thanh Thảo',      '2004-12-05', N'Nữ',  N'0912345008', N'thanhthao08@sv.uneti.edu.vn', N'56 Đại La, Hai Bà Trưng, Hà Nội',    '2025-10-03', N'Hoạt động'),
(9,  11, N'Đỗ Hải Nam',          '2003-06-30', N'Nam', N'0912345009', N'hainam09@sv.uneti.edu.vn',    N'78 Bạch Mai, Hai Bà Trưng, Hà Nội',   '2025-10-10', N'Hoạt động'),
(10, 12, N'Ngô Thị Kim Ngân',    '2005-04-19', N'Nữ',  N'0912345010', N'kimngan10@sv.uneti.edu.vn',   N'15 Tân Mai, Hoàng Mai, Hà Nội',       '2025-10-15', N'Hoạt động'),
(11, 13, N'Dương Văn Phúc',      '2004-08-12', N'Nam', N'0912345011', N'vanphuc11@sv.uneti.edu.vn',   N'210 Nguyễn Khoái, Hoàng Mai, Hà Nội', '2025-11-01', N'Hoạt động'),
(12, 14, N'Lý Thị Tuyết Mai',    '2004-10-28', N'Nữ',  N'0912345012', N'tuyetmai12@sv.uneti.edu.vn',  N'90 Lò Đúc, Hai Bà Trưng, Hà Nội',     '2025-11-05', N'Hoạt động'),
(13, 15, N'Trương Công Vinh',    '2003-02-09', N'Nam', N'0912345013', N'congvinh13@sv.uneti.edu.vn',  N'45 Định Công, Hoàng Mai, Hà Nội',     '2025-11-12', N'Hoạt động'),
(14, 16, N'Phan Thị Hồng Nhung', '2005-01-17', N'Nữ',  N'0912345014', N'hongnhung14@sv.uneti.edu.vn', N'64 Phố Vọng, Đống Đa, Hà Nội',       '2025-11-20', N'Hoạt động'),
(15, 17, N'Đinh Trọng Nghĩa',    '2004-04-04', N'Nam', N'0912345015', N'trongnghia15@sv.uneti.edu.vn',N'12 Đền Lừ, Hoàng Mai, Hà Nội',       '2025-12-01', N'Bị khóa');
SET IDENTITY_INSERT [dbo].[DocGias] OFF;
GO

SET IDENTITY_INSERT [dbo].[PhieuMuons] ON;
INSERT INTO [dbo].[PhieuMuons] ([MaPhieuMuon], [MaDocGia], [NgayMuon], [HanTra], [TrangThai]) VALUES

(1,  1,  DATEADD(DAY, -75, CAST(GETDATE() AS DATE)), DATEADD(DAY, -61, CAST(GETDATE() AS DATE)), N'Đã trả'),
(2,  2,  DATEADD(DAY, -70, CAST(GETDATE() AS DATE)), DATEADD(DAY, -56, CAST(GETDATE() AS DATE)), N'Đã trả'),
(3,  3,  DATEADD(DAY, -55, CAST(GETDATE() AS DATE)), DATEADD(DAY, -41, CAST(GETDATE() AS DATE)), N'Đã trả'),
(4,  4,  DATEADD(DAY, -45, CAST(GETDATE() AS DATE)), DATEADD(DAY, -31, CAST(GETDATE() AS DATE)), N'Đã trả'),
(5,  5,  DATEADD(DAY, -35, CAST(GETDATE() AS DATE)), DATEADD(DAY, -21, CAST(GETDATE() AS DATE)), N'Đã trả'),
(6,  6,  DATEADD(DAY, -28, CAST(GETDATE() AS DATE)), DATEADD(DAY, -14, CAST(GETDATE() AS DATE)), N'Đã trả'),
(7,  7,  DATEADD(DAY, -20, CAST(GETDATE() AS DATE)), DATEADD(DAY, -6,  CAST(GETDATE() AS DATE)), N'Đã trả'),

(8,  1,  DATEADD(DAY, -25, CAST(GETDATE() AS DATE)), DATEADD(DAY, -11, CAST(GETDATE() AS DATE)), N'Đang mượn'),
(9,  3,  DATEADD(DAY, -18, CAST(GETDATE() AS DATE)), DATEADD(DAY, -4,  CAST(GETDATE() AS DATE)), N'Đang mượn'),
(10, 8,  DATEADD(DAY, -16, CAST(GETDATE() AS DATE)), DATEADD(DAY, -2,  CAST(GETDATE() AS DATE)), N'Đang mượn'),

(11, 2,  DATEADD(DAY, -5,  CAST(GETDATE() AS DATE)), DATEADD(DAY, 9,   CAST(GETDATE() AS DATE)), N'Đang mượn'),
(12, 9,  DATEADD(DAY, -3,  CAST(GETDATE() AS DATE)), DATEADD(DAY, 11,  CAST(GETDATE() AS DATE)), N'Đang mượn'),
(13, 10, DATEADD(DAY, -2,  CAST(GETDATE() AS DATE)), DATEADD(DAY, 12,  CAST(GETDATE() AS DATE)), N'Đang mượn'),

(14, 4,  DATEADD(DAY, -1,  CAST(GETDATE() AS DATE)), DATEADD(DAY, 13,  CAST(GETDATE() AS DATE)), N'Chờ xác nhận'),
(15, 11, CAST(GETDATE() AS DATE),                    DATEADD(DAY, 14,  CAST(GETDATE() AS DATE)), N'Chờ xác nhận'),
(16, 12, CAST(GETDATE() AS DATE),                    DATEADD(DAY, 14,  CAST(GETDATE() AS DATE)), N'Chờ xác nhận'),

(17, 13, DATEADD(DAY, -15, CAST(GETDATE() AS DATE)), DATEADD(DAY, -1,  CAST(GETDATE() AS DATE)), N'Đã hủy'),
(18, 14, DATEADD(DAY, -8,  CAST(GETDATE() AS DATE)), DATEADD(DAY, 6,   CAST(GETDATE() AS DATE)), N'Đã hủy');
SET IDENTITY_INSERT [dbo].[PhieuMuons] OFF;
GO

INSERT INTO [dbo].[ChiTietPhieuMuons] ([MaPhieuMuon], [MaSach], [SoLuongMuon], [NgayTra], [TinhTrangTra]) VALUES

(1, 1,  1, DATEADD(DAY, -63, CAST(GETDATE() AS DATE)), N'Tốt'),
(1, 2,  1, DATEADD(DAY, -63, CAST(GETDATE() AS DATE)), N'Tốt'),
(2, 3,  1, DATEADD(DAY, -58, CAST(GETDATE() AS DATE)), N'Tốt'),
(2, 11, 1, DATEADD(DAY, -58, CAST(GETDATE() AS DATE)), N'Tốt'),
(3, 1,  1, DATEADD(DAY, -42, CAST(GETDATE() AS DATE)), N'Tốt'),
(3, 4,  1, DATEADD(DAY, -42, CAST(GETDATE() AS DATE)), N'Hư hỏng nhẹ'),
(4, 7,  1, DATEADD(DAY, -33, CAST(GETDATE() AS DATE)), N'Tốt'),
(4, 8,  1, DATEADD(DAY, -33, CAST(GETDATE() AS DATE)), N'Tốt'),
(5, 15, 2, DATEADD(DAY, -22, CAST(GETDATE() AS DATE)), N'Tốt'),
(6, 2,  1, DATEADD(DAY, -15, CAST(GETDATE() AS DATE)), N'Tốt'),
(6, 19, 1, DATEADD(DAY, -15, CAST(GETDATE() AS DATE)), N'Tốt'),
(7, 1,  1, DATEADD(DAY, -7,  CAST(GETDATE() AS DATE)), N'Tốt'),

(8,  1,  1, NULL, N'Chưa trả'),
(8,  3,  1, NULL, N'Chưa trả'),
(9,  2,  1, NULL, N'Chưa trả'),
(9,  5,  1, NULL, N'Chưa trả'),
(10, 11, 2, NULL, N'Chưa trả'),

(11, 1,  2, NULL, N'Chưa trả'),
(11, 15, 2, NULL, N'Chưa trả'),
(12, 2,  2, NULL, N'Chưa trả'),
(12, 7,  1, NULL, N'Chưa trả'),
(13, 3,  1, NULL, N'Chưa trả'),
(13, 16, 1, NULL, N'Chưa trả'),

(14, 4,  1, NULL, N'Chưa trả'),
(14, 19, 1, NULL, N'Chưa trả'),
(15, 8,  1, NULL, N'Chưa trả'),
(15, 12, 1, NULL, N'Chưa trả'),
(16, 20, 1, NULL, N'Chưa trả'),

(17, 10, 1, NULL, N'Đã hủy'),
(18, 13, 1, NULL, N'Đã hủy');
GO

SET IDENTITY_INSERT [dbo].[GiaoDichThanhToans] ON;
INSERT INTO [dbo].[GiaoDichThanhToans] (
    [MaGiaoDich], [MaGiaoDichCode], [MaPhieuMuon], [MaDocGia], [HuongDongTien],
    [LoaiThanhToan], [SoTien], [NguonTienRa], [NguonTienVao], [PhuongThuc],
    [NguoiThucHien], [SoDienThoai], [ThongTinTaiKhoanDoiUng], [NoiDung], [NgayGiaoDich], [TrangThai]
) VALUES
(1, N'THU202602100915_PM1', 1, 1, N'Nguồn vào', N'Tiền cọc mượn sách', 100000,
 N'Tài khoản cá nhân độc giả (Chuyển khoản QR)', N'Tài khoản MB Bank thư viện (21611611116 - NGUYEN XUAN TRUONG)',
 N'Chuyển khoản QR (MB Bank)', N'Nguyễn Minh Tuấn', N'0912345001', NULL,
 N'UNETI COC PM1 DG1', DATEADD(MINUTE, 555, DATEADD(DAY, -75, CAST(CAST(GETDATE() AS DATE) AS DATETIME2))), N'Đã hoàn thành'),

(2, N'CHI202602221430_PM1', 1, 1, N'Nguồn ra', N'Hoàn trả tiền cọc', 100000,
 N'Quỹ tiền mặt quầy thủ thư', N'Tiền mặt độc giả nhận tại quầy',
 N'Tiền mặt tại quầy', N'Nguyễn Minh Tuấn', N'0912345001', NULL,
 N'Hoàn trả tiền cọc phiếu mượn #1', DATEADD(MINUTE, 870, DATEADD(DAY, -63, CAST(CAST(GETDATE() AS DATE) AS DATETIME2))), N'Đã hoàn thành'),

(3, N'THU202602151020_PM2', 2, 2, N'Nguồn vào', N'Tiền cọc mượn sách', 100000,
 N'Ví MoMo của độc giả', N'Ví MoMo thư viện (09******498)',
 N'Ví MoMo', N'Trần Thu Hà', N'0912345002', NULL,
 N'UNETI COC PM2 DG2', DATEADD(MINUTE, 620, DATEADD(DAY, -70, CAST(CAST(GETDATE() AS DATE) AS DATETIME2))), N'Đã hoàn thành'),

(4, N'CHI202602271610_PM2', 2, 2, N'Nguồn ra', N'Hoàn trả tiền cọc', 100000,
 N'Tài khoản MB Bank thư viện (21611611116 - NGUYEN XUAN TRUONG)', N'Tài khoản ngân hàng của độc giả / sinh viên',
 N'Chuyển khoản QR (MB Bank)', N'Trần Thu Hà', N'0912345002', N'1038291039 - Vietcombank (Trần Thu Hà)',
 N'Hoàn tiền cọc mượn sách phiếu #2', DATEADD(MINUTE, 970, DATEADD(DAY, -58, CAST(CAST(GETDATE() AS DATE) AS DATETIME2))), N'Đã hoàn thành'),

(5, N'THU202603010845_PM3', 3, 3, N'Nguồn vào', N'Tiền cọc mượn sách', 100000,
 N'Tiền mặt của độc giả mang theo', N'Quỹ tiền mặt quầy thủ thư',
 N'Tiền mặt tại quầy', N'Lê Văn Hùng', N'0912345003', NULL,
 N'UNETI COC PM3 DG3', DATEADD(MINUTE, 525, DATEADD(DAY, -55, CAST(CAST(GETDATE() AS DATE) AS DATETIME2))), N'Đã hoàn thành'),

(6, N'CHI202603141530_PM3', 3, 3, N'Nguồn ra', N'Hoàn trả tiền cọc', 100000,
 N'Quỹ tiền mặt quầy thủ thư', N'Tiền mặt độc giả nhận tại quầy',
 N'Tiền mặt tại quầy', N'Lê Văn Hùng', N'0912345003', NULL,
 N'Hoàn trả tiền cọc phiếu mượn #3', DATEADD(MINUTE, 930, DATEADD(DAY, -42, CAST(CAST(GETDATE() AS DATE) AS DATETIME2))), N'Đã hoàn thành'),

(7, N'THU202603141535_PM3', 3, 3, N'Nguồn vào', N'Bồi thường hư hỏng sách', 30000,
 N'Tiền mặt của độc giả mang theo', N'Quỹ tiền mặt quầy thủ thư',
 N'Tiền mặt tại quầy', N'Lê Văn Hùng', N'0912345003', NULL,
 N'Bồi thường sách hư hỏng nhẹ phiếu #3', DATEADD(MINUTE, 935, DATEADD(DAY, -42, CAST(CAST(GETDATE() AS DATE) AS DATETIME2))), N'Đã hoàn thành'),

(8, N'THU202603310900_PM8', 8, 1, N'Nguồn vào', N'Tiền cọc mượn sách', 100000,
 N'Tài khoản cá nhân độc giả (Chuyển khoản QR)', N'Tài khoản MB Bank thư viện (21611611116 - NGUYEN XUAN TRUONG)',
 N'Chuyển khoản QR (MB Bank)', N'Nguyễn Minh Tuấn', N'0912345001', NULL,
 N'UNETI COC PM8 DG1', DATEADD(MINUTE, 540, DATEADD(DAY, -25, CAST(CAST(GETDATE() AS DATE) AS DATETIME2))), N'Đã hoàn thành'),

(9, N'THU202604071415_PM9', 9, 3, N'Nguồn vào', N'Tiền cọc mượn sách', 100000,
 N'Tiền mặt của độc giả mang theo', N'Quỹ tiền mặt quầy thủ thư',
 N'Tiền mặt tại quầy', N'Lê Văn Hùng', N'0912345003', NULL,
 N'UNETI COC PM9 DG3', DATEADD(MINUTE, 855, DATEADD(DAY, -18, CAST(CAST(GETDATE() AS DATE) AS DATETIME2))), N'Đã hoàn thành'),

(10, N'THU202604091130_PM10', 10, 8, N'Nguồn vào', N'Tiền cọc mượn sách', 100000,
 N'Ví MoMo của độc giả', N'Ví MoMo thư viện (09******498)',
 N'Ví MoMo', N'Bùi Thanh Thảo', N'0912345008', NULL,
 N'UNETI COC PM10 DG8', DATEADD(MINUTE, 690, DATEADD(DAY, -16, CAST(CAST(GETDATE() AS DATE) AS DATETIME2))), N'Đã hoàn thành');
SET IDENTITY_INSERT [dbo].[GiaoDichThanhToans] OFF;
GO

CREATE OR ALTER VIEW [dbo].[v_DanhSachSach]
AS
SELECT
    s.[MaSach],
    s.[TenSach],
    tl.[TenTheLoai],
    s.[TacGia],
    s.[NhaXuatBan],
    s.[NamXuatBan],
    s.[SoLuong],
    s.[SoLuongCon],
    (s.[SoLuong] - s.[SoLuongCon]) AS [SoLuongDangMuon],
    s.[GiaSach],
    s.[TrangThai]
FROM [dbo].[Sachs] s
INNER JOIN [dbo].[TheLoais] tl ON s.[MaTheLoai] = tl.[MaTheLoai];
GO

CREATE OR ALTER VIEW [dbo].[v_LichSuPhieuMuon]
AS
SELECT
    pm.[MaPhieuMuon],
    dg.[MaDocGia],
    dg.[HoTen] AS [TenDocGia],
    dg.[SoDienThoai],
    dg.[Email],
    pm.[NgayMuon],
    pm.[HanTra],
    pm.[TrangThai],
    CASE
        WHEN pm.[TrangThai] = N'Đang mượn' AND pm.[HanTra] < CAST(GETDATE() AS DATE)
        THEN DATEDIFF(DAY, pm.[HanTra], CAST(GETDATE() AS DATE))
        ELSE 0
    END AS [SoNgayQuaHan],
    COUNT(ct.[MaSach]) AS [TongSoDauSachMuon],
    ISNULL(SUM(ct.[SoLuongMuon]), 0) AS [TongSoCuonMuon]
FROM [dbo].[PhieuMuons] pm
INNER JOIN [dbo].[DocGias] dg ON pm.[MaDocGia] = dg.[MaDocGia]
LEFT JOIN [dbo].[ChiTietPhieuMuons] ct ON pm.[MaPhieuMuon] = ct.[MaPhieuMuon]
GROUP BY
    pm.[MaPhieuMuon], dg.[MaDocGia], dg.[HoTen], dg.[SoDienThoai], dg.[Email],
    pm.[NgayMuon], pm.[HanTra], pm.[TrangThai];
GO

CREATE OR ALTER VIEW [dbo].[v_DanhSachQuaHan]
AS
SELECT
    pm.[MaPhieuMuon],
    dg.[HoTen] AS [TenDocGia],
    dg.[SoDienThoai],
    dg.[Email],
    s.[TenSach],
    ct.[SoLuongMuon],
    pm.[NgayMuon],
    pm.[HanTra],
    DATEDIFF(DAY, pm.[HanTra], CAST(GETDATE() AS DATE)) AS [SoNgayTreHan]
FROM [dbo].[PhieuMuons] pm
INNER JOIN [dbo].[DocGias] dg ON pm.[MaDocGia] = dg.[MaDocGia]
INNER JOIN [dbo].[ChiTietPhieuMuons] ct ON pm.[MaPhieuMuon] = ct.[MaPhieuMuon]
INNER JOIN [dbo].[Sachs] s ON ct.[MaSach] = s.[MaSach]
WHERE pm.[TrangThai] = N'Đang mượn'
  AND pm.[HanTra] < CAST(GETDATE() AS DATE)
  AND ct.[NgayTra] IS NULL;
GO

CREATE OR ALTER VIEW [dbo].[v_SoGiaoDichDongTien]
AS
SELECT
    gd.[MaGiaoDich],
    gd.[MaGiaoDichCode],
    gd.[MaPhieuMuon],
    gd.[MaDocGia],
    dg.[HoTen] AS [TenDocGia],
    gd.[HuongDongTien],
    gd.[LoaiThanhToan],
    gd.[SoTien],
    gd.[NguonTienRa],
    gd.[NguonTienVao],
    gd.[PhuongThuc],
    gd.[NguoiThucHien],
    gd.[SoDienThoai],
    gd.[ThongTinTaiKhoanDoiUng],
    gd.[NoiDung],
    gd.[NgayGiaoDich],
    gd.[TrangThai]
FROM [dbo].[GiaoDichThanhToans] gd
LEFT JOIN [dbo].[DocGias] dg ON gd.[MaDocGia] = dg.[MaDocGia];
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_DangKyDocGia]
    @TenDangNhap  NVARCHAR(50),
    @MatKhau      NVARCHAR(100),
    @HoTen        NVARCHAR(100),
    @NgaySinh     DATE,
    @GioiTinh     NVARCHAR(20),
    @SoDienThoai  NVARCHAR(15),
    @Email        NVARCHAR(100),
    @DiaChi       NVARCHAR(250),
    @KetQua       INT OUTPUT,
    @ThongBao     NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM [dbo].[TaiKhoans] WHERE [TenDangNhap] = @TenDangNhap)
    BEGIN
        SET @KetQua = -1;
        SET @ThongBao = N'Tên đăng nhập đã tồn tại trong hệ thống!';
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM [dbo].[DocGias] WHERE [Email] = @Email)
    BEGIN
        SET @KetQua = -2;
        SET @ThongBao = N'Địa chỉ Email đã được đăng ký!';
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM [dbo].[DocGias] WHERE [SoDienThoai] = @SoDienThoai)
    BEGIN
        SET @KetQua = -3;
        SET @ThongBao = N'Số điện thoại đã được đăng ký!';
        RETURN;
    END

    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @NewMaTaiKhoan INT;

        INSERT INTO [dbo].[TaiKhoans] ([TenDangNhap], [MatKhau], [HoTen], [VaiTro], [TrangThai])
        VALUES (@TenDangNhap, @MatKhau, @HoTen, N'DocGia', N'Hoạt động');

        SET @NewMaTaiKhoan = SCOPE_IDENTITY();

        INSERT INTO [dbo].[DocGias] ([MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [SoDienThoai], [Email], [DiaChi], [NgayDangKy], [TrangThai])
        VALUES (@NewMaTaiKhoan, @HoTen, @NgaySinh, @GioiTinh, @SoDienThoai, @Email, @DiaChi, CAST(GETDATE() AS DATE), N'Hoạt động');

        COMMIT TRANSACTION;
        SET @KetQua = 1;
        SET @ThongBao = N'Đăng ký tài khoản độc giả thành công!';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @KetQua = 0;
        SET @ThongBao = ERROR_MESSAGE();
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_KiemTraDangNhap]
    @TenDangNhap NVARCHAR(50),
    @MatKhau     NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        tk.[MaTaiKhoan],
        tk.[TenDangNhap],
        tk.[HoTen],
        tk.[VaiTro],
        tk.[TrangThai] AS [TrangThaiTaiKhoan],
        dg.[MaDocGia],
        dg.[Email],
        dg.[SoDienThoai],
        dg.[TrangThai] AS [TrangThaiDocGia]
    FROM [dbo].[TaiKhoans] tk
    LEFT JOIN [dbo].[DocGias] dg ON tk.[MaTaiKhoan] = dg.[MaTaiKhoan]
    WHERE tk.[TenDangNhap] = @TenDangNhap
      AND tk.[MatKhau] = @MatKhau;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_ThongKeTongQuanDashboard]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        (SELECT COUNT(*) FROM [dbo].[Sachs]) AS [TongSoDauSach],
        (SELECT ISNULL(SUM([SoLuong]), 0) FROM [dbo].[Sachs]) AS [TongSoBanSach],
        (SELECT ISNULL(SUM([SoLuongCon]), 0) FROM [dbo].[Sachs]) AS [TongSoSachCon],
        (SELECT COUNT(*) FROM [dbo].[DocGias]) AS [TongSoDocGia],
        (SELECT COUNT(*) FROM [dbo].[PhieuMuons] WHERE [TrangThai] = N'Đang mượn') AS [TongPhieuDangMuon],
        (SELECT COUNT(*) FROM [dbo].[PhieuMuons] WHERE [TrangThai] = N'Chờ xác nhận') AS [TongPhieuChoDuyet],
        (SELECT COUNT(*) FROM [dbo].[PhieuMuons] WHERE [TrangThai] = N'Đã trả') AS [TongPhieuDaTra],
        (SELECT COUNT(*) FROM [dbo].[PhieuMuons] WHERE [TrangThai] = N'Đang mượn' AND [HanTra] < CAST(GETDATE() AS DATE)) AS [TongPhieuQuaHan],
        (SELECT ISNULL(SUM([SoTien]), 0) FROM [dbo].[GiaoDichThanhToans] WHERE [HuongDongTien] = N'Nguồn vào' AND [TrangThai] = N'Đã hoàn thành') AS [TongNguonVao],
        (SELECT ISNULL(SUM([SoTien]), 0) FROM [dbo].[GiaoDichThanhToans] WHERE [HuongDongTien] = N'Nguồn ra' AND [TrangThai] = N'Đã hoàn thành') AS [TongNguonRa],
        (SELECT (ISNULL(SUM(CASE WHEN [HuongDongTien] = N'Nguồn vào' THEN [SoTien] ELSE -[SoTien] END), 0)) FROM [dbo].[GiaoDichThanhToans] WHERE [TrangThai] = N'Đã hoàn thành') AS [TonQuyTien];
END;
GO

PRINT N'';
PRINT N'================================================================================';
PRINT N'==> Hoàn tất khởi tạo Cơ sở dữ liệu QuanLyThuVienDb.';
PRINT N'================================================================================';
PRINT N'';

SELECT 'TaiKhoans' AS [TenBang], COUNT(*) AS [SoLuongBanGhi] FROM [dbo].[TaiKhoans]
UNION ALL
SELECT 'TheLoais', COUNT(*) FROM [dbo].[TheLoais]
UNION ALL
SELECT 'Sachs', COUNT(*) FROM [dbo].[Sachs]
UNION ALL
SELECT 'DocGias', COUNT(*) FROM [dbo].[DocGias]
UNION ALL
SELECT 'PhieuMuons', COUNT(*) FROM [dbo].[PhieuMuons]
UNION ALL
SELECT 'ChiTietPhieuMuons', COUNT(*) FROM [dbo].[ChiTietPhieuMuons]
UNION ALL
SELECT 'GiaoDichThanhToans', COUNT(*) FROM [dbo].[GiaoDichThanhToans];

SELECT [MaTaiKhoan], [TenDangNhap], [HoTen], [VaiTro], [TrangThai]
FROM [dbo].[TaiKhoans]
ORDER BY [VaiTro] ASC, [MaTaiKhoan] ASC;
GO
