using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyTuVanThuoc_Nhom13.Data;
using QuanLyTuVanThuoc_Nhom13.Models;
using QuanLyTuVanThuoc_Nhom13.Services;
using QuanLyTuVanThuoc_Nhom13.ViewModels;
using System.Security.Claims;

namespace QuanLyTuVanThuoc_Nhom13.Controllers;

[Authorize(Roles = "Admin,Nhân viên kho")]
public class PhieuNhapKhoController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly WarningService _warningService;

    public PhieuNhapKhoController(ApplicationDbContext db, WarningService warningService)
    {
        _db = db;
        _warningService = warningService;
    }

    public async Task<IActionResult> Index() => View(await _db.PhieuNhapKhos
        .Include(x => x.NguoiDung)
        .Include(x => x.ChiTietPhieuNhaps)
        .OrderByDescending(x => x.NgayNhap)
        .ToListAsync());

    public async Task<IActionResult> Create()
    {
        var vm = new PhieuNhapFormViewModel();
        await LoadOptions(vm);
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PhieuNhapFormViewModel vm)
    {
        vm.ChiTietInputs ??= new List<ChiTietPhieuNhapInput>();
        var selectedInputs = ValidateAndGetMedicineRows(vm);

        if (!ModelState.IsValid)
        {
            EnsureRows(vm);
            await LoadOptions(vm);
            return View(vm);
        }

        var ids = selectedInputs.Select(x => x.MaThuoc).Distinct().ToList();
        var medicines = await _db.Thuocs
            .Where(x => ids.Contains(x.MaThuoc))
            .ToDictionaryAsync(x => x.MaThuoc);

        if (medicines.Count != ids.Count)
        {
            ModelState.AddModelError(string.Empty, "Có thuốc không tồn tại trong hệ thống.");
        }

        foreach (var input in selectedInputs)
        {
            if (!medicines.TryGetValue(input.MaThuoc, out var medicine))
            {
                continue;
            }

            if (!medicine.TrangThai)
            {
                ModelState.AddModelError(string.Empty,
                    $"{medicine.TenThuoc} đã ngừng sử dụng, không thể nhập thêm vào phiếu này.");
            }
        }

        if (!ModelState.IsValid)
        {
            EnsureRows(vm);
            await LoadOptions(vm);
            return View(vm);
        }

        await using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var receipt = new PhieuNhapKho
            {
                MaNguoiDung = userId,
                NgayNhap = DateTime.Now,
                NhaCungCap = vm.NhaCungCap?.Trim() ?? string.Empty,
                GhiChu = vm.GhiChu?.Trim()
            };

            _db.PhieuNhapKhos.Add(receipt);
            await _db.SaveChangesAsync();

            foreach (var input in selectedInputs)
            {
                var medicine = medicines[input.MaThuoc];
                _db.ChiTietPhieuNhaps.Add(new ChiTietPhieuNhap
                {
                    MaPhieuNhap = receipt.MaPhieuNhap,
                    MaThuoc = input.MaThuoc,
                    SoLuongNhap = input.SoLuongNhap,
                    DonGiaNhap = input.DonGiaNhap,
                    HanSuDung = input.HanSuDung
                });

                medicine.SoLuongTon += input.SoLuongNhap;
                if (input.HanSuDung.HasValue)
                {
                    medicine.HanSuDung = input.HanSuDung.Value.Date;
                }

                await _warningService.RefreshMedicineWarningsAsync(medicine, saveChanges: false);
            }

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            TempData["Success"] = "Đã nhập kho, cập nhật số lượng tồn và đồng bộ cảnh báo.";
            return RedirectToAction(nameof(Details), new { id = receipt.MaPhieuNhap });
        }
        catch
        {
            await transaction.RollbackAsync();
            ModelState.AddModelError(string.Empty,
                "Không thể lưu phiếu nhập. Dữ liệu đã được hoàn tác an toàn.");
            EnsureRows(vm);
            await LoadOptions(vm);
            return View(vm);
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.PhieuNhapKhos
            .Include(x => x.NguoiDung)
            .Include(x => x.ChiTietPhieuNhaps)
            .ThenInclude(x => x.Thuoc)
            .FirstOrDefaultAsync(x => x.MaPhieuNhap == id);

        return item == null ? NotFound() : View(item);
    }

    private List<ChiTietPhieuNhapInput> ValidateAndGetMedicineRows(PhieuNhapFormViewModel vm)
    {
        var selected = new List<ChiTietPhieuNhapInput>();

        for (var i = 0; i < vm.ChiTietInputs.Count; i++)
        {
            var row = vm.ChiTietInputs[i];
            var hasAnyData = row.MaThuoc > 0
                || row.SoLuongNhap != 0
                || row.DonGiaNhap.HasValue
                || row.HanSuDung.HasValue;

            if (!hasAnyData)
            {
                continue;
            }

            if (row.MaThuoc <= 0)
            {
                ModelState.AddModelError($"ChiTietInputs[{i}].MaThuoc", "Vui lòng chọn thuốc.");
            }

            if (row.SoLuongNhap <= 0)
            {
                ModelState.AddModelError($"ChiTietInputs[{i}].SoLuongNhap", "Số lượng nhập phải lớn hơn 0.");
            }

            if (row.DonGiaNhap.HasValue && row.DonGiaNhap.Value < 0)
            {
                ModelState.AddModelError($"ChiTietInputs[{i}].DonGiaNhap", "Đơn giá nhập không được âm.");
            }

            if (row.HanSuDung.HasValue && row.HanSuDung.Value.Date <= DateTime.Today)
            {
                ModelState.AddModelError($"ChiTietInputs[{i}].HanSuDung", "Hạn sử dụng phải sau ngày hiện tại.");
            }

            if (row.MaThuoc > 0 && row.SoLuongNhap > 0)
            {
                selected.Add(row);
            }
        }

        if (selected.Count == 0)
        {
            ModelState.AddModelError(string.Empty,
                "Vui lòng chọn ít nhất một thuốc và nhập số lượng lớn hơn 0.");
        }

        if (selected.GroupBy(x => x.MaThuoc).Any(g => g.Count() > 1))
        {
            ModelState.AddModelError(string.Empty, "Không chọn trùng thuốc trong cùng phiếu nhập.");
        }

        return selected;
    }

    private static void EnsureRows(PhieuNhapFormViewModel vm)
    {
        vm.ChiTietInputs ??= new List<ChiTietPhieuNhapInput>();
        while (vm.ChiTietInputs.Count < 5)
        {
            vm.ChiTietInputs.Add(new ChiTietPhieuNhapInput());
        }
    }

    private async Task LoadOptions(PhieuNhapFormViewModel vm) =>
        vm.ThuocOptions = await _db.Thuocs
            .Where(x => x.TrangThai)
            .OrderBy(x => x.TenThuoc)
            .Select(x => new SelectListItem(
                $"{x.TenThuoc} {x.HamLuong} - tồn {x.SoLuongTon}",
                x.MaThuoc.ToString()))
            .ToListAsync();
}
