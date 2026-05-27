/* ============================================================
   CSDL: QuanLyTuVanThuocDB
   Hệ thống Quản lý Tư vấn Thuốc - Nhóm 13
   Chạy trong SQL Server Management Studio hoặc Visual Studio SQL Server Object Explorer
   ============================================================ */

IF DB_ID(N'QuanLyTuVanThuocDB') IS NULL
BEGIN
    CREATE DATABASE QuanLyTuVanThuocDB;
END
GO

USE QuanLyTuVanThuocDB;
GO

IF OBJECT_ID(N'dbo.ChiTietPhieuNhap', N'U') IS NOT NULL DROP TABLE dbo.ChiTietPhieuNhap;
IF OBJECT_ID(N'dbo.PhieuNhapKho', N'U') IS NOT NULL DROP TABLE dbo.PhieuNhapKho;
IF OBJECT_ID(N'dbo.CanhBao', N'U') IS NOT NULL DROP TABLE dbo.CanhBao;
IF OBJECT_ID(N'dbo.ChiTietDonTuVan', N'U') IS NOT NULL DROP TABLE dbo.ChiTietDonTuVan;
IF OBJECT_ID(N'dbo.DonTuVan', N'U') IS NOT NULL DROP TABLE dbo.DonTuVan;
IF OBJECT_ID(N'dbo.Thuoc', N'U') IS NOT NULL DROP TABLE dbo.Thuoc;
IF OBJECT_ID(N'dbo.LoaiThuoc', N'U') IS NOT NULL DROP TABLE dbo.LoaiThuoc;
IF OBJECT_ID(N'dbo.BenhNhan', N'U') IS NOT NULL DROP TABLE dbo.BenhNhan;
IF OBJECT_ID(N'dbo.NguoiDung', N'U') IS NOT NULL DROP TABLE dbo.NguoiDung;
IF OBJECT_ID(N'dbo.VaiTro', N'U') IS NOT NULL DROP TABLE dbo.VaiTro;
GO

