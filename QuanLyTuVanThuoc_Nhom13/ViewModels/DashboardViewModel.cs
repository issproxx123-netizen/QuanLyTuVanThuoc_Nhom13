using QuanLyTuVanThuoc_Nhom13.Models;

namespace QuanLyTuVanThuoc_Nhom13.ViewModels;

public class DashboardViewModel
{
    public int TongBenhNhan { get; set; }
    public int TongThuoc { get; set; }
    public int TongDonTuVan { get; set; }
    public int TongCanhBao { get; set; }
    public int ThuocTonKhoThap { get; set; }
    public int ThuocSapHetHan { get; set; }
    public int TongPhieuNhap { get; set; }
    public decimal GiaTriTonKho { get; set; }
    public List<DonTuVan> DonGanDay { get; set; } = new();
    public List<CanhBao> CanhBaoMoi { get; set; } = new();
    public List<Thuoc> ThuocCanChuY { get; set; } = new();
    public List<ChartPointViewModel> LuotTuVanTheoNgay { get; set; } = new();
}
