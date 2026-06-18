using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTuVanThuoc_Nhom13.Models;

public class DonTuVan
{
    [Key]
    public int MaDonTuVan { get; set; }

    [Display(Name = "Bệnh nhân")]
    public int MaBenhNhan { get; set; }

    [Display(Name = "Người tư vấn")]
    public int MaNguoiDung { get; set; }

    [Display(Name = "Ngày tư vấn")]
    public DateTime NgayTuVan { get; set; } = DateTime.Now;

    [Display(Name = "Triệu chứng")]
    public string? TrieuChung { get; set; }

    [Display(Name = "Chẩn đoán")]
    public string? ChanDoan { get; set; }

    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    [ForeignKey(nameof(MaBenhNhan))]
    public BenhNhan? BenhNhan { get; set; }

    [ForeignKey(nameof(MaNguoiDung))]
    public NguoiDung? NguoiDung { get; set; }

    public ICollection<ChiTietDonTuVan> ChiTietDonTuVans { get; set; } = new List<ChiTietDonTuVan>();
}
