using System.ComponentModel.DataAnnotations;

namespace QuanLyTuVanThuoc_Nhom13.Models;

public class LoaiThuoc
{
    [Key]
    public int MaLoaiThuoc { get; set; }

    [Required, Display(Name = "Tên loại thuốc")]
    [StringLength(100)]
    public string TenLoaiThuoc { get; set; } = string.Empty;

    [Display(Name = "Mô tả")]
    [StringLength(255)]
    public string? MoTa { get; set; }

    public ICollection<Thuoc> Thuocs { get; set; } = new List<Thuoc>();
}
