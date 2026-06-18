using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyTuVanThuoc_Nhom13.Data;
using QuanLyTuVanThuoc_Nhom13.ViewModels;
using System.Globalization;
using System.Text;

namespace QuanLyTuVanThuoc_Nhom13.Controllers;

[Authorize]
public class BaoCaoController : Controller
{
    private readonly ApplicationDbContext _db;
    public BaoCaoController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;
        var soon = today.AddDays(90);
        var start = today.AddDays(-6);

        var raw = await _db.DonTuVans
            .Where(x => x.NgayTuVan >= start)
            .GroupBy(x => x.NgayTuVan.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync();

        var points = Enumerable.Range(0, 7)
            .Select(i => new ChartPointViewModel
            {
                Label = start.AddDays(i).ToString("dd/MM"),
                Value = raw.FirstOrDefault(x => x.Date == start.AddDays(i))?.Count ?? 0
            })
            .ToList();

        var max = Math.Max(1, points.Max(x => x.Value));
        foreach (var point in points)
        {
            point.HeightPercent = Math.Max(8, point.Value * 100 / max);
        }

        var inventoryValue = await _db.Thuocs
            .Where(x => x.TrangThai)
            .Select(x => (decimal?)(x.GiaBan * x.SoLuongTon))
            .SumAsync() ?? 0;

        var vm = new BaoCaoViewModel
        {
            TongBenhNhan = await _db.BenhNhans.CountAsync(),
            TongThuoc = await _db.Thuocs.CountAsync(x => x.TrangThai),
            TongDon = await _db.DonTuVans.CountAsync(),
            TongCanhBao = await _db.CanhBaos.CountAsync(),
            ThuocTonKhoThap = await _db.Thuocs.CountAsync(x => x.TrangThai && x.SoLuongTon <= 50),
            ThuocSapHetHan = await _db.Thuocs.CountAsync(x =>
                x.TrangThai && x.HanSuDung != null && x.HanSuDung.Value.Date <= soon),
            TongPhieuNhap = await _db.PhieuNhapKhos.CountAsync(),
            GiaTriTonKho = inventoryValue,
            LuotTuVanTheoNgay = points,
            ThuocCanChuY = await _db.Thuocs
                .Include(x => x.LoaiThuoc)
                .Where(x => x.TrangThai
                    && (x.SoLuongTon <= 50
                        || (x.HanSuDung != null && x.HanSuDung.Value.Date <= soon)))
                .OrderBy(x => x.SoLuongTon)
                .ToListAsync(),
            DonGanDay = await _db.DonTuVans
                .Include(x => x.BenhNhan)
                .OrderByDescending(x => x.NgayTuVan)
                .Take(10)
                .ToListAsync(),
            PhieuNhapGanDay = await _db.PhieuNhapKhos
                .Include(x => x.NguoiDung)
                .Include(x => x.ChiTietPhieuNhaps)
                .OrderByDescending(x => x.NgayNhap)
                .Take(5)
                .ToListAsync()
        };

        return View(vm);
    }

    public async Task<FileResult> ExportCsv()
    {
        var rows = await _db.Thuocs
            .Include(x => x.LoaiThuoc)
            .OrderBy(x => x.TenThuoc)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", new[]
        {
            Csv("Mã"),
            Csv("Tên thuốc"),
            Csv("Loại"),
            Csv("Hàm lượng"),
            Csv("Tồn kho"),
            Csv("Hạn sử dụng"),
            Csv("Giá bán"),
            Csv("Trạng thái")
        }));

        foreach (var medicine in rows)
        {
            sb.AppendLine(string.Join(",", new[]
            {
                Csv(medicine.MaThuoc.ToString(CultureInfo.InvariantCulture)),
                Csv(medicine.TenThuoc),
                Csv(medicine.LoaiThuoc?.TenLoaiThuoc),
                Csv(medicine.HamLuong),
                Csv(medicine.SoLuongTon.ToString(CultureInfo.InvariantCulture)),
                Csv(medicine.HanSuDung?.ToString("dd/MM/yyyy")),
                Csv(medicine.GiaBan.ToString("0.##", CultureInfo.InvariantCulture)),
                Csv(medicine.TrangThai ? "Hoạt động" : "Ngừng sử dụng")
            }));
        }

        var bytes = Encoding.UTF8.GetPreamble()
            .Concat(Encoding.UTF8.GetBytes(sb.ToString()))
            .ToArray();

        return File(bytes, "text/csv; charset=utf-8", $"BaoCaoKhoThuoc_{DateTime.Now:yyyyMMdd_HHmm}.csv");
    }

    private static string Csv(string? value)
    {
        value ??= string.Empty;
        var escaped = value.Replace("\"", "\"\"");
        return "\"" + escaped + "\"";
    }
}
