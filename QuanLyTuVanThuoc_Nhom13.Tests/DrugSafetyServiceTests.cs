using QuanLyTuVanThuoc_Nhom13.Models;
using QuanLyTuVanThuoc_Nhom13.Services;

namespace QuanLyTuVanThuoc_Nhom13.Tests;

public class DrugSafetyServiceTests
{
    [Fact]
    public void HasAllergyRisk_PenicillinAndAmoxicillin_ReturnsTrue()
    {
        var patient = new BenhNhan
        {
            HoTen = "Nguyễn Văn A",
            DiUng = "Dị ứng penicillin"
        };

        var medicine = new Thuoc
        {
            TenThuoc = "Amoxicillin",
            HamLuong = "500mg"
        };

        var result = DrugSafetyService.HasAllergyRisk(
            patient,
            medicine,
            out var reason);

        Assert.True(result);
        Assert.Contains("không phù hợp", reason);
    }

    [Fact]
    public void HasAllergyRisk_NoAllergy_ReturnsFalse()
    {
        var patient = new BenhNhan
        {
            HoTen = "Trần Thị B",
            DiUng = "Không có"
        };

        var medicine = new Thuoc
        {
            TenThuoc = "Amoxicillin",
            HamLuong = "500mg"
        };

        var result = DrugSafetyService.HasAllergyRisk(
            patient,
            medicine,
            out var reason);

        Assert.False(result);
        Assert.Equal(string.Empty, reason);
    }

    [Fact]
    public void HasAllergyRisk_IgnoresVietnameseAccents_ReturnsTrue()
    {
        var patient = new BenhNhan
        {
            HoTen = "Lê Văn C",
            DiUng = "DỊ ỨNG PARACETAMOL"
        };

        var medicine = new Thuoc
        {
            TenThuoc = "Paracetamol",
            HamLuong = "500mg"
        };

        var result = DrugSafetyService.HasAllergyRisk(
            patient,
            medicine,
            out _);

        Assert.True(result);
    }
}