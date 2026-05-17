using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyTuVanThuoc.Models
{
    public class DonThuoc
    {
        [Key]
        public int Id { get; set; }
        public string? TenBenhNhan { get; set; } // Thêm dấu ? để hết lỗi Build
        public string? ChanDoan { get; set; }
        public string? DiUng { get; set; }
        public DateTime NgayKe { get; set; } = DateTime.Now;
        public bool DaTuVan { get; set; }
    }
}