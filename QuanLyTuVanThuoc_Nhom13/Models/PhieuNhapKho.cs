using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTuVanThuoc_Nhom13.Models;

public class PhieuNhapKho
{
    [Key]
    public int MaPhieuNhap { get; set; }

    public int MaNguoiDung { get; set; }
    public DateTime NgayNhap { get; set; } = DateTime.Now;

    [StringLength(150)]
    public string? NhaCungCap { get; set; }

    public string? GhiChu { get; set; }

    [ForeignKey(nameof(MaNguoiDung))]
    public NguoiDung? NguoiDung { get; set; }

    public ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();
}
