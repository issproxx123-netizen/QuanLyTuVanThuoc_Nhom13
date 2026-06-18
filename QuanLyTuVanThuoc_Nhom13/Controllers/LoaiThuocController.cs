using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyTuVanThuoc_Nhom13.Data;
using QuanLyTuVanThuoc_Nhom13.Models;

namespace QuanLyTuVanThuoc_Nhom13.Controllers;

[Authorize]
public class LoaiThuocController : Controller
{
    private readonly ApplicationDbContext _db;
    public LoaiThuocController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.LoaiThuocs.Include(x => x.Thuocs).OrderBy(x => x.TenLoaiThuoc).ToListAsync());

    [Authorize(Roles = "Admin,Nhân viên kho")]
    public IActionResult Create() => View(new LoaiThuoc());

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Nhân viên kho")]
    public async Task<IActionResult> Create(LoaiThuoc model)
    {
        if (await _db.LoaiThuocs.AnyAsync(x => x.TenLoaiThuoc == model.TenLoaiThuoc))
            ModelState.AddModelError(nameof(model.TenLoaiThuoc), "Tên loại thuốc đã tồn tại.");
        if (!ModelState.IsValid) return View(model);
        _db.Add(model); await _db.SaveChangesAsync();
        TempData["Success"] = "Đã thêm loại thuốc.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,Nhân viên kho")]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.LoaiThuocs.FindAsync(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Nhân viên kho")]
    public async Task<IActionResult> Edit(int id, LoaiThuoc model)
    {
        if (id != model.MaLoaiThuoc) return NotFound();
        if (await _db.LoaiThuocs.AnyAsync(x => x.MaLoaiThuoc != id && x.TenLoaiThuoc == model.TenLoaiThuoc))
            ModelState.AddModelError(nameof(model.TenLoaiThuoc), "Tên loại thuốc đã tồn tại.");
        if (!ModelState.IsValid) return View(model);
        _db.Update(model); await _db.SaveChangesAsync();
        TempData["Success"] = "Đã cập nhật loại thuốc.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.LoaiThuocs.Include(x => x.Thuocs).FirstOrDefaultAsync(x => x.MaLoaiThuoc == id);
        if (item == null) return NotFound();
        if (item.Thuocs.Any())
        {
            TempData["Error"] = "Không thể xóa loại thuốc đang được sử dụng.";
            return RedirectToAction(nameof(Index));
        }
        _db.Remove(item); await _db.SaveChangesAsync();
        TempData["Success"] = "Đã xóa loại thuốc.";
        return RedirectToAction(nameof(Index));
    }
}
