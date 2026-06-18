using System.ComponentModel.DataAnnotations;

namespace QuanLyTuVanThuoc_Nhom13.Models;

public class VaiTro
{
    [Key]
    public int MaVaiTro { get; set; }

    [Required, StringLength(100)]
    public string TenVaiTro { get; set; } = string.Empty;

    public ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
