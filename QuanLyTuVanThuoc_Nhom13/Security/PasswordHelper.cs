using Microsoft.AspNetCore.Identity;

namespace QuanLyTuVanThuoc_Nhom13.Security;

public static class PasswordHelper
{
    private static readonly PasswordHasher<object> Hasher = new();

    public static string Hash(string password) => Hasher.HashPassword(new object(), password);

    public static bool Verify(string storedValue, string password)
    {
        if (string.IsNullOrWhiteSpace(storedValue)) return false;
        if (!storedValue.StartsWith("AQAAAA", StringComparison.Ordinal))
            return string.Equals(storedValue, password, StringComparison.Ordinal);

        return Hasher.VerifyHashedPassword(new object(), storedValue, password)
            != PasswordVerificationResult.Failed;
    }

    public static bool IsHashed(string value) =>
        !string.IsNullOrWhiteSpace(value) && value.StartsWith("AQAAAA", StringComparison.Ordinal);
}