CREATE TABLE dbo.VaiTro (
    MaVaiTro INT IDENTITY(1,1) PRIMARY KEY,
    TenVaiTro NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE dbo.NguoiDung (
    MaNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    Email VARCHAR(100) NULL,
    SoDienThoai VARCHAR(20) NULL,
    MaVaiTro INT NOT NULL,
    TrangThai BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_NguoiDung_VaiTro FOREIGN KEY (MaVaiTro) REFERENCES dbo.VaiTro(MaVaiTro)
);

CREATE TABLE dbo.BenhNhan (
    MaBenhNhan INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10) NULL,
    NgaySinh DATE NULL,
    SoDienThoai VARCHAR(20) NULL,
    DiaChi NVARCHAR(255) NULL,
    TienSuBenh NVARCHAR(MAX) NULL,
    DiUng NVARCHAR(MAX) NULL,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE dbo.LoaiThuoc (
    MaLoaiThuoc INT IDENTITY(1,1) PRIMARY KEY,
    TenLoaiThuoc NVARCHAR(100) NOT NULL UNIQUE,
    MoTa NVARCHAR(255) NULL
);

CREATE TABLE dbo.Thuoc (
    MaThuoc INT IDENTITY(1,1) PRIMARY KEY,
    TenThuoc NVARCHAR(150) NOT NULL,
    MaLoaiThuoc INT NOT NULL,
    DonViTinh NVARCHAR(50) NULL,
    HamLuong NVARCHAR(100) NULL,
    CongDung NVARCHAR(MAX) NULL,
    CachDung NVARCHAR(MAX) NULL,
    ChongChiDinh NVARCHAR(MAX) NULL,
    SoLuongTon INT NOT NULL DEFAULT 0,
    HanSuDung DATE NULL,
    GiaBan DECIMAL(18,2) NOT NULL DEFAULT 0,
    TrangThai BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Thuoc_LoaiThuoc FOREIGN KEY (MaLoaiThuoc) REFERENCES dbo.LoaiThuoc(MaLoaiThuoc),
    CONSTRAINT CK_Thuoc_SoLuongTon CHECK (SoLuongTon >= 0),
    CONSTRAINT CK_Thuoc_GiaBan CHECK (GiaBan >= 0)
);

CREATE TABLE dbo.DonTuVan (
    MaDonTuVan INT IDENTITY(1,1) PRIMARY KEY,
    MaBenhNhan INT NOT NULL,
    MaNguoiDung INT NOT NULL,
    NgayTuVan DATETIME NOT NULL DEFAULT GETDATE(),
    TrieuChung NVARCHAR(MAX) NULL,
    ChanDoan NVARCHAR(MAX) NULL,
    GhiChu NVARCHAR(MAX) NULL,
    CONSTRAINT FK_DonTuVan_BenhNhan FOREIGN KEY (MaBenhNhan) REFERENCES dbo.BenhNhan(MaBenhNhan),
    CONSTRAINT FK_DonTuVan_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES dbo.NguoiDung(MaNguoiDung)
);

CREATE TABLE dbo.ChiTietDonTuVan (
    MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,
    MaDonTuVan INT NOT NULL,
    MaThuoc INT NOT NULL,
    SoLuong INT NOT NULL,
    LieuDung NVARCHAR(255) NULL,
    CachDung NVARCHAR(255) NULL,
    SoNgayDung INT NULL,
    CONSTRAINT FK_CTDonTuVan_DonTuVan FOREIGN KEY (MaDonTuVan) REFERENCES dbo.DonTuVan(MaDonTuVan),
    CONSTRAINT FK_CTDonTuVan_Thuoc FOREIGN KEY (MaThuoc) REFERENCES dbo.Thuoc(MaThuoc),
    CONSTRAINT CK_CTDonTuVan_SoLuong CHECK (SoLuong > 0)
);

CREATE TABLE dbo.CanhBao (
    MaCanhBao INT IDENTITY(1,1) PRIMARY KEY,
    MaBenhNhan INT NULL,
    MaThuoc INT NULL,
    LoaiCanhBao NVARCHAR(100) NOT NULL,
    NoiDung NVARCHAR(MAX) NOT NULL,
    MucDo NVARCHAR(50) NULL,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_CanhBao_BenhNhan FOREIGN KEY (MaBenhNhan) REFERENCES dbo.BenhNhan(MaBenhNhan),
    CONSTRAINT FK_CanhBao_Thuoc FOREIGN KEY (MaThuoc) REFERENCES dbo.Thuoc(MaThuoc)
);

CREATE TABLE dbo.PhieuNhapKho (
    MaPhieuNhap INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    NgayNhap DATETIME NOT NULL DEFAULT GETDATE(),
    NhaCungCap NVARCHAR(150) NULL,
    GhiChu NVARCHAR(MAX) NULL,
    CONSTRAINT FK_PhieuNhapKho_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES dbo.NguoiDung(MaNguoiDung)
);

CREATE TABLE dbo.ChiTietPhieuNhap (
    MaChiTietNhap INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuNhap INT NOT NULL,
    MaThuoc INT NOT NULL,
    SoLuongNhap INT NOT NULL,
    DonGiaNhap DECIMAL(18,2) NULL,
    HanSuDung DATE NULL,
    CONSTRAINT FK_CTPhieuNhap_PhieuNhapKho FOREIGN KEY (MaPhieuNhap) REFERENCES dbo.PhieuNhapKho(MaPhieuNhap),
    CONSTRAINT FK_CTPhieuNhap_Thuoc FOREIGN KEY (MaThuoc) REFERENCES dbo.Thuoc(MaThuoc),
    CONSTRAINT CK_CTPhieuNhap_SoLuongNhap CHECK (SoLuongNhap > 0)
);
GO

INSERT INTO dbo.VaiTro (TenVaiTro)
VALUES (N'Admin'), (N'Bác sĩ / Nhân viên tư vấn'), (N'Nhân viên kho'), (N'Quản lý');

INSERT INTO dbo.NguoiDung (HoTen, TenDangNhap, MatKhau, Email, SoDienThoai, MaVaiTro)
VALUES
(N'Quản trị viên', 'admin', '123456', 'admin@gmail.com', '0900000001', 1),
(N'Bác sĩ An', 'bacsi', '123456', 'bacsi@gmail.com', '0900000002', 2),
(N'Nhân viên kho Dược', 'kho', '123456', 'kho@gmail.com', '0900000003', 3),
(N'Quản lý phòng khám', 'quanly', '123456', 'quanly@gmail.com', '0900000004', 4);

INSERT INTO dbo.BenhNhan (HoTen, GioiTinh, NgaySinh, SoDienThoai, DiaChi, TienSuBenh, DiUng)
VALUES
(N'Nguyễn Văn A', N'Nam', '2003-05-12', '0912345678', N'TP.HCM', N'Đau dạ dày', N'Dị ứng penicillin'),
(N'Trần Thị B', N'Nữ', '2004-08-20', '0987654321', N'TP.HCM', N'Không có', N'Không có'),
(N'Lê Minh C', N'Nam', '2002-11-10', '0901122334', N'Bình Dương', N'Viêm xoang', N'Dị ứng hải sản');

INSERT INTO dbo.LoaiThuoc (TenLoaiThuoc, MoTa)
VALUES
(N'Kháng sinh', N'Thuốc dùng để điều trị nhiễm khuẩn'),
(N'Giảm đau', N'Thuốc giảm đau thông thường'),
(N'Hạ sốt', N'Thuốc giúp hạ sốt'),
(N'Vitamin', N'Thuốc bổ sung vitamin'),
(N'Dị ứng', N'Thuốc hỗ trợ điều trị dị ứng'),
(N'Tiêu hóa', N'Thuốc hỗ trợ tiêu hóa');

INSERT INTO dbo.Thuoc (TenThuoc, MaLoaiThuoc, DonViTinh, HamLuong, CongDung, CachDung, ChongChiDinh, SoLuongTon, HanSuDung, GiaBan)
VALUES
(N'Paracetamol', 3, N'Viên', N'500mg', N'Hạ sốt, giảm đau', N'Uống sau ăn', N'Dị ứng paracetamol, bệnh gan nặng', 100, '2027-12-31', 2000),
(N'Amoxicillin', 1, N'Viên', N'500mg', N'Kháng sinh điều trị nhiễm khuẩn', N'Uống theo chỉ định của bác sĩ', N'Dị ứng penicillin', 80, '2027-06-30', 3000),
(N'Vitamin C', 4, N'Viên', N'500mg', N'Bổ sung vitamin C', N'Uống sau ăn', N'Sỏi thận nặng', 150, '2028-01-01', 1500),
(N'Loratadine', 5, N'Viên', N'10mg', N'Giảm triệu chứng dị ứng', N'Uống 1 viên/ngày', N'Mẫn cảm với thành phần thuốc', 60, '2027-09-15', 2500),
(N'Omeprazole', 6, N'Viên', N'20mg', N'Giảm tiết acid dạ dày', N'Uống trước ăn', N'Mẫn cảm với omeprazole', 45, '2027-05-20', 3500);

INSERT INTO dbo.DonTuVan (MaBenhNhan, MaNguoiDung, TrieuChung, ChanDoan, GhiChu)
VALUES
(1, 2, N'Sốt, đau đầu', N'Cảm sốt thông thường', N'Tư vấn uống thuốc sau ăn và theo dõi nhiệt độ'),
(2, 2, N'Hắt hơi, ngứa mũi', N'Dị ứng thời tiết', N'Tránh bụi, uống nhiều nước');

INSERT INTO dbo.ChiTietDonTuVan (MaDonTuVan, MaThuoc, SoLuong, LieuDung, CachDung, SoNgayDung)
VALUES
(1, 1, 6, N'1 viên/lần, ngày 2 lần', N'Uống sau ăn', 3),
(1, 3, 3, N'1 viên/lần, ngày 1 lần', N'Uống sau ăn sáng', 3),
(2, 4, 5, N'1 viên/lần, ngày 1 lần', N'Uống sau ăn tối', 5);

INSERT INTO dbo.CanhBao (MaBenhNhan, MaThuoc, LoaiCanhBao, NoiDung, MucDo)
VALUES
(1, 2, N'Dị ứng thuốc', N'Bệnh nhân dị ứng penicillin, cần tránh Amoxicillin', N'Cao'),
(NULL, 5, N'Tồn kho thấp', N'Thuốc Omeprazole còn ít trong kho', N'Trung bình');

INSERT INTO dbo.PhieuNhapKho (MaNguoiDung, NhaCungCap, GhiChu)
VALUES (3, N'Công ty Dược HUTECH', N'Nhập thuốc đầu kỳ');

INSERT INTO dbo.ChiTietPhieuNhap (MaPhieuNhap, MaThuoc, SoLuongNhap, DonGiaNhap, HanSuDung)
VALUES
(1, 1, 50, 1200, '2027-12-31'),
(1, 3, 100, 900, '2028-01-01'),
(1, 4, 40, 1500, '2027-09-15');
GO

SELECT * FROM dbo.VaiTro;
SELECT * FROM dbo.NguoiDung;
SELECT * FROM dbo.BenhNhan;
SELECT * FROM dbo.LoaiThuoc;
SELECT * FROM dbo.Thuoc;
SELECT * FROM dbo.DonTuVan;
SELECT * FROM dbo.ChiTietDonTuVan;
SELECT * FROM dbo.CanhBao;
GO
