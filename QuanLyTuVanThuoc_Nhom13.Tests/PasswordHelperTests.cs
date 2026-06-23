using QuanLyTuVanThuoc_Nhom13.Security;

namespace QuanLyTuVanThuoc_Nhom13.Tests;

public class PasswordHelperTests
{
    [Fact]
    public void HashAndVerify_CorrectPassword_ReturnsTrue()
    {
        var password = "123456";
        var hashed = PasswordHelper.Hash(password);

        Assert.True(PasswordHelper.IsHashed(hashed));
        Assert.True(PasswordHelper.Verify(hashed, password));
    }

    [Fact]
    public void Verify_WrongPassword_ReturnsFalse()
    {
        var hashed = PasswordHelper.Hash("123456");

        Assert.False(PasswordHelper.Verify(hashed, "111111"));
    }

    [Fact]
    public void Verify_LegacyPlainPassword_ReturnsTrue()
    {
        Assert.True(PasswordHelper.Verify("123456", "123456"));
    }
}