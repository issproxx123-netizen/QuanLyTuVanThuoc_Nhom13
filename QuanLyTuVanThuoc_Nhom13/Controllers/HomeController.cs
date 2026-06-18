using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyTuVanThuoc_Nhom13.Data;
using QuanLyTuVanThuoc_Nhom13.Models;
using QuanLyTuVanThuoc_Nhom13.ViewModels;
using System.Diagnostics;

namespace QuanLyTuVanThuoc_Nhom13.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    public HomeController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        try
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

            var vm = new DashboardViewModel
            {
                TongBenhNhan = await _db.BenhNhans.CountAsync(),
                TongThuoc = await _db.Thuocs.CountAsync(x => x.TrangThai),
                TongDonTuVan = await _db.DonTuVans.CountAsync(),
                TongCanhBao = await _db.CanhBaos.CountAsync(),
                ThuocTonKhoThap = await _db.Thuocs.CountAsync(x => x.TrangThai && x.SoLuongTon <= 50),
                ThuocSapHetHan = await _db.Thuocs.CountAsync(x =>
                    x.TrangThai && x.HanSuDung != null && x.HanSuDung.Value.Date <= soon),
                TongPhieuNhap = await _db.PhieuNhapKhos.CountAsync(),
                GiaTriTonKho = inventoryValue,
                LuotTuVanTheoNgay = points,
                DonGanDay = await _db.DonTuVans
                    .Include(x => x.BenhNhan)
                    .Include(x => x.NguoiDung)
                    .OrderByDescending(x => x.NgayTuVan)
                    .Take(5)
                    .ToListAsync(),
                CanhBaoMoi = await _db.CanhBaos
                    .Include(x => x.BenhNhan)
                    .Include(x => x.Thuoc)
                    .OrderByDescending(x => x.NgayTao)
                    .Take(5)
                    .ToListAsync(),
                ThuocCanChuY = await _db.Thuocs
                    .Include(x => x.LoaiThuoc)
                    .Where(x => x.TrangThai
                        && (x.SoLuongTon <= 50
                            || (x.HanSuDung != null && x.HanSuDung.Value.Date <= soon)))
                    .OrderBy(x => x.SoLuongTon)
                    .Take(5)
                    .ToListAsync()
            };

            return View(vm);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = ex.Message;
            return View("DatabaseError");
        }
    }

    [AllowAnonymous]
    public IActionResult Privacy() => View();

    [AllowAnonymous, ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
    });
}
