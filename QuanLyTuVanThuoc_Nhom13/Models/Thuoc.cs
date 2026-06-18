using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTuVanThuoc_Nhom13.Models;

public class Thuoc
{
    [Key]
    public int MaThuoc { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên thuốc")]
    [Display(Name = "Tên thuốc")]
    [StringLength(150)]
    public string TenThuoc { get; set; } = string.Empty;

    [Display(Name = "Loại thuốc")]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn loại thuốc")]
    public int MaLoaiThuoc { get; set; }

    [Display(Name = "Đơn vị tính")]
    [StringLength(50)]
    public string? DonViTinh { get; set; }

    [Display(Name = "Hàm lượng")]
    [StringLength(100)]
    public string? HamLuong { get; set; }

    [Display(Name = "Công dụng")]
    public string? CongDung { get; set; }

    [Display(Name = "Cách dùng")]
    public string? CachDung { get; set; }

    [Display(Name = "Chống chỉ định")]
    public string? ChongChiDinh { get; set; }

    [Display(Name = "Số lượng tồn")]
    [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn không được âm")]
    public int SoLuongTon { get; set; }

    [Display(Name = "Hạn sử dụng")]
    public DateTime? HanSuDung { get; set; }

    [Display(Name = "Giá bán")]
    [Range(0, double.MaxValue, ErrorMessage = "Giá bán không được âm")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaBan { get; set; }

    [Display(Name = "Trạng thái")]
    public bool TrangThai { get; set; } = true;

    [ForeignKey(nameof(MaLoaiThuoc))]
    public LoaiThuoc? LoaiThuoc { get; set; }

    public ICollection<ChiTietDonTuVan> ChiTietDonTuVans { get; set; } = new List<ChiTietDonTuVan>();
    public ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();
    public ICollection<CanhBao> CanhBaos { get; set; } = new List<CanhBao>();
}
