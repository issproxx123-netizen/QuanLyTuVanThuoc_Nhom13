using Microsoft.EntityFrameworkCore;
using QuanLyTuVanThuoc_Nhom13.Models;

namespace QuanLyTuVanThuoc_Nhom13.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<VaiTro> VaiTros => Set<VaiTro>();
    public DbSet<NguoiDung> NguoiDungs => Set<NguoiDung>();
    public DbSet<BenhNhan> BenhNhans => Set<BenhNhan>();
    public DbSet<LoaiThuoc> LoaiThuocs => Set<LoaiThuoc>();
    public DbSet<Thuoc> Thuocs => Set<Thuoc>();
    public DbSet<DonTuVan> DonTuVans => Set<DonTuVan>();
    public DbSet<ChiTietDonTuVan> ChiTietDonTuVans => Set<ChiTietDonTuVan>();
    public DbSet<CanhBao> CanhBaos => Set<CanhBao>();
    public DbSet<PhieuNhapKho> PhieuNhapKhos => Set<PhieuNhapKho>();
    public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps => Set<ChiTietPhieuNhap>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VaiTro>().ToTable("VaiTro");
        modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");
        modelBuilder.Entity<BenhNhan>().ToTable("BenhNhan");
        modelBuilder.Entity<LoaiThuoc>().ToTable("LoaiThuoc");
        modelBuilder.Entity<Thuoc>().ToTable("Thuoc");
        modelBuilder.Entity<DonTuVan>().ToTable("DonTuVan");
        modelBuilder.Entity<ChiTietDonTuVan>().ToTable("ChiTietDonTuVan");
        modelBuilder.Entity<CanhBao>().ToTable("CanhBao");
        modelBuilder.Entity<PhieuNhapKho>().ToTable("PhieuNhapKho");
        modelBuilder.Entity<ChiTietPhieuNhap>().ToTable("ChiTietPhieuNhap");

        modelBuilder.Entity<NguoiDung>().HasIndex(x => x.TenDangNhap).IsUnique();
        modelBuilder.Entity<VaiTro>().HasIndex(x => x.TenVaiTro).IsUnique();
        modelBuilder.Entity<LoaiThuoc>().HasIndex(x => x.TenLoaiThuoc).IsUnique();

        modelBuilder.Entity<NguoiDung>()
            .HasOne(x => x.VaiTro)
            .WithMany(x => x.NguoiDungs)
            .HasForeignKey(x => x.MaVaiTro)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Thuoc>()
            .HasOne(x => x.LoaiThuoc)
            .WithMany(x => x.Thuocs)
            .HasForeignKey(x => x.MaLoaiThuoc)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DonTuVan>()
            .HasOne(x => x.BenhNhan)
            .WithMany(x => x.DonTuVans)
            .HasForeignKey(x => x.MaBenhNhan)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DonTuVan>()
            .HasOne(x => x.NguoiDung)
            .WithMany(x => x.DonTuVans)
            .HasForeignKey(x => x.MaNguoiDung)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChiTietDonTuVan>()
            .HasOne(x => x.DonTuVan)
            .WithMany(x => x.ChiTietDonTuVans)
            .HasForeignKey(x => x.MaDonTuVan)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChiTietDonTuVan>()
            .HasOne(x => x.Thuoc)
            .WithMany(x => x.ChiTietDonTuVans)
            .HasForeignKey(x => x.MaThuoc)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CanhBao>()
            .HasOne(x => x.BenhNhan)
            .WithMany(x => x.CanhBaos)
            .HasForeignKey(x => x.MaBenhNhan)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<CanhBao>()
            .HasOne(x => x.Thuoc)
            .WithMany(x => x.CanhBaos)
            .HasForeignKey(x => x.MaThuoc)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<PhieuNhapKho>()
            .HasOne(x => x.NguoiDung)
            .WithMany(x => x.PhieuNhapKhos)
            .HasForeignKey(x => x.MaNguoiDung)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChiTietPhieuNhap>()
            .HasOne(x => x.PhieuNhapKho)
            .WithMany(x => x.ChiTietPhieuNhaps)
            .HasForeignKey(x => x.MaPhieuNhap)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChiTietPhieuNhap>()
            .HasOne(x => x.Thuoc)
            .WithMany(x => x.ChiTietPhieuNhaps)
            .HasForeignKey(x => x.MaThuoc)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Thuoc>().HasIndex(x => x.TenThuoc);
        modelBuilder.Entity<BenhNhan>().HasIndex(x => x.SoDienThoai);
        modelBuilder.Entity<DonTuVan>().HasIndex(x => x.NgayTuVan);
        modelBuilder.Entity<CanhBao>().HasIndex(x => x.NgayTao);

    }
}
