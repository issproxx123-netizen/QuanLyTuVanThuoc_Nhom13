using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyTuVanThuoc_Nhom13.Data;
using QuanLyTuVanThuoc_Nhom13.Models;

namespace QuanLyTuVanThuoc_Nhom13.Controllers;

[Authorize]
public class BenhNhanController : Controller
{
    private readonly ApplicationDbContext _db;
    public BenhNhanController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? search)
    {
        var query = _db.BenhNhans.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(x => x.HoTen.Contains(search) || (x.SoDienThoai != null && x.SoDienThoai.Contains(search)) || (x.DiUng != null && x.DiUng.Contains(search)));
        ViewBag.Search = search; return View(await query.OrderByDescending(x => x.NgayTao).ToListAsync());
    }

    [Authorize(Roles = "Admin,Bác sĩ / Nhân viên tư vấn")]
    public IActionResult Create() => View(new BenhNhan { NgayTao = DateTime.Now });

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Bác sĩ / Nhân viên tư vấn")]
    public async Task<IActionResult> Create(BenhNhan model)
    {
        if (!ModelState.IsValid) return View(model); model.NgayTao = DateTime.Now; _db.Add(model); await _db.SaveChangesAsync(); TempData["Success"] = "Đã thêm hồ sơ bệnh nhân."; return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,Bác sĩ / Nhân viên tư vấn")]
    public async Task<IActionResult> Edit(int id) { var item = await _db.BenhNhans.FindAsync(id); return item == null ? NotFound() : View(item); }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Bác sĩ / Nhân viên tư vấn")]
    public async Task<IActionResult> Edit(int id, BenhNhan model)
    {
        if (id != model.MaBenhNhan) return NotFound(); if (!ModelState.IsValid) return View(model);
        var current = await _db.BenhNhans.AsNoTracking().FirstOrDefaultAsync(x => x.MaBenhNhan == id); if (current == null) return NotFound(); model.NgayTao = current.NgayTao;
        _db.Update(model); await _db.SaveChangesAsync(); TempData["Success"] = "Đã cập nhật hồ sơ bệnh nhân."; return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.BenhNhans.Include(x => x.DonTuVans).ThenInclude(x => x.ChiTietDonTuVans).ThenInclude(x => x.Thuoc).FirstOrDefaultAsync(x => x.MaBenhNhan == id);
        return item == null ? NotFound() : View(item);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id) { var item = await _db.BenhNhans.FindAsync(id); return item == null ? NotFound() : View(item); }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _db.BenhNhans.Include(x => x.DonTuVans).FirstOrDefaultAsync(x => x.MaBenhNhan == id); if (item == null) return NotFound();
        if (item.DonTuVans.Any()) { TempData["Error"] = "Không thể xóa bệnh nhân đã có lịch sử tư vấn. Hệ thống giữ dữ liệu để đảm bảo truy vết."; return RedirectToAction(nameof(Index)); }
        _db.Remove(item); await _db.SaveChangesAsync(); TempData["Success"] = "Đã xóa hồ sơ bệnh nhân."; return RedirectToAction(nameof(Index));
    }
}
