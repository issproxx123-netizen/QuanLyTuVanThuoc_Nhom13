using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyTuVanThuoc.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DonThuocs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenBenhNhan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayKe = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaTuVan = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonThuocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Thuocs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenThuoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HamLuong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoLuongTon = table.Column<int>(type: "int", nullable: false),
                    HuongDan = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thuocs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonThuocs");

            migrationBuilder.DropTable(
                name: "Thuocs");
        }
    }
}
