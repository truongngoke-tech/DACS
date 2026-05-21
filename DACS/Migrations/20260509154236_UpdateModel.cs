using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Ket_Qua_Thi",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ket_Qua_Thi_UserId1",
                table: "Ket_Qua_Thi",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Ket_Qua_Thi_AspNetUsers_UserId1",
                table: "Ket_Qua_Thi",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ket_Qua_Thi_AspNetUsers_UserId1",
                table: "Ket_Qua_Thi");

            migrationBuilder.DropIndex(
                name: "IX_Ket_Qua_Thi_UserId1",
                table: "Ket_Qua_Thi");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Ket_Qua_Thi");
        }
    }
}
