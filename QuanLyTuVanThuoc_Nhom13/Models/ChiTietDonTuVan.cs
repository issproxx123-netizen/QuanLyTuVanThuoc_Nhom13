using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTuVanThuoc_Nhom13.Models;

public class ChiTietDonTuVan
{
    [Key]
    public int MaChiTiet { get; set; }

    public int MaDonTuVan { get; set; }
    public int MaThuoc { get; set; }

    [Display(Name = "Số lượng")]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int SoLuong { get; set; }

    [Display(Name = "Liều dùng")]
    [StringLength(255)]
    public string? LieuDung { get; set; }

    [Display(Name = "Cách dùng")]
    [StringLength(255)]
    public string? CachDung { get; set; }

    [Display(Name = "Số ngày dùng")]
    public int? SoNgayDung { get; set; }

    [ForeignKey(nameof(MaDonTuVan))]
    public DonTuVan? DonTuVan { get; set; }

    [ForeignKey(nameof(MaThuoc))]
    public Thuoc? Thuoc { get; set; }
}
