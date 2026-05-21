using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    public partial class AddNewFeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeThi_YeuThich",
                columns: table => new {
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    MaDeThi = table.Column<int>(nullable: false),
                    NgayLuu = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table => {
                    table.PrimaryKey("PK_DeThi_YeuThich", x => x.Id);
                    table.ForeignKey("FK_DeThi_YeuThich_Users", x => x.UserId, "AspNetUsers", "Id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_DeThi_YeuThich_DeThi", x => x.MaDeThi, "De_Thi", "MaDeThi", onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex("IX_DeThi_YeuThich_UserId_MaDeThi", "DeThi_YeuThich", new[]{"UserId","MaDeThi"}, unique: true);

            migrationBuilder.CreateTable(
                name: "CauHoi_Sai",
                columns: table => new {
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    MaCauHoi = table.Column<int>(nullable: false),
                    DapAnDaChon = table.Column<string>(nullable: true),
                    LanSai = table.Column<int>(nullable: false, defaultValue: 1),
                    LanCuoiSai = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table => {
                    table.PrimaryKey("PK_CauHoi_Sai", x => x.Id);
                    table.ForeignKey("FK_CauHoi_Sai_Users", x => x.UserId, "AspNetUsers", "Id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_CauHoi_Sai_CauHoi", x => x.MaCauHoi, "Cau_Hoi", "MaCauHoi", onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex("IX_CauHoi_Sai_UserId_MaCauHoi", "CauHoi_Sai", new[]{"UserId","MaCauHoi"}, unique: true);

            migrationBuilder.CreateTable(
                name: "Thong_Bao",
                columns: table => new {
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    NoiDung = table.Column<string>(nullable: false),
                    DuongDan = table.Column<string>(nullable: true),
                    LoaiIcon = table.Column<string>(nullable: false, defaultValue: "info"),
                    DaDoc = table.Column<bool>(nullable: false, defaultValue: false),
                    ThoiGian = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table => {
                    table.PrimaryKey("PK_Thong_Bao", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("DeThi_YeuThich");
            migrationBuilder.DropTable("CauHoi_Sai");
            migrationBuilder.DropTable("Thong_Bao");
        }
    }
}
