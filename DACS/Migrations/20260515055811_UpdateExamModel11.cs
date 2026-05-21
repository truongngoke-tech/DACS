using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExamModel11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CauHoi_Sai",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaCauHoi = table.Column<int>(type: "int", nullable: false),
                    DapAnDaChon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanSai = table.Column<int>(type: "int", nullable: false),
                    LanCuoiSai = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauHoi_Sai", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CauHoi_Sai_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CauHoi_Sai_Cau_Hoi_MaCauHoi",
                        column: x => x.MaCauHoi,
                        principalTable: "Cau_Hoi",
                        principalColumn: "MaCauHoi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeThi_YeuThich",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaDeThi = table.Column<int>(type: "int", nullable: false),
                    NgayLuu = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeThi_YeuThich", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeThi_YeuThich_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeThi_YeuThich_De_Thi_MaDeThi",
                        column: x => x.MaDeThi,
                        principalTable: "De_Thi",
                        principalColumn: "MaDeThi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Thong_Bao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DuongDan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoaiIcon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DaDoc = table.Column<bool>(type: "bit", nullable: false),
                    ThoiGian = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thong_Bao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Thong_Bao_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CauHoi_Sai_MaCauHoi",
                table: "CauHoi_Sai",
                column: "MaCauHoi");

            migrationBuilder.CreateIndex(
                name: "IX_CauHoi_Sai_UserId_MaCauHoi",
                table: "CauHoi_Sai",
                columns: new[] { "UserId", "MaCauHoi" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeThi_YeuThich_MaDeThi",
                table: "DeThi_YeuThich",
                column: "MaDeThi");

            migrationBuilder.CreateIndex(
                name: "IX_DeThi_YeuThich_UserId_MaDeThi",
                table: "DeThi_YeuThich",
                columns: new[] { "UserId", "MaDeThi" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Thong_Bao_UserId",
                table: "Thong_Bao",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CauHoi_Sai");

            migrationBuilder.DropTable(
                name: "DeThi_YeuThich");

            migrationBuilder.DropTable(
                name: "Thong_Bao");
        }
    }
}
