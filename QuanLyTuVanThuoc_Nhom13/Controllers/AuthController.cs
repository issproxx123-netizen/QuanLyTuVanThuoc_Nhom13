using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyTuVanThuoc_Nhom13.Data;
using QuanLyTuVanThuoc_Nhom13.Security;
using QuanLyTuVanThuoc_Nhom13.ViewModels;
using System.Security.Claims;

namespace QuanLyTuVanThuoc_Nhom13.Controllers;

[AllowAnonymous]
public class AuthController : Controller
{
    private readonly ApplicationDbContext _db;
    public AuthController(ApplicationDbContext db) => _db = db;

    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Index", "Home");
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        vm.TenDangNhap = vm.TenDangNhap?.Trim() ?? string.Empty;
        var user = await _db.NguoiDungs.Include(x => x.VaiTro)
            .SingleOrDefaultAsync(x => x.TenDangNhap == vm.TenDangNhap);

        if (user == null || !user.TrangThai || !PasswordHelper.Verify(user.MatKhau, vm.MatKhau))
        {
            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(vm);
        }

        if (!PasswordHelper.IsHashed(user.MatKhau))
        {
            user.MatKhau = PasswordHelper.Hash(vm.MatKhau);
            await _db.SaveChangesAsync();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.MaNguoiDung.ToString()),
            new(ClaimTypes.Name, user.HoTen),
            new(ClaimTypes.Role, user.VaiTro?.TenVaiTro ?? string.Empty),
            new("username", user.TenDangNhap)
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
            new AuthenticationProperties { IsPersistent = vm.GhiNho, ExpiresUtc = DateTimeOffset.Now.AddHours(vm.GhiNho ? 24 : 8) });

        if (!string.IsNullOrWhiteSpace(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl)) return LocalRedirect(vm.ReturnUrl);
        return RedirectToAction("Index", "Home");
    }

    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AccessDenied() => View();
}
