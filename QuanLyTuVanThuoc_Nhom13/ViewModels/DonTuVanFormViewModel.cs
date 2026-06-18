using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace QuanLyTuVanThuoc_Nhom13.ViewModels;

public class DonTuVanFormViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn bệnh nhân")]
    [Display(Name = "Bệnh nhân")]
    public int MaBenhNhan { get; set; }

    [Display(Name = "Người tư vấn")]
    public int MaNguoiDung { get; set; } = 2;

    [Required(ErrorMessage = "Vui lòng nhập triệu chứng")]
    [StringLength(1000, ErrorMessage = "Triệu chứng không được vượt quá 1000 ký tự")]
    [Display(Name = "Triệu chứng")]
    public string TrieuChung { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Chẩn đoán không được vượt quá 1000 ký tự")]
    [Display(Name = "Chẩn đoán")]
    public string? ChanDoan { get; set; }

    [StringLength(2000, ErrorMessage = "Ghi chú không được vượt quá 2000 ký tự")]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    public List<ChiTietDonTuVanInput> ChiTietInputs { get; set; } = new()
    {
        new ChiTietDonTuVanInput(),
        new ChiTietDonTuVanInput(),
        new ChiTietDonTuVanInput()
    };

    public List<SelectListItem> BenhNhanOptions { get; set; } = new();
    public List<SelectListItem> NguoiDungOptions { get; set; } = new();
    public List<SelectListItem> ThuocOptions { get; set; } = new();
    public string? ThongTinDiUng { get; set; }
}

public class ChiTietDonTuVanInput
{
    public int MaThuoc { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Số lượng không hợp lệ")]
    public int SoLuong { get; set; }

    [StringLength(255)]
    public string? LieuDung { get; set; }

    [StringLength(255)]
    public string? CachDung { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Số ngày dùng phải lớn hơn 0")]
    public int? SoNgayDung { get; set; }
}
