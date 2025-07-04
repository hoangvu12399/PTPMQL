using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoMVC.Migrations
{
    /// <inheritdoc />
    public partial class HeThongPhanPhoi_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeThongPhanPhoi",
                columns: table => new
                {
                    MaHTPP = table.Column<string>(type: "TEXT", nullable: false),
                    TenHTPP = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    MaDaily = table.Column<string>(type: "TEXT", nullable: true),
                    TenDaily = table.Column<string>(type: "TEXT", nullable: true),
                    DiaChi = table.Column<string>(type: "TEXT", nullable: true),
                    NguoiDaiDien = table.Column<string>(type: "TEXT", nullable: true),
                    DienThoai = table.Column<string>(type: "TEXT", nullable: true),
                    HeThongPhanPhoiMaHTPP = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeThongPhanPhoi", x => x.MaHTPP);
                    table.ForeignKey(
                        name: "FK_HeThongPhanPhoi_HeThongPhanPhoi_HeThongPhanPhoiMaHTPP",
                        column: x => x.HeThongPhanPhoiMaHTPP,
                        principalTable: "HeThongPhanPhoi",
                        principalColumn: "MaHTPP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeThongPhanPhoi_HeThongPhanPhoiMaHTPP",
                table: "HeThongPhanPhoi",
                column: "HeThongPhanPhoiMaHTPP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeThongPhanPhoi");
        }
    }
}
