using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTuVanThuoc_Nhom13.Models;

public class NguoiDung
{
    [Key]
    public int MaNguoiDung { get; set; }

    [Required, StringLength(100)]
    public string HoTen { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string TenDangNhap { get; set; } = string.Empty;

    [Required, StringLength(255)]
    public string MatKhau { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    public int MaVaiTro { get; set; }
    public bool TrangThai { get; set; } = true;

    [ForeignKey(nameof(MaVaiTro))]
    public VaiTro? VaiTro { get; set; }

    public ICollection<DonTuVan> DonTuVans { get; set; } = new List<DonTuVan>();
    public ICollection<PhieuNhapKho> PhieuNhapKhos { get; set; } = new List<PhieuNhapKho>();
}
