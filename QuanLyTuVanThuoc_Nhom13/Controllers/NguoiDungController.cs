using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyTuVanThuoc_Nhom13.Data;
using QuanLyTuVanThuoc_Nhom13.Models;
using QuanLyTuVanThuoc_Nhom13.Security;
using QuanLyTuVanThuoc_Nhom13.ViewModels;
using System.Security.Claims;

namespace QuanLyTuVanThuoc_Nhom13.Controllers;

[Authorize(Roles = "Admin")]
public class NguoiDungController : Controller
{
    private readonly ApplicationDbContext _db;
    public NguoiDungController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.NguoiDungs
        .Include(x => x.VaiTro)
        .OrderBy(x => x.HoTen)
        .ToListAsync());

    public async Task<IActionResult> Create()
    {
        var vm = new UserFormViewModel();
        await LoadRoles(vm);
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserFormViewModel vm)
    {
        Normalize(vm);

        if (string.IsNullOrWhiteSpace(vm.MatKhau) || vm.MatKhau.Length < 6)
        {
            ModelState.AddModelError(nameof(vm.MatKhau), "Mật khẩu phải có ít nhất 6 ký tự.");
        }

        if (await _db.NguoiDungs.AnyAsync(x => x.TenDangNhap == vm.TenDangNhap))
        {
            ModelState.AddModelError(nameof(vm.TenDangNhap), "Tên đăng nhập đã tồn tại.");
        }

        if (!await _db.VaiTros.AnyAsync(x => x.MaVaiTro == vm.MaVaiTro))
        {
            ModelState.AddModelError(nameof(vm.MaVaiTro), "Vai trò không hợp lệ.");
        }

        if (!ModelState.IsValid)
        {
            await LoadRoles(vm);
            return View(vm);
        }

        _db.Add(new NguoiDung
        {
            HoTen = vm.HoTen,
            TenDangNhap = vm.TenDangNhap,
            MatKhau = PasswordHelper.Hash(vm.MatKhau!),
            Email = vm.Email,
            SoDienThoai = vm.SoDienThoai,
            MaVaiTro = vm.MaVaiTro,
            TrangThai = vm.TrangThai
        });

        await _db.SaveChangesAsync();
        TempData["Success"] = "Đã tạo tài khoản.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await _db.NguoiDungs.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var vm = new UserFormViewModel
        {
            MaNguoiDung = user.MaNguoiDung,
            HoTen = user.HoTen,
            TenDangNhap = user.TenDangNhap,
            Email = user.Email,
            SoDienThoai = user.SoDienThoai,
            MaVaiTro = user.MaVaiTro,
            TrangThai = user.TrangThai
        };

        ViewBag.IsCurrentUser = id == CurrentUserId();
        await LoadRoles(vm);
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserFormViewModel vm)
    {
        if (id != vm.MaNguoiDung)
        {
            return NotFound();
        }

        var user = await _db.NguoiDungs.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        Normalize(vm);
        var currentId = CurrentUserId();
        var adminRoleId = await GetAdminRoleIdAsync();

        if (await _db.NguoiDungs.AnyAsync(x => x.MaNguoiDung != id && x.TenDangNhap == vm.TenDangNhap))
        {
            ModelState.AddModelError(nameof(vm.TenDangNhap), "Tên đăng nhập đã tồn tại.");
        }

        if (!string.IsNullOrWhiteSpace(vm.MatKhau) && vm.MatKhau.Length < 6)
        {
            ModelState.AddModelError(nameof(vm.MatKhau), "Mật khẩu phải có ít nhất 6 ký tự.");
        }

        if (!await _db.VaiTros.AnyAsync(x => x.MaVaiTro == vm.MaVaiTro))
        {
            ModelState.AddModelError(nameof(vm.MaVaiTro), "Vai trò không hợp lệ.");
        }

        if (id == currentId)
        {
            if (!vm.TrangThai)
            {
                ModelState.AddModelError(nameof(vm.TrangThai),
                    "Không thể khóa tài khoản đang đăng nhập.");
            }

            if (vm.MaVaiTro != adminRoleId)
            {
                ModelState.AddModelError(nameof(vm.MaVaiTro),
                    "Không thể tự gỡ quyền Admin của tài khoản đang đăng nhập.");
            }
        }

        if (user.MaVaiTro == adminRoleId
            && user.TrangThai
            && (vm.MaVaiTro != adminRoleId || !vm.TrangThai))
        {
            var hasOtherActiveAdmin = await _db.NguoiDungs.AnyAsync(x =>
                x.MaNguoiDung != id
                && x.MaVaiTro == adminRoleId
                && x.TrangThai);

            if (!hasOtherActiveAdmin)
            {
                ModelState.AddModelError(string.Empty,
                    "Hệ thống phải luôn còn ít nhất một tài khoản Admin đang hoạt động.");
            }
        }

        if (!ModelState.IsValid)
        {
            ViewBag.IsCurrentUser = id == currentId;
            await LoadRoles(vm);
            return View(vm);
        }

        user.HoTen = vm.HoTen;
        user.TenDangNhap = vm.TenDangNhap;
        user.Email = vm.Email;
        user.SoDienThoai = vm.SoDienThoai;
        user.MaVaiTro = vm.MaVaiTro;
        user.TrangThai = vm.TrangThai;

        if (!string.IsNullOrWhiteSpace(vm.MatKhau))
        {
            user.MatKhau = PasswordHelper.Hash(vm.MatKhau);
        }

        await _db.SaveChangesAsync();
        TempData["Success"] = "Đã cập nhật tài khoản.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int id)
    {
        var currentId = CurrentUserId();
        if (id == currentId)
        {
            TempData["Error"] = "Không thể khóa tài khoản đang đăng nhập.";
            return RedirectToAction(nameof(Index));
        }

        var user = await _db.NguoiDungs.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var adminRoleId = await GetAdminRoleIdAsync();
        if (user.TrangThai && user.MaVaiTro == adminRoleId)
        {
            var hasOtherActiveAdmin = await _db.NguoiDungs.AnyAsync(x =>
                x.MaNguoiDung != id
                && x.MaVaiTro == adminRoleId
                && x.TrangThai);

            if (!hasOtherActiveAdmin)
            {
                TempData["Error"] = "Không thể khóa Admin đang hoạt động cuối cùng của hệ thống.";
                return RedirectToAction(nameof(Index));
            }
        }

        user.TrangThai = !user.TrangThai;
        await _db.SaveChangesAsync();

        TempData["Success"] = user.TrangThai ? "Đã mở khóa tài khoản." : "Đã khóa tài khoản.";
        return RedirectToAction(nameof(Index));
    }

    private int CurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

    private async Task<int> GetAdminRoleIdAsync() =>
        await _db.VaiTros
            .Where(x => x.TenVaiTro == "Admin")
            .Select(x => x.MaVaiTro)
            .SingleAsync();

    private static void Normalize(UserFormViewModel vm)
    {
        vm.HoTen = vm.HoTen?.Trim() ?? string.Empty;
        vm.TenDangNhap = vm.TenDangNhap?.Trim() ?? string.Empty;
        vm.Email = vm.Email?.Trim();
        vm.SoDienThoai = vm.SoDienThoai?.Trim();
    }

    private async Task LoadRoles(UserFormViewModel vm) =>
        vm.VaiTroOptions = await _db.VaiTros
            .OrderBy(x => x.MaVaiTro)
            .Select(x => new SelectListItem(x.TenVaiTro, x.MaVaiTro.ToString()))
            .ToListAsync();
}
