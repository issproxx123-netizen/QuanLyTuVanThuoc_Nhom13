using Microsoft.EntityFrameworkCore;
using QuanLyTuVanThuoc_Nhom13.Data;
using QuanLyTuVanThuoc_Nhom13.Models;
using QuanLyTuVanThuoc_Nhom13.Services;

namespace QuanLyTuVanThuoc_Nhom13.Tests;

public class WarningServiceIntegrationTests
{
    private static ApplicationDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task RefreshMedicineWarnings_LowStock_CreatesHighWarning()
    {
        await using var db = CreateDb();

        var medicine = new Thuoc
        {
            MaThuoc = 1,
            TenThuoc = "Paracetamol",
            DonViTinh = "viên",
            SoLuongTon = 5,
            TrangThai = true,
            HanSuDung = DateTime.Today.AddYears(1)
        };

        db.Thuocs.Add(medicine);
        await db.SaveChangesAsync();

        var service = new WarningService(db);
        await service.RefreshMedicineWarningsAsync(medicine);

        var warning = await db.CanhBaos.SingleAsync();

        Assert.Equal("Tồn kho thấp", warning.LoaiCanhBao);
        Assert.Equal("Cao", warning.MucDo);
    }

    [Fact]
    public async Task RefreshMedicineWarnings_NearExpiry_CreatesExpiryWarning()
    {
        await using var db = CreateDb();

        var medicine = new Thuoc
        {
            MaThuoc = 2,
            TenThuoc = "Loratadine",
            DonViTinh = "viên",
            SoLuongTon = 100,
            TrangThai = true,
            HanSuDung = DateTime.Today.AddDays(30)
        };

        db.Thuocs.Add(medicine);
        await db.SaveChangesAsync();

        var service = new WarningService(db);
        await service.RefreshMedicineWarningsAsync(medicine);

        var warning = await db.CanhBaos.SingleAsync();

        Assert.Equal("Sắp hết hạn", warning.LoaiCanhBao);
        Assert.Equal("Trung bình", warning.MucDo);
    }
}