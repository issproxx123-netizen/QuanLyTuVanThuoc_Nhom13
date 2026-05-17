using Microsoft.EntityFrameworkCore;
using QuanLyTuVanThuoc.Models;
using QuanLyTuVanThuoc.Models;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Tự động tạo dữ liệu mẫu cho Nhóm 13[cite: 1]
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    if (!context.Thuocs.Any())
    {
        context.Thuocs.AddRange(
            new Thuoc { TenThuoc = "Amoxicillin", HamLuong = "500mg", SoLuongTon = 200, LoaiThuoc = "Kháng sinh", HanSuDung = new DateTime(2027, 12, 31), HuongDan = "Uống sau ăn 30 phút" },
            new Thuoc { TenThuoc = "Paracetamol", HamLuong = "500mg", SoLuongTon = 500, LoaiThuoc = "Giảm đau", HanSuDung = new DateTime(2026, 06, 15), HuongDan = "Uống khi sốt cao" }
        );
    }

    if (!context.DonThuocs.Any())
    {
        context.DonThuocs.AddRange(
            new DonThuoc { TenBenhNhan = "Nguyễn Văn A", ChanDoan = "Viêm họng", DiUng = "Không", DaTuVan = true },
            new DonThuoc { TenBenhNhan = "Trần Thị B", ChanDoan = "Sốt siêu vi", DiUng = "Hải sản", DaTuVan = false }
        );
    }
    context.SaveChanges();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();