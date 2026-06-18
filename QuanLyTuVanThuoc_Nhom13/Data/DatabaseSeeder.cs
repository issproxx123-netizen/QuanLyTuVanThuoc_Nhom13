using QuanLyTuVanThuoc_Nhom13.Models;
using QuanLyTuVanThuoc_Nhom13.Security;

namespace QuanLyTuVanThuoc_Nhom13.Data;

public static class DatabaseSeeder
{
    public static void Seed(ApplicationDbContext db)
    {
        if (!db.VaiTros.Any())
        {
            db.VaiTros.AddRange(
                new VaiTro { TenVaiTro = "Admin" },
                new VaiTro { TenVaiTro = "Bác sĩ / Nhân viên tư vấn" },
                new VaiTro { TenVaiTro = "Nhân viên kho" },
                new VaiTro { TenVaiTro = "Quản lý" });
            db.SaveChanges();
        }

        if (!db.NguoiDungs.Any())
        {
            int RoleId(string name) => db.VaiTros.Single(x => x.TenVaiTro == name).MaVaiTro;
            db.NguoiDungs.AddRange(
                new NguoiDung { HoTen = "Quản trị viên", TenDangNhap = "admin", MatKhau = PasswordHelper.Hash("123456"), Email = "admin@hutech.edu.vn", SoDienThoai = "0900000001", MaVaiTro = RoleId("Admin") },
                new NguoiDung { HoTen = "Bác sĩ An", TenDangNhap = "bacsi", MatKhau = PasswordHelper.Hash("123456"), Email = "bacsi@hutech.edu.vn", SoDienThoai = "0900000002", MaVaiTro = RoleId("Bác sĩ / Nhân viên tư vấn") },
                new NguoiDung { HoTen = "Nhân viên kho Dược", TenDangNhap = "kho", MatKhau = PasswordHelper.Hash("123456"), Email = "kho@hutech.edu.vn", SoDienThoai = "0900000003", MaVaiTro = RoleId("Nhân viên kho") },
                new NguoiDung { HoTen = "Quản lý phòng khám", TenDangNhap = "quanly", MatKhau = PasswordHelper.Hash("123456"), Email = "quanly@hutech.edu.vn", SoDienThoai = "0900000004", MaVaiTro = RoleId("Quản lý") });
            db.SaveChanges();
        }
        else
        {
            foreach (var user in db.NguoiDungs.Where(x => !x.MatKhau.StartsWith("AQAAAA")))
                user.MatKhau = PasswordHelper.Hash(user.MatKhau);
            db.SaveChanges();
        }

        if (!db.BenhNhans.Any())
        {
            db.BenhNhans.AddRange(
                new BenhNhan { HoTen = "Nguyễn Văn A", GioiTinh = "Nam", NgaySinh = new DateTime(2003, 5, 12), SoDienThoai = "0912345678", DiaChi = "TP.HCM", TienSuBenh = "Đau dạ dày", DiUng = "Dị ứng penicillin" },
                new BenhNhan { HoTen = "Trần Thị B", GioiTinh = "Nữ", NgaySinh = new DateTime(2004, 8, 20), SoDienThoai = "0987654321", DiaChi = "TP.HCM", TienSuBenh = "Không có", DiUng = "Không có" },
                new BenhNhan { HoTen = "Lê Minh C", GioiTinh = "Nam", NgaySinh = new DateTime(2002, 11, 10), SoDienThoai = "0901122334", DiaChi = "Bình Dương", TienSuBenh = "Viêm xoang", DiUng = "Dị ứng hải sản" });
            db.SaveChanges();
        }

        if (!db.LoaiThuocs.Any())
        {
            db.LoaiThuocs.AddRange(
                new LoaiThuoc { TenLoaiThuoc = "Kháng sinh", MoTa = "Thuốc điều trị nhiễm khuẩn" },
                new LoaiThuoc { TenLoaiThuoc = "Giảm đau", MoTa = "Thuốc giảm đau thông thường" },
                new LoaiThuoc { TenLoaiThuoc = "Hạ sốt", MoTa = "Thuốc giúp hạ sốt" },
                new LoaiThuoc { TenLoaiThuoc = "Vitamin", MoTa = "Thuốc bổ sung vitamin" },
                new LoaiThuoc { TenLoaiThuoc = "Dị ứng", MoTa = "Thuốc hỗ trợ điều trị dị ứng" },
                new LoaiThuoc { TenLoaiThuoc = "Tiêu hóa", MoTa = "Thuốc hỗ trợ tiêu hóa" });
            db.SaveChanges();
        }

        if (!db.Thuocs.Any())
        {
            int TypeId(string name) => db.LoaiThuocs.Single(x => x.TenLoaiThuoc == name).MaLoaiThuoc;
            db.Thuocs.AddRange(
                new Thuoc { TenThuoc = "Paracetamol", MaLoaiThuoc = TypeId("Hạ sốt"), DonViTinh = "Viên", HamLuong = "500mg", CongDung = "Hạ sốt, giảm đau", CachDung = "Uống sau ăn", ChongChiDinh = "Dị ứng paracetamol, bệnh gan nặng", SoLuongTon = 100, HanSuDung = DateTime.Today.AddYears(2), GiaBan = 2000 },
                new Thuoc { TenThuoc = "Amoxicillin", MaLoaiThuoc = TypeId("Kháng sinh"), DonViTinh = "Viên", HamLuong = "500mg", CongDung = "Điều trị nhiễm khuẩn", CachDung = "Theo chỉ định bác sĩ", ChongChiDinh = "Dị ứng penicillin", SoLuongTon = 80, HanSuDung = DateTime.Today.AddYears(1), GiaBan = 3000 },
                new Thuoc { TenThuoc = "Vitamin C", MaLoaiThuoc = TypeId("Vitamin"), DonViTinh = "Viên", HamLuong = "500mg", CongDung = "Bổ sung vitamin C", CachDung = "Uống sau ăn", ChongChiDinh = "Sỏi thận nặng", SoLuongTon = 150, HanSuDung = DateTime.Today.AddYears(2), GiaBan = 1500 },
                new Thuoc { TenThuoc = "Loratadine", MaLoaiThuoc = TypeId("Dị ứng"), DonViTinh = "Viên", HamLuong = "10mg", CongDung = "Giảm triệu chứng dị ứng", CachDung = "1 viên/ngày", ChongChiDinh = "Mẫn cảm với loratadine", SoLuongTon = 60, HanSuDung = DateTime.Today.AddYears(1), GiaBan = 2500 },
                new Thuoc { TenThuoc = "Omeprazole", MaLoaiThuoc = TypeId("Tiêu hóa"), DonViTinh = "Viên", HamLuong = "20mg", CongDung = "Giảm tiết acid dạ dày", CachDung = "Uống trước ăn", ChongChiDinh = "Mẫn cảm với omeprazole", SoLuongTon = 45, HanSuDung = DateTime.Today.AddMonths(2), GiaBan = 3500 });
            db.SaveChanges();
        }

        if (!db.DonTuVans.Any())
        {
            var doctor = db.NguoiDungs.First(x => x.TenDangNhap == "bacsi");
            var patient1 = db.BenhNhans.OrderBy(x => x.MaBenhNhan).First();
            var patient2 = db.BenhNhans.OrderBy(x => x.MaBenhNhan).Skip(1).First();
            var paracetamol = db.Thuocs.First(x => x.TenThuoc == "Paracetamol");
            var loratadine = db.Thuocs.First(x => x.TenThuoc == "Loratadine");
            var d1 = new DonTuVan { MaBenhNhan = patient1.MaBenhNhan, MaNguoiDung = doctor.MaNguoiDung, NgayTuVan = DateTime.Today.AddDays(-1).AddHours(9), TrieuChung = "Sốt, đau đầu", ChanDoan = "Cảm sốt thông thường", GhiChu = "Theo dõi nhiệt độ" };
            var d2 = new DonTuVan { MaBenhNhan = patient2.MaBenhNhan, MaNguoiDung = doctor.MaNguoiDung, NgayTuVan = DateTime.Today.AddHours(10), TrieuChung = "Hắt hơi, ngứa mũi", ChanDoan = "Dị ứng thời tiết", GhiChu = "Tránh bụi" };
            db.DonTuVans.AddRange(d1, d2);
            db.SaveChanges();
            db.ChiTietDonTuVans.AddRange(
                new ChiTietDonTuVan { MaDonTuVan = d1.MaDonTuVan, MaThuoc = paracetamol.MaThuoc, SoLuong = 6, LieuDung = "1 viên/lần, ngày 2 lần", CachDung = "Sau ăn", SoNgayDung = 3 },
                new ChiTietDonTuVan { MaDonTuVan = d2.MaDonTuVan, MaThuoc = loratadine.MaThuoc, SoLuong = 5, LieuDung = "1 viên/ngày", CachDung = "Sau ăn tối", SoNgayDung = 5 });
            db.SaveChanges();
        }

        if (!db.PhieuNhapKhos.Any())
        {
            var warehouse = db.NguoiDungs.First(x => x.TenDangNhap == "kho");
            var receipt = new PhieuNhapKho { MaNguoiDung = warehouse.MaNguoiDung, NhaCungCap = "Công ty Dược HUTECH", GhiChu = "Phiếu nhập mẫu đầu kỳ", NgayNhap = DateTime.Today.AddDays(-3) };
            db.PhieuNhapKhos.Add(receipt);
            db.SaveChanges();
            var medicine = db.Thuocs.First();
            db.ChiTietPhieuNhaps.Add(new ChiTietPhieuNhap { MaPhieuNhap = receipt.MaPhieuNhap, MaThuoc = medicine.MaThuoc, SoLuongNhap = 50, DonGiaNhap = 1200, HanSuDung = medicine.HanSuDung });
            db.SaveChanges();
        }

        if (!db.CanhBaos.Any())
        {
            var low = db.Thuocs.OrderBy(x => x.SoLuongTon).First();
            db.CanhBaos.Add(new CanhBao { MaThuoc = low.MaThuoc, LoaiCanhBao = "Tồn kho thấp", NoiDung = $"Thuốc {low.TenThuoc} còn {low.SoLuongTon} {low.DonViTinh}.", MucDo = "Trung bình" });
            db.SaveChanges();
        }
    }
}
