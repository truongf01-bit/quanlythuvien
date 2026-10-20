using System;
using System.IO;
using System.Text;

namespace QuanLyThuVien.Data
{
    public static class AssetGenerator
    {
        public static void TaoTaiNguyenAnhSan(string webRootPath)
        {
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var booksDir = Path.Combine(webRootPath, "images", "books");
            var bannersDir = Path.Combine(webRootPath, "images", "banners");
            var categoriesDir = Path.Combine(webRootPath, "images", "categories");
            var avatarsDir = Path.Combine(webRootPath, "images", "avatars");

            Directory.CreateDirectory(booksDir);
            Directory.CreateDirectory(bannersDir);
            Directory.CreateDirectory(categoriesDir);
            Directory.CreateDirectory(avatarsDir);

            var bookInfo = new (int Id, string Title, string Sub, string Author, string Cat, string Nxb, string Grad1, string Grad2, string Accent, string Icon)[]
            {
                (1, "Lập trình Web ASP.NET Core 10", "Kiến trúc MVC & Entity Framework Core", "TS. Nguyễn Văn Hòa", "CÔNG NGHỆ THÔNG TIN", "NXB KHOA HỌC KỸ THUẬT", "#0f172a", "#1e3a8a", "#38bdf8", "MVC"),
                (2, "Làm chủ C# & Hướng Đối Tượng", "Từ Cơ Bản Đến Lập Trình Nâng Cao", "PGS.TS. Trần Minh Tuấn", "CÔNG NGHỆ THÔNG TIN", "NXB GIÁO DỤC VIỆT NAM", "#1e1b4b", "#4338ca", "#818cf8", "C#"),
                (3, "Hệ Quản Trị CSDL SQL Server", "Thiết Kế, T-SQL & Tối Ưu Hiệu Năng", "ThS. Lê Hoàng Việt", "CÔNG NGHỆ THÔNG TIN", "NXB KHOA HỌC KỸ THUẬT", "#09090b", "#0284c7", "#38bdf8", "SQL"),
                (4, "Cấu Trúc Dữ Liệu & Giải Thuật", "Danh Sách, Cây, Đồ Thị & Thuật Toán", "TS. Phạm Văn Ất", "CÔNG NGHỆ THÔNG TIN", "NXB GIÁO DỤC VIỆT NAM", "#0f291e", "#047857", "#34d399", "ALGO"),
                (5, "Nhập Môn Trí Tuệ Nhân Tạo", "Machine Learning & Deep Learning", "TS. Đỗ Quang Vinh", "CÔNG NGHỆ THÔNG TIN", "NXB TRẺ", "#2e1065", "#7c3aed", "#c084fc", "AI"),
                (6, "An Toàn & Bảo Mật Hệ Thống", "Mã Hóa, Tường Lửa & Phòng Chống Tấn Công", "ThS. Vũ Đình Khoa", "CÔNG NGHỆ THÔNG TIN", "NXB THÔNG TIN VÀ TRUYỀN THÔNG", "#1c1917", "#b91c1c", "#f87171", "SEC"),

                (7, "Nguyên Lý Quản Trị Kinh Doanh", "Hoạch Định Chiến Lược & Điều Hành", "GS.TS. Nguyễn Thành Độ", "KINH TẾ - QUẢN TRỊ", "NXB LAO ĐỘNG", "#1e293b", "#0369a1", "#bae6fd", "BIZ"),
                (8, "Marketing Căn Bản & Thực Hành", "Hành Vi Tiêu Dùng & Chiến Lược 4P", "TS. Trần Thị Phương", "KINH TẾ - QUẢN TRỊ", "NXB TỔNG HỢP TP.HCM", "#3f2c00", "#d97706", "#fde68a", "MKT"),
                (9, "Kinh Tế Học Vi Mô & Vĩ Mô", "Quy Luật Cung Cầu & Chính Sách Tài Khóa", "PGS.TS. Vũ Kim Dũng", "KINH TẾ - QUẢN TRỊ", "NXB GIÁO DỤC", "#042f2e", "#0d9488", "#99f6e4", "ECON"),
                (10, "Phân Tích Báo Cáo Tài Chính", "Đọc Hiểu Bảng Cân Đối & Dòng Tiền", "TS. Nguyễn Năng Phúc", "KINH TẾ - QUẢN TRỊ", "NXB TÀI CHÍNH", "#14532d", "#15803d", "#86efac", "FIN"),

                (11, "Mắt Biếc", "Truyện Dài Kinh Điển Tuổi Học Trò", "Nguyễn Nhật Ánh", "VĂN HỌC VIỆT NAM", "NXB TRẺ", "#1e1b4b", "#312e81", "#a5b4fc", "LIT"),
                (12, "Cho Tôi Xin Một Vé Đi Tuổi Thơ", "Tác Phẩm Văn Học Đoạt Giải Thưởng", "Nguyễn Nhật Ánh", "VĂN HỌC VIỆT NAM", "NXB TRẺ", "#78350f", "#b45309", "#fde68a", "LIT"),
                (13, "Dế Mèn Phiêu Lưu Ký", "Truyện Thiếu Nhi Kinh Điển Việt Nam", "Tô Hoài", "VĂN HỌC VIỆT NAM", "NXB KIM ĐỒNG", "#14532d", "#166534", "#86efac", "LIT"),
                (14, "Tuyển Tập Truyện Ngắn Nam Cao", "Chí Phèo - Lão Hạc - Đời Thừa", "Nam Cao", "VĂN HỌC VIỆT NAM", "NXB VĂN HỌC", "#451a03", "#9a3412", "#fed7aa", "LIT"),

                (15, "Luyện Thi TOEIC 750+", "Cẩm Nang Trọng Tâm Nghe - Đọc", "ThS. Mai Lan Hương", "NGOẠI NGỮ - KỸ NĂNG", "NXB TỔNG HỢP TP.HCM", "#1e3a8a", "#2563eb", "#93c5fd", "ENG"),
                (16, "Tiếng Anh Chuyên Ngành CNTT", "Thuật Ngữ & Kỹ Năng Đọc Tài Liệu IT", "ThS. Hoàng Thị Minh", "NGOẠI NGỮ - KỸ NĂNG", "NXB GIÁO DỤC", "#0c4a6e", "#0284c7", "#7dd3fc", "IT"),
                (17, "Kỹ Năng Giao Tiếp & Thuyết Trình", "Làm Chủ Sân Khấu & Thuyết Phục", "TS. Lê Thẩm Dương", "NGOẠI NGỮ - KỸ NĂNG", "NXB LAO ĐỘNG", "#4a044e", "#9333ea", "#f0abfc", "SOFT"),
                (18, "Tư Duy Phản Biện Cho Sinh Viên", "Lập Luận Logic & Giải Quyết Vấn Đề", "ThS. Nguyễn Hoàng Khắc Hiếu", "NGOẠI NGỮ - KỸ NĂNG", "NXB TRẺ", "#111827", "#374151", "#e5e7eb", "CRIT"),

                (19, "Toán Cao Cấp Tập 1", "Đại Số Tuyến Tính & Không Gian Vectơ", "GS. Nguyễn Đình Trí", "KHOA HỌC CƠ BẢN", "NXB GIÁO DỤC", "#0f172a", "#1d4ed8", "#bfdbfe", "MATH"),
                (20, "Xác Suất Thống Kê & Ứng Dụng", "Phân Phối Xác Suất & Kiểm Định", "PGS.TS. Đào Hữu Hồ", "KHOA HỌC CƠ BẢN", "NXB GIÁO DỤC", "#022c22", "#059669", "#6ee7b7", "STAT"),
                (21, "Vật Lý Đại Cương Tập 2", "Điện Học & Từ Học Ứng Dụng", "GS. Lương Duyên Bình", "KHOA HỌC CƠ BẢN", "NXB GIÁO DỤC", "#1e1e24", "#4b5563", "#e2e8f0", "PHYS"),

                (22, "Cơ Sở Văn Hóa Việt Nam", "Nghiên Cứu Bản Sắc & Tiến Trình Lịch Sử", "GS.TS. Trần Ngọc Thêm", "LỊCH SỬ - VĂN HÓA", "NXB GIÁO DỤC", "#7f1d1d", "#b91c1c", "#fecaca", "CULT")
            };

            foreach (var b in bookInfo)
            {
                var filePath = Path.Combine(booksDir, $"book-{b.Id}.svg");
                var svg = $@"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 400 560"" width=""100%"" height=""100%"">
  <defs>
    <linearGradient id=""bgGrad{b.Id}"" x1=""0%"" y1=""0%"" x2=""100%"" y2=""100%"">
      <stop offset=""0%"" stop-color=""{b.Grad1}"" />
      <stop offset=""100%"" stop-color=""{b.Grad2}"" />
    </linearGradient>
    <linearGradient id=""spineGrad{b.Id}"" x1=""0%"" y1=""0%"" x2=""100%"" y2=""0%"">
      <stop offset=""0%"" stop-color=""rgba(0,0,0,0.55)"" />
      <stop offset=""35%"" stop-color=""rgba(255,255,255,0.2)"" />
      <stop offset=""70%"" stop-color=""rgba(0,0,0,0.15)"" />
      <stop offset=""100%"" stop-color=""rgba(0,0,0,0.6)"" />
    </linearGradient>
  </defs>

  <rect width=""400"" height=""560"" rx=""10"" fill=""url(#bgGrad{b.Id})"" />

  <!-- Background decorative geometric accents -->
  <circle cx=""350"" cy=""80"" r=""130"" fill=""{b.Accent}"" opacity=""0.08"" />
  <circle cx=""50"" cy=""460"" r=""150"" fill=""{b.Accent}"" opacity=""0.06"" />
  <rect x=""240"" y=""190"" width=""160"" height=""160"" rx=""20"" transform=""rotate(25 240 190)"" fill=""{b.Accent}"" opacity=""0.04"" />

  <!-- 3D book spine on left -->
  <rect x=""0"" y=""0"" width=""28"" height=""560"" rx=""4"" fill=""url(#spineGrad{b.Id})"" />
  <line x1=""28"" y1=""0"" x2=""28"" y2=""560"" stroke=""rgba(255,255,255,0.18)"" stroke-width=""1.5"" />

  <!-- Elegant inner border -->
  <rect x=""42"" y=""24"" width=""334"" height=""512"" rx=""6"" fill=""none"" stroke=""rgba(255,255,255,0.15)"" stroke-width=""1.2"" />

  <!-- Category badge -->
  <rect x=""52"" y=""38"" width=""210"" height=""26"" rx=""4"" fill=""{b.Accent}"" opacity=""0.2"" />
  <text x=""60"" y=""55"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""10.5"" font-weight=""700"" fill=""{b.Accent}"" letter-spacing=""1"">{b.Cat}</text>

  <!-- Icon stamp -->
  <g transform=""translate(315, 34)"">
    <circle cx=""16"" cy=""16"" r=""16"" fill=""rgba(255,255,255,0.08)"" stroke=""{b.Accent}"" stroke-width=""1"" />
    <text x=""16"" y=""21"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""10"" font-weight=""800"" fill=""{b.Accent}"" text-anchor=""middle"">{b.Icon}</text>
  </g>

  <line x1=""52"" y1=""78"" x2=""360"" y2=""78"" stroke=""{b.Accent}"" stroke-width=""2"" opacity=""0.5"" />

  <!-- Book Title -->
  <text x=""52"" y=""135"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""22"" font-weight=""800"" fill=""#ffffff"">
    {b.Title}
  </text>

  <!-- Subtitle -->
  <text x=""52"" y=""175"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""12.5"" font-weight=""400"" fill=""rgba(255,255,255,0.85)"">
    {b.Sub}
  </text>

  <!-- Center emblem -->
  <g transform=""translate(52, 215)"">
    <rect width=""296"" height=""175"" rx=""8"" fill=""rgba(255,255,255,0.05)"" stroke=""rgba(255,255,255,0.1)"" />
    <path d=""M 60,95 Q 110,80 148,100 Q 186,80 236,95 L 236,135 Q 186,120 148,140 Q 110,120 60,135 Z"" fill=""none"" stroke=""{b.Accent}"" stroke-width=""2.5"" />
    <line x1=""148"" y1=""100"" x2=""148"" y2=""140"" stroke=""{b.Accent}"" stroke-width=""2.5"" />
    <circle cx=""148"" cy=""60"" r=""18"" fill=""none"" stroke=""{b.Accent}"" stroke-width=""1.5"" opacity=""0.7"" />
    <text x=""148"" y=""65"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""12"" font-weight=""700"" fill=""{b.Accent}"" text-anchor=""middle"">★</text>
    <text x=""148"" y=""162"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""10"" font-weight=""600"" fill=""rgba(255,255,255,0.5)"" text-anchor=""middle"" letter-spacing=""1.5"">GIÁO TRÌNH HỌC THUẬT</text>
  </g>

  <!-- Author Info -->
  <g transform=""translate(52, 420)"">
    <text x=""0"" y=""12"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""10.5"" font-weight=""500"" fill=""rgba(255,255,255,0.6)"" letter-spacing=""0.5"">TÁC GIẢ</text>
    <text x=""0"" y=""34"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""15"" font-weight=""700"" fill=""#ffffff"">{b.Author}</text>
  </g>

  <!-- Footer / Publisher -->
  <line x1=""52"" y1=""480"" x2=""360"" y2=""480"" stroke=""rgba(255,255,255,0.15)"" stroke-width=""1"" />
  <text x=""206"" y=""508"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""11.5"" font-weight=""700"" fill=""{b.Accent}"" text-anchor=""middle"" letter-spacing=""1"">
    {b.Nxb}
  </text>
</svg>";
                File.WriteAllText(filePath, svg, Encoding.UTF8);
            }

            var defaultCoverPath = Path.Combine(booksDir, "default-book.svg");
            var defaultSvg = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 400 560"" width=""100%"" height=""100%"">
  <defs>
    <linearGradient id=""defGrad"" x1=""0%"" y1=""0%"" x2=""100%"" y2=""100%"">
      <stop offset=""0%"" stop-color=""#1e293b"" />
      <stop offset=""100%"" stop-color=""#0f172a"" />
    </linearGradient>
    <linearGradient id=""defSpine"" x1=""0%"" y1=""0%"" x2=""100%"" y2=""0%"">
      <stop offset=""0%"" stop-color=""rgba(0,0,0,0.5)"" />
      <stop offset=""40%"" stop-color=""rgba(255,255,255,0.2)"" />
      <stop offset=""100%"" stop-color=""rgba(0,0,0,0.5)"" />
    </linearGradient>
  </defs>
  <rect width=""400"" height=""560"" rx=""10"" fill=""url(#defGrad)"" />
  <rect x=""0"" y=""0"" width=""28"" height=""560"" rx=""4"" fill=""url(#defSpine)"" />
  <rect x=""42"" y=""24"" width=""334"" height=""512"" rx=""6"" fill=""none"" stroke=""rgba(255,255,255,0.15)"" stroke-width=""1.2"" />
  <g transform=""translate(52, 180)"">
    <circle cx=""148"" cy=""60"" r=""45"" fill=""rgba(255,255,255,0.05)"" stroke=""#38bdf8"" stroke-width=""2"" />
    <path d=""M 120,60 Q 148,45 148,75 Q 148,45 176,60"" fill=""none"" stroke=""#38bdf8"" stroke-width=""2.5"" />
    <text x=""148"" y=""140"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""20"" font-weight=""700"" fill=""#ffffff"" text-anchor=""middle"">SÁCH THƯ VIỆN</text>
    <text x=""148"" y=""170"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""13"" fill=""#94a3b8"" text-anchor=""middle"">Tài liệu học tập &amp; nghiên cứu</text>
  </g>
  <line x1=""52"" y1=""480"" x2=""360"" y2=""480"" stroke=""rgba(255,255,255,0.15)"" stroke-width=""1"" />
  <text x=""206"" y=""508"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""12"" font-weight=""600"" fill=""#38bdf8"" text-anchor=""middle"">THƯ VIỆN ĐẠI HỌC</text>
</svg>";
            File.WriteAllText(defaultCoverPath, defaultSvg, Encoding.UTF8);

            var heroBannerPath = Path.Combine(bannersDir, "library-hero.svg");
            var heroSvg = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 1200 400"" width=""100%"" height=""100%"">
  <defs>
    <linearGradient id=""heroGrad"" x1=""0%"" y1=""0%"" x2=""100%"" y2=""100%"">
      <stop offset=""0%"" stop-color=""#0a192f"" />
      <stop offset=""50%"" stop-color=""#1e3a8a"" />
      <stop offset=""100%"" stop-color=""#0369a1"" />
    </linearGradient>
  </defs>
  <rect width=""1200"" height=""400"" rx=""16"" fill=""url(#heroGrad)"" />
  <circle cx=""1050"" cy=""100"" r=""180"" fill=""#38bdf8"" opacity=""0.15"" />
  <circle cx=""950"" cy=""350"" r=""140"" fill=""#818cf8"" opacity=""0.1"" />
  <circle cx=""150"" cy=""350"" r=""120"" fill=""#38bdf8"" opacity=""0.08"" />
  <g transform=""translate(820, 80)"" opacity=""0.9"">
    <rect x=""0"" y=""220"" width=""300"" height=""14"" rx=""4"" fill=""#cbd5e1"" opacity=""0.5"" />
    <rect x=""25"" y=""50"" width=""36"" height=""170"" rx=""4"" fill=""#38bdf8"" />
    <rect x=""68"" y=""80"" width=""32"" height=""140"" rx=""4"" fill=""#fbbf24"" />
    <rect x=""108"" y=""40"" width=""42"" height=""180"" rx=""4"" fill=""#34d399"" />
    <rect x=""158"" y=""100"" width=""28"" height=""120"" rx=""4"" fill=""#f87171"" />
    <rect x=""194"" y=""60"" width=""40"" height=""160"" rx=""4"" fill=""#a78bfa"" />
    <rect x=""242"" y=""70"" width=""32"" height=""150"" rx=""4"" fill=""#f472b6"" transform=""rotate(14 242 220)"" />
  </g>
</svg>";
            File.WriteAllText(heroBannerPath, heroSvg, Encoding.UTF8);

            var loginArtPath = Path.Combine(bannersDir, "login-art.svg");
            var loginArtSvg = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 600 700"" width=""100%"" height=""100%"">
  <defs>
    <linearGradient id=""loginGrad"" x1=""0%"" y1=""0%"" x2=""100%"" y2=""100%"">
      <stop offset=""0%"" stop-color=""#0f172a"" />
      <stop offset=""60%"" stop-color=""#1e3a8a"" />
      <stop offset=""100%"" stop-color=""#1d4ed8"" />
    </linearGradient>
  </defs>
  <rect width=""600"" height=""700"" fill=""url(#loginGrad)"" />
  <circle cx=""500"" cy=""150"" r=""160"" fill=""#38bdf8"" opacity=""0.15"" />
  <circle cx=""100"" cy=""600"" r=""180"" fill=""#6366f1"" opacity=""0.12"" />

  <g transform=""translate(100, 120)"">
    <circle cx=""200"" cy=""100"" r=""80"" fill=""rgba(255,255,255,0.06)"" stroke=""#38bdf8"" stroke-width=""2"" />
    <path d=""M 140,100 Q 200,70 200,120 Q 200,70 260,100 L 260,160 Q 200,130 200,180 Q 200,130 140,160 Z"" fill=""none"" stroke=""#38bdf8"" stroke-width=""4"" stroke-linejoin=""round"" />
    <line x1=""200"" y1=""120"" x2=""200"" y2=""180"" stroke=""#38bdf8"" stroke-width=""4"" />

    <text x=""200"" y=""230"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""24"" font-weight=""800"" fill=""#ffffff"" text-anchor=""middle"">THƯ VIỆN ĐẠI HỌC</text>
    <text x=""200"" y=""265"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""13.5"" fill=""#93c5fd"" text-anchor=""middle"">CỔNG TRA CỨU &amp; MƯỢN TRẢ TÀI LIỆU</text>

    <g transform=""translate(0, 300)"">
      <rect x=""10"" y=""0"" width=""380"" height=""170"" rx=""12"" fill=""rgba(255,255,255,0.07)"" stroke=""rgba(255,255,255,0.12)"" />
      <text x=""30"" y=""38"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""14"" font-weight=""700"" fill=""#38bdf8"">📚 Không gian Tri thức &amp; Học tập</text>
      <text x=""30"" y=""72"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""13"" fill=""rgba(255,255,255,0.85)"">• Tra cứu nhanh hàng ngàn đầu sách chuyên ngành</text>
      <text x=""30"" y=""102"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""13"" fill=""rgba(255,255,255,0.85)"">• Đăng ký mượn sách trực tuyến thuận tiện, minh bạch</text>
      <text x=""30"" y=""132"" font-family=""'Segoe UI', Arial, sans-serif"" font-size=""13"" fill=""rgba(255,255,255,0.85)"">• Theo dõi hạn trả sách tự động, không lo trễ hạn</text>
    </g>
  </g>
</svg>";
            File.WriteAllText(loginArtPath, loginArtSvg, Encoding.UTF8);

            for (int i = 1; i <= 6; i++)
            {
                var catPath = Path.Combine(categoriesDir, $"cat-{i}.svg");
                string catColor = i switch
                {
                    1 => "#0284c7",
                    2 => "#d97706",
                    3 => "#7c3aed",
                    4 => "#2563eb",
                    5 => "#059669",
                    _ => "#dc2626"
                };
                string catIcon = i switch
                {
                    1 => "💻",
                    2 => "📈",
                    3 => "📖",
                    4 => "🌐",
                    5 => "🔬",
                    _ => "🏛️"
                };
                var catSvg = $@"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 100 100"" width=""100%"" height=""100%"">
  <rect width=""100"" height=""100"" rx=""16"" fill=""{catColor}"" opacity=""0.15"" />
  <rect x=""1"" y=""1"" width=""98"" height=""98"" rx=""15"" fill=""none"" stroke=""{catColor}"" stroke-width=""2"" opacity=""0.3"" />
  <text x=""50"" y=""62"" font-size=""38"" text-anchor=""middle"">{catIcon}</text>
</svg>";
                File.WriteAllText(catPath, catSvg, Encoding.UTF8);
            }

            var studentAvatarPath = Path.Combine(avatarsDir, "avatar-student.svg");
            var studentAvatarSvg = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 100 100"" width=""100%"" height=""100%"">
  <circle cx=""50"" cy=""50"" r=""50"" fill=""#e0e7ff"" />
  <circle cx=""50"" cy=""38"" r=""20"" fill=""#4338ca"" />
  <path d=""M 15 90 C 15 65 30 60 50 60 C 70 60 85 65 85 90 Z"" fill=""#4338ca"" />
</svg>";
            File.WriteAllText(studentAvatarPath, studentAvatarSvg, Encoding.UTF8);

            var adminAvatarPath = Path.Combine(avatarsDir, "avatar-admin.svg");
            var adminAvatarSvg = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 100 100"" width=""100%"" height=""100%"">
  <circle cx=""50"" cy=""50"" r=""50"" fill=""#fef3c7"" />
  <circle cx=""50"" cy=""38"" r=""20"" fill=""#d97706"" />
  <path d=""M 15 90 C 15 65 30 60 50 60 C 70 60 85 65 85 90 Z"" fill=""#d97706"" />
</svg>";
            File.WriteAllText(adminAvatarPath, adminAvatarSvg, Encoding.UTF8);
        }
    }
}
