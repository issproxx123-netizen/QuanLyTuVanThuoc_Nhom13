using System.ComponentModel.DataAnnotations;

namespace QuanLyTuVanThuoc_Nhom13.Models;

public class BenhNhan
{
    [Key]
    public int MaBenhNhan { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập họ tên bệnh nhân")]
    [Display(Name = "Họ tên")]
    [StringLength(100)]
    public string HoTen { get; set; } = string.Empty;

    [Display(Name = "Giới tính")]
    [StringLength(10)]
    public string? GioiTinh { get; set; }

    [Display(Name = "Ngày sinh")]
    public DateTime? NgaySinh { get; set; }

    [Display(Name = "Số điện thoại")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    [Display(Name = "Địa chỉ")]
    [StringLength(255)]
    public string? DiaChi { get; set; }

    [Display(Name = "Tiền sử bệnh")]
    public string? TienSuBenh { get; set; }

    [Display(Name = "Dị ứng")]
    public string? DiUng { get; set; }

    [Display(Name = "Ngày tạo")]
    public DateTime NgayTao { get; set; } = DateTime.Now;

    public ICollection<DonTuVan> DonTuVans { get; set; } = new List<DonTuVan>();
    public ICollection<CanhBao> CanhBaos { get; set; } = new List<CanhBao>();
}
