using QuanLyTuVanThuoc_Nhom13.Models;
using System.Globalization;
using System.Text;

namespace QuanLyTuVanThuoc_Nhom13.Services;

public static class DrugSafetyService
{
    private static readonly Dictionary<string, string[]> AllergyGroups = new(StringComparer.OrdinalIgnoreCase)
    {
        ["penicillin"] = ["penicillin", "amoxicillin", "ampicillin", "augmentin"],
        ["amoxicillin"] = ["amoxicillin", "penicillin"],
        ["paracetamol"] = ["paracetamol", "acetaminophen"],
        ["aspirin"] = ["aspirin", "acetylsalicylic"],
        ["ibuprofen"] = ["ibuprofen"],
        ["sulfa"] = ["sulfa", "sulfonamide"],
        ["cephalosporin"] = ["cephalosporin", "cephalexin", "cefuroxime"],
        ["omeprazole"] = ["omeprazole"],
        ["loratadine"] = ["loratadine"]
    };

    public static bool HasAllergyRisk(BenhNhan patient, Thuoc medicine, out string reason)
    {
        reason = string.Empty;
        var allergy = Normalize(patient.DiUng);
        if (string.IsNullOrWhiteSpace(allergy) || allergy.Contains("khong co")) return false;

        var medicineText = Normalize($"{medicine.TenThuoc} {medicine.HamLuong} {medicine.ChongChiDinh}");
        foreach (var group in AllergyGroups)
        {
            if (!allergy.Contains(group.Key)) continue;
            if (group.Value.Any(medicineText.Contains))
            {
                reason = $"Bệnh nhân có tiền sử '{patient.DiUng}', không phù hợp với {medicine.TenThuoc}.";
                return true;
            }
        }

        var normalizedName = Normalize(medicine.TenThuoc);
        if (normalizedName.Length >= 4 && allergy.Contains(normalizedName))
        {
            reason = $"Bệnh nhân khai báo dị ứng trực tiếp với {medicine.TenThuoc}.";
            return true;
        }

        return false;
    }

    private static string Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;
        var decomposed = value.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c == 'đ' ? 'd' : c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
