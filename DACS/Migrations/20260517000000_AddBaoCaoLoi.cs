using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    public partial class AddBaoCaoLoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bao_Cao_Loi",
                columns: table => new
                {
                    Id         = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    UserId     = table.Column<string>(nullable: false),
                    MaDeThi    = table.Column<int>(nullable: false),
                    MaCauHoi   = table.Column<int>(nullable: true),
                    LoaiBaoCao = table.Column<string>(nullable: false),
                    MoTa       = table.Column<string>(nullable: false),
                    TrangThai  = table.Column<string>(nullable: false, defaultValue: "Chờ xử lý"),
                    GhiChuAdmin = table.Column<string>(nullable: true),
                    NgayTao    = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    NgayXuLy   = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bao_Cao_Loi", x => x.Id);
                    table.ForeignKey("FK_BaoCao_User", x => x.UserId, "AspNetUsers", "Id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_BaoCao_DeThi", x => x.MaDeThi, "De_Thi", "MaDeThi", onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Bao_Cao_Loi");
        }
    }
}
