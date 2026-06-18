using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTuVanThuoc_Nhom13.Models;

public class CanhBao
{
    [Key]
    public int MaCanhBao { get; set; }

    public int? MaBenhNhan { get; set; }
    public int? MaThuoc { get; set; }

    [Required, Display(Name = "Loại cảnh báo")]
    [StringLength(100)]
    public string LoaiCanhBao { get; set; } = string.Empty;

    [Required, Display(Name = "Nội dung")]
    public string NoiDung { get; set; } = string.Empty;

    [Display(Name = "Mức độ")]
    [StringLength(50)]
    public string? MucDo { get; set; }

    [Display(Name = "Ngày tạo")]
    public DateTime NgayTao { get; set; } = DateTime.Now;

    [ForeignKey(nameof(MaBenhNhan))]
    public BenhNhan? BenhNhan { get; set; }

    [ForeignKey(nameof(MaThuoc))]
    public Thuoc? Thuoc { get; set; }
}
