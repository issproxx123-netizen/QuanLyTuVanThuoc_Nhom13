using Microsoft.EntityFrameworkCore;
using QuanLyTuVanThuoc_Nhom13.Data;
using QuanLyTuVanThuoc_Nhom13.Models;

namespace QuanLyTuVanThuoc_Nhom13.Services;

public class WarningService
{
    private const int LowStockThreshold = 50;
    private const int CriticalStockThreshold = 10;
    private const int ExpiryWarningDays = 90;

    private readonly ApplicationDbContext _db;

    public WarningService(ApplicationDbContext db) => _db = db;

    public async Task RefreshAllMedicineWarningsAsync()
    {
        var medicines = await _db.Thuocs.ToListAsync();
        foreach (var medicine in medicines)
        {
            await RefreshMedicineWarningsAsync(medicine, saveChanges: false);
        }

        await _db.SaveChangesAsync();
    }

    public async Task RefreshMedicineWarningsAsync(Thuoc medicine, bool saveChanges = true)
    {
        var today = DateTime.Today;
        var soon = today.AddDays(ExpiryWarningDays);

        var shouldWarnStock = medicine.TrangThai && medicine.SoLuongTon <= LowStockThreshold;
        var stockContent = $"Thuốc {medicine.TenThuoc} còn {medicine.SoLuongTon} {medicine.DonViTinh}.";
        var stockSeverity = medicine.SoLuongTon <= CriticalStockThreshold ? "Cao" : "Trung bình";
        await SyncMedicineWarningAsync(
            medicine,
            "Tồn kho thấp",
            shouldWarnStock,
            stockContent,
            stockSeverity);

        var shouldWarnExpiry = medicine.TrangThai
            && medicine.HanSuDung.HasValue
            && medicine.HanSuDung.Value.Date <= soon;

        var expiryContent = medicine.HanSuDung.HasValue && medicine.HanSuDung.Value.Date < today
            ? $"Thuốc {medicine.TenThuoc} đã hết hạn ngày {medicine.HanSuDung:dd/MM/yyyy}."
            : $"Thuốc {medicine.TenThuoc} sắp hết hạn ngày {medicine.HanSuDung:dd/MM/yyyy}.";
        var expirySeverity = medicine.HanSuDung.HasValue && medicine.HanSuDung.Value.Date < today
            ? "Cao"
            : "Trung bình";

        await SyncMedicineWarningAsync(
            medicine,
            "Sắp hết hạn",
            shouldWarnExpiry,
            expiryContent,
            expirySeverity);

        if (saveChanges)
        {
            await _db.SaveChangesAsync();
        }
    }

    public async Task LogAllergyWarningAsync(BenhNhan patient, Thuoc medicine, string reason)
    {
        var from = DateTime.Now.AddHours(-24);
        var warning = await _db.CanhBaos
            .Where(x => x.MaBenhNhan == patient.MaBenhNhan
                && x.MaThuoc == medicine.MaThuoc
                && x.LoaiCanhBao == "Dị ứng thuốc"
                && x.NgayTao >= from)
            .OrderByDescending(x => x.NgayTao)
            .FirstOrDefaultAsync();

        var content = $"Bệnh nhân {patient.HoTen}: {reason}";
        if (warning == null)
        {
            _db.CanhBaos.Add(new CanhBao
            {
                MaBenhNhan = patient.MaBenhNhan,
                MaThuoc = medicine.MaThuoc,
                LoaiCanhBao = "Dị ứng thuốc",
                NoiDung = content,
                MucDo = "Cao",
                NgayTao = DateTime.Now
            });
        }
        else
        {
            warning.NoiDung = content;
            warning.MucDo = "Cao";
            warning.NgayTao = DateTime.Now;
        }

        await _db.SaveChangesAsync();
    }

    private async Task SyncMedicineWarningAsync(
        Thuoc medicine,
        string type,
        bool shouldExist,
        string content,
        string severity)
    {
        var warnings = await _db.CanhBaos
            .Where(x => x.MaThuoc == medicine.MaThuoc && x.LoaiCanhBao == type)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync();

        if (!shouldExist)
        {
            if (warnings.Count > 0)
            {
                _db.CanhBaos.RemoveRange(warnings);
            }
            return;
        }

        var warning = warnings.FirstOrDefault();
        if (warning == null)
        {
            _db.CanhBaos.Add(new CanhBao
            {
                MaThuoc = medicine.MaThuoc,
                LoaiCanhBao = type,
                NoiDung = content,
                MucDo = severity,
                NgayTao = DateTime.Now
            });
        }
        else
        {
            if (!string.Equals(warning.NoiDung, content, StringComparison.Ordinal)
                || !string.Equals(warning.MucDo, severity, StringComparison.Ordinal))
            {
                warning.NoiDung = content;
                warning.MucDo = severity;
                warning.NgayTao = DateTime.Now;
            }

            if (warnings.Count > 1)
            {
                _db.CanhBaos.RemoveRange(warnings.Skip(1));
            }
        }
    }
}
