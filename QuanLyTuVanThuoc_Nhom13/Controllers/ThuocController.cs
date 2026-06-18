using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyTuVanThuoc_Nhom13.Data;
using QuanLyTuVanThuoc_Nhom13.Models;
using QuanLyTuVanThuoc_Nhom13.Services;

namespace QuanLyTuVanThuoc_Nhom13.Controllers;

[Authorize]
public class ThuocController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly WarningService _warningService;

    public ThuocController(ApplicationDbContext db, WarningService warningService)
    {
        _db = db;
        _warningService = warningService;
    }

    public async Task<IActionResult> Index(string? search, int? maLoaiThuoc, bool showInactive = false)
    {
        var query = _db.Thuocs.Include(x => x.LoaiThuoc).AsQueryable();
        if (!showInactive)
        {
            query = query.Where(x => x.TrangThai);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(x =>
                x.TenThuoc.Contains(search)
                || (x.HamLuong != null && x.HamLuong.Contains(search))
                || (x.CongDung != null && x.CongDung.Contains(search)));
        }

        if (maLoaiThuoc.GetValueOrDefault() > 0)
        {
            query = query.Where(x => x.MaLoaiThuoc == maLoaiThuoc);
        }

        ViewBag.Search = search;
        ViewBag.ShowInactive = showInactive;
        ViewBag.SelectedType = maLoaiThuoc;
        ViewBag.LoaiThuocOptions = await _db.LoaiThuocs
            .OrderBy(x => x.TenLoaiThuoc)
            .Select(x => new SelectListItem(x.TenLoaiThuoc, x.MaLoaiThuoc.ToString()))
            .ToListAsync();

        return View(await query.OrderBy(x => x.TenThuoc).ToListAsync());
    }

    [Authorize(Roles = "Admin,Nhân viên kho")]
    public async Task<IActionResult> Create()
    {
        await LoadTypes();
        return View(new Thuoc { TrangThai = true, HanSuDung = DateTime.Today.AddYears(1) });
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Nhân viên kho")]
    public async Task<IActionResult> Create(Thuoc model)
    {
        Normalize(model);
        Validate(model);
        if (!ModelState.IsValid)
        {
            await LoadTypes();
            return View(model);
        }

        _db.Add(model);
        await _db.SaveChangesAsync();
        await _warningService.RefreshMedicineWarningsAsync(model);

        TempData["Success"] = "Đã thêm thuốc vào kho và đồng bộ cảnh báo.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,Nhân viên kho")]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.Thuocs.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        await LoadTypes();
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Nhân viên kho")]
    public async Task<IActionResult> Edit(int id, Thuoc model)
    {
        if (id != model.MaThuoc)
        {
            return NotFound();
        }

        Normalize(model);
        Validate(model);
        if (!ModelState.IsValid)
        {
            await LoadTypes();
            return View(model);
        }

        var item = await _db.Thuocs.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        item.TenThuoc = model.TenThuoc;
        item.MaLoaiThuoc = model.MaLoaiThuoc;
        item.DonViTinh = model.DonViTinh;
        item.HamLuong = model.HamLuong;
        item.CongDung = model.CongDung;
        item.CachDung = model.CachDung;
        item.ChongChiDinh = model.ChongChiDinh;
        item.SoLuongTon = model.SoLuongTon;
        item.HanSuDung = model.HanSuDung?.Date;
        item.GiaBan = model.GiaBan;
        item.TrangThai = model.TrangThai;

        await _db.SaveChangesAsync();
        await _warningService.RefreshMedicineWarningsAsync(item);

        TempData["Success"] = "Đã cập nhật thuốc và đồng bộ cảnh báo.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.Thuocs
            .Include(x => x.LoaiThuoc)
            .FirstOrDefaultAsync(x => x.MaThuoc == id);
        return item == null ? NotFound() : View(item);
    }

    [Authorize(Roles = "Admin,Nhân viên kho")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Thuocs
            .Include(x => x.LoaiThuoc)
            .FirstOrDefaultAsync(x => x.MaThuoc == id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken, Authorize(Roles = "Admin,Nhân viên kho")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _db.Thuocs.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        item.TrangThai = false;
        await _db.SaveChangesAsync();
        await _warningService.RefreshMedicineWarningsAsync(item);

        TempData["Success"] = "Đã ngừng sử dụng thuốc; lịch sử dữ liệu được giữ nguyên.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Nhân viên kho")]
    public async Task<IActionResult> Toggle(int id)
    {
        var item = await _db.Thuocs.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        item.TrangThai = !item.TrangThai;
        await _db.SaveChangesAsync();
        await _warningService.RefreshMedicineWarningsAsync(item);

        TempData["Success"] = item.TrangThai ? "Đã kích hoạt thuốc." : "Đã ngừng sử dụng thuốc.";
        return RedirectToAction(nameof(Index), new { showInactive = true });
    }

    public async Task<IActionResult> CanhBao()
    {
        var soon = DateTime.Today.AddDays(90);
        return View(await _db.Thuocs
            .Include(x => x.LoaiThuoc)
            .Where(x => x.TrangThai
                && (x.SoLuongTon <= 50
                    || (x.HanSuDung != null && x.HanSuDung.Value.Date <= soon)))
            .OrderBy(x => x.HanSuDung)
            .ToListAsync());
    }

    private static void Normalize(Thuoc model)
    {
        model.TenThuoc = model.TenThuoc?.Trim() ?? string.Empty;
        model.DonViTinh = model.DonViTinh?.Trim();
        model.HamLuong = model.HamLuong?.Trim();
        model.CongDung = model.CongDung?.Trim();
        model.CachDung = model.CachDung?.Trim();
        model.ChongChiDinh = model.ChongChiDinh?.Trim();
        model.HanSuDung = model.HanSuDung?.Date;
    }

    private void Validate(Thuoc model)
    {
        if (model.SoLuongTon < 0)
        {
            ModelState.AddModelError(nameof(model.SoLuongTon), "Số lượng tồn không được âm.");
        }

        if (model.GiaBan < 0)
        {
            ModelState.AddModelError(nameof(model.GiaBan), "Giá bán không được âm.");
        }

        if (model.TrangThai && model.HanSuDung.HasValue && model.HanSuDung.Value.Date < DateTime.Today)
        {
            ModelState.AddModelError(nameof(model.HanSuDung),
                "Không thể lưu thuốc mới hoặc cập nhật với hạn sử dụng đã qua.");
        }
    }

    private async Task LoadTypes() =>
        ViewBag.LoaiThuocOptions = await _db.LoaiThuocs
            .OrderBy(x => x.TenLoaiThuoc)
            .Select(x => new SelectListItem(x.TenLoaiThuoc, x.MaLoaiThuoc.ToString()))
            .ToListAsync();
}
