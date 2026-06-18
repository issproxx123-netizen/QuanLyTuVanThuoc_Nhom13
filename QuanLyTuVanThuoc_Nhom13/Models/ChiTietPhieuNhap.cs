using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTuVanThuoc_Nhom13.Models;

public class ChiTietPhieuNhap
{
    [Key]
    public int MaChiTietNhap { get; set; }

    public int MaPhieuNhap { get; set; }
    public int MaThuoc { get; set; }
    public int SoLuongNhap { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? DonGiaNhap { get; set; }

    public DateTime? HanSuDung { get; set; }

    [ForeignKey(nameof(MaPhieuNhap))]
    public PhieuNhapKho? PhieuNhapKho { get; set; }

    [ForeignKey(nameof(MaThuoc))]
    public Thuoc? Thuoc { get; set; }
}
