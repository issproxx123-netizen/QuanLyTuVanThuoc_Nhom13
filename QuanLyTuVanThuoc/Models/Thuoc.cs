using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyTuVanThuoc.Models
{
    public class Thuoc
    {
        [Key]
        public int Id { get; set; }
        public string? TenThuoc { get; set; } // Thêm dấu ? để hết lỗi Build
        public string? HamLuong { get; set; }
        public int SoLuongTon { get; set; }
        public string? LoaiThuoc { get; set; }
        public DateTime HanSuDung { get; set; }
        public string? HuongDan { get; set; }
    }
}