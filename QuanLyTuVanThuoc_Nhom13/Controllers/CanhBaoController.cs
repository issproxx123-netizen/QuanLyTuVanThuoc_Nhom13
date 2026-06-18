using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyTuVanThuoc_Nhom13.Data;

namespace QuanLyTuVanThuoc_Nhom13.Controllers;

[Authorize]
public class CanhBaoController : Controller
{
    private readonly ApplicationDbContext _db;
    public CanhBaoController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? type, string? severity)
    {
        var query = _db.CanhBaos.Include(x => x.BenhNhan).Include(x => x.Thuoc).AsQueryable();
        if (!string.IsNullOrWhiteSpace(type)) query = query.Where(x => x.LoaiCanhBao == type);
        if (!string.IsNullOrWhiteSpace(severity)) query = query.Where(x => x.MucDo == severity);
        ViewBag.Type = type; ViewBag.Severity = severity;
        return View(await query.OrderByDescending(x => x.NgayTao).ToListAsync());
    }
}
