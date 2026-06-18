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

[Authorize]
public class DonTuVanController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly WarningService _warningService;

    public DonTuVanController(ApplicationDbContext db, WarningService warningService)
    {
        _db = db;
        _warningService = warningService;
    }

    public async Task<IActionResult> Index(string? search)
    {
        var query = _db.DonTuVans
            .Include(x => x.BenhNhan)
            .Include(x => x.NguoiDung)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(x =>
                x.BenhNhan!.HoTen.Contains(search)
                || (x.TrieuChung != null && x.TrieuChung.Contains(search))
                || (x.ChanDoan != null && x.ChanDoan.Contains(search)));
        }

        ViewBag.Search = search;
        return View(await query.OrderByDescending(x => x.NgayTuVan).ToListAsync());
    }

    [Authorize(Roles = "Admin,Bác sĩ / Nhân viên tư vấn")]
    public async Task<IActionResult> Create()
    {
        var vm = new DonTuVanFormViewModel { MaNguoiDung = CurrentUserId() };
        await LoadOptions(vm);
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Bác sĩ / Nhân viên tư vấn")]
    public async Task<IActionResult> Create(DonTuVanFormViewModel vm)
    {
        vm.MaNguoiDung = CurrentUserId();
        vm.ChiTietInputs ??= new List<ChiTietDonTuVanInput>();

        var selectedInputs = ValidateAndGetMedicineRows(vm);
        if (!ModelState.IsValid)
        {
            EnsureRows(vm);
            await LoadOptions(vm);
            return View(vm);
        }

        var patient = await _db.BenhNhans.FindAsync(vm.MaBenhNhan);
        if (patient == null)
        {
            ModelState.AddModelError(nameof(vm.MaBenhNhan), "Bệnh nhân không tồn tại.");
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

        var allergyWarnings = new List<(Thuoc Medicine, string Reason)>();
        foreach (var input in selectedInputs)
        {
            if (!medicines.TryGetValue(input.MaThuoc, out var medicine))
            {
                continue;
            }

            if (!medicine.TrangThai)
            {
                ModelState.AddModelError(string.Empty, $"{medicine.TenThuoc} đã ngừng sử dụng.");
            }

            if (medicine.HanSuDung.HasValue && medicine.HanSuDung.Value.Date < DateTime.Today)
            {
                ModelState.AddModelError(string.Empty, $"{medicine.TenThuoc} đã hết hạn.");
            }

            if (input.SoLuong > medicine.SoLuongTon)
            {
                ModelState.AddModelError(string.Empty,
                    $"{medicine.TenThuoc} chỉ còn {medicine.SoLuongTon} {medicine.DonViTinh}, không đủ số lượng {input.SoLuong}.");
            }

            if (DrugSafetyService.HasAllergyRisk(patient, medicine, out var reason))
            {
                ModelState.AddModelError(string.Empty, $"CẢNH BÁO DỊ ỨNG: {reason} Vui lòng chọn thuốc thay thế.");
                allergyWarnings.Add((medicine, reason));
            }
        }

        foreach (var warning in allergyWarnings)
        {
            await _warningService.LogAllergyWarningAsync(patient, warning.Medicine, warning.Reason);
        }

        if (!ModelState.IsValid)
        {
            vm.ThongTinDiUng = patient.DiUng;
            EnsureRows(vm);
            await LoadOptions(vm);
            return View(vm);
        }

        await using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            var consultation = new DonTuVan
            {
                MaBenhNhan = vm.MaBenhNhan,
                MaNguoiDung = vm.MaNguoiDung,
                NgayTuVan = DateTime.Now,
                TrieuChung = vm.TrieuChung?.Trim() ?? string.Empty,
                ChanDoan = vm.ChanDoan?.Trim(),
                GhiChu = vm.GhiChu?.Trim()
            };

            _db.DonTuVans.Add(consultation);
            await _db.SaveChangesAsync();

            foreach (var input in selectedInputs)
            {
                var medicine = medicines[input.MaThuoc];
                _db.ChiTietDonTuVans.Add(new ChiTietDonTuVan
                {
                    MaDonTuVan = consultation.MaDonTuVan,
                    MaThuoc = medicine.MaThuoc,
                    SoLuong = input.SoLuong,
                    LieuDung = input.LieuDung?.Trim(),
                    CachDung = input.CachDung?.Trim(),
                    SoNgayDung = input.SoNgayDung
                });

                medicine.SoLuongTon -= input.SoLuong;
                await _warningService.RefreshMedicineWarningsAsync(medicine, saveChanges: false);
            }

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            TempData["Success"] = "Đã lập phiếu tư vấn, kiểm tra an toàn và cập nhật tồn kho.";
            return RedirectToAction(nameof(Details), new { id = consultation.MaDonTuVan });
        }
        catch
        {
            await transaction.RollbackAsync();
            ModelState.AddModelError(string.Empty,
                "Không thể lưu phiếu. Toàn bộ thay đổi đã được hoàn tác an toàn.");
            EnsureRows(vm);
            await LoadOptions(vm);
            return View(vm);
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.DonTuVans
            .Include(x => x.BenhNhan)
            .Include(x => x.NguoiDung)
            .Include(x => x.ChiTietDonTuVans)
            .ThenInclude(x => x.Thuoc)
            .FirstOrDefaultAsync(x => x.MaDonTuVan == id);

        return item == null ? NotFound() : View(item);
    }

    private List<ChiTietDonTuVanInput> ValidateAndGetMedicineRows(DonTuVanFormViewModel vm)
    {
        var selected = new List<ChiTietDonTuVanInput>();

        for (var i = 0; i < vm.ChiTietInputs.Count; i++)
        {
            var row = vm.ChiTietInputs[i];
            var hasAnyData = row.MaThuoc > 0
                || row.SoLuong != 0
                || row.SoNgayDung.HasValue
                || !string.IsNullOrWhiteSpace(row.LieuDung)
                || !string.IsNullOrWhiteSpace(row.CachDung);

            if (!hasAnyData)
            {
                continue;
            }

            if (row.MaThuoc <= 0)
            {
                ModelState.AddModelError($"ChiTietInputs[{i}].MaThuoc", "Vui lòng chọn thuốc.");
            }

            if (row.SoLuong <= 0)
            {
                ModelState.AddModelError($"ChiTietInputs[{i}].SoLuong", "Số lượng phải lớn hơn 0.");
            }

            if (row.SoNgayDung.HasValue && row.SoNgayDung.Value <= 0)
            {
                ModelState.AddModelError($"ChiTietInputs[{i}].SoNgayDung", "Số ngày dùng phải lớn hơn 0.");
            }

            if (row.MaThuoc > 0 && row.SoLuong > 0)
            {
                selected.Add(row);
            }
        }

        if (selected.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "Vui lòng chọn ít nhất một thuốc và nhập số lượng lớn hơn 0.");
        }

        if (selected.GroupBy(x => x.MaThuoc).Any(g => g.Count() > 1))
        {
            ModelState.AddModelError(string.Empty, "Không chọn trùng thuốc trong cùng phiếu tư vấn.");
        }

        return selected;
    }

    private static void EnsureRows(DonTuVanFormViewModel vm)
    {
        vm.ChiTietInputs ??= new List<ChiTietDonTuVanInput>();
        while (vm.ChiTietInputs.Count < 3)
        {
            vm.ChiTietInputs.Add(new ChiTietDonTuVanInput());
        }
    }

    private int CurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

    private async Task LoadOptions(DonTuVanFormViewModel vm)
    {
        vm.BenhNhanOptions = await _db.BenhNhans
            .OrderBy(x => x.HoTen)
            .Select(x => new SelectListItem(
                $"{x.HoTen} - {x.SoDienThoai} - Dị ứng: {x.DiUng}",
                x.MaBenhNhan.ToString()))
            .ToListAsync();

        vm.NguoiDungOptions = await _db.NguoiDungs
            .Where(x => x.TrangThai)
            .OrderBy(x => x.HoTen)
            .Select(x => new SelectListItem(x.HoTen, x.MaNguoiDung.ToString()))
            .ToListAsync();

        vm.ThuocOptions = await _db.Thuocs
            .Where(x => x.TrangThai
                && x.SoLuongTon > 0
                && (!x.HanSuDung.HasValue || x.HanSuDung.Value.Date >= DateTime.Today))
            .OrderBy(x => x.TenThuoc)
            .Select(x => new SelectListItem(
                $"{x.TenThuoc} {x.HamLuong} - tồn {x.SoLuongTon}",
                x.MaThuoc.ToString()))
            .ToListAsync();
    }
}
