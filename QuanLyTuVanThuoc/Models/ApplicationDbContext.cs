using Microsoft.EntityFrameworkCore;

namespace QuanLyTuVanThuoc.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Thuoc> Thuocs { get; set; }
        public DbSet<DonThuoc> DonThuocs { get; set; }
    }
}