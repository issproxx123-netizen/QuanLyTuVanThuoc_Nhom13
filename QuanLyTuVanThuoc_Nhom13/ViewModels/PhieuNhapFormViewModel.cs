using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace QuanLyTuVanThuoc_Nhom13.ViewModels;

public class PhieuNhapFormViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập nhà cung cấp")]
    [StringLength(150, ErrorMessage = "Tên nhà cung cấp không được vượt quá 150 ký tự")]
    [Display(Name = "Nhà cung cấp")]
    public string NhaCungCap { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Ghi chú không được vượt quá 2000 ký tự")]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    public List<ChiTietPhieuNhapInput> ChiTietInputs { get; set; } =
        new() { new(), new(), new(), new(), new() };

    public List<SelectListItem> ThuocOptions { get; set; } = new();
}

public class ChiTietPhieuNhapInput
{
    public int MaThuoc { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Số lượng nhập không hợp lệ")]
    public int SoLuongNhap { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Đơn giá nhập không được âm")]
    public decimal? DonGiaNhap { get; set; }

    public DateTime? HanSuDung { get; set; }
}
