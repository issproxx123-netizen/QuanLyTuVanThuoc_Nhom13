using QuanLyTuVanThuoc_Nhom13.Models;

namespace QuanLyTuVanThuoc_Nhom13.ViewModels;

public class ChartPointViewModel
{
    public string Label { get; set; } = string.Empty;
    public int Value { get; set; }
    public int HeightPercent { get; set; }
}

public class BaoCaoViewModel
{
    public int TongBenhNhan { get; set; }
    public int TongThuoc { get; set; }
    public int TongDon { get; set; }
    public int TongCanhBao { get; set; }
    public int ThuocTonKhoThap { get; set; }
    public int ThuocSapHetHan { get; set; }
    public int TongPhieuNhap { get; set; }
    public decimal GiaTriTonKho { get; set; }
    public List<ChartPointViewModel> LuotTuVanTheoNgay { get; set; } = new();
    public List<Thuoc> ThuocCanChuY { get; set; } = new();
    public List<DonTuVan> DonGanDay { get; set; } = new();
    public List<PhieuNhapKho> PhieuNhapGanDay { get; set; } = new();
}
