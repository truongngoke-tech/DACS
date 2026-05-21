using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ket_Qua_Thi_Nguoi_Dung_NguoiDungMaNguoiDung",
                table: "Ket_Qua_Thi");

            migrationBuilder.DropForeignKey(
                name: "FK_Nhat_Ky_Hoc_Tap_Nguoi_Dung_NguoiDungMaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap");

            migrationBuilder.DropForeignKey(
                name: "FK_Phan_Tich_AI_Nguoi_Dung_NguoiDungMaNguoiDung",
                table: "Phan_Tich_AI");

            migrationBuilder.DropTable(
                name: "Nguoi_Dung");

            migrationBuilder.DropIndex(
                name: "IX_Phan_Tich_AI_NguoiDungMaNguoiDung",
                table: "Phan_Tich_AI");

            migrationBuilder.DropIndex(
                name: "IX_Nhat_Ky_Hoc_Tap_NguoiDungMaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap");

            migrationBuilder.DropIndex(
                name: "IX_Ket_Qua_Thi_NguoiDungMaNguoiDung",
                table: "Ket_Qua_Thi");

            migrationBuilder.DropColumn(
                name: "NguoiDungMaNguoiDung",
                table: "Phan_Tich_AI");

            migrationBuilder.DropColumn(
                name: "NguoiDungMaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap");

            migrationBuilder.DropColumn(
                name: "NguoiDungMaNguoiDung",
                table: "Ket_Qua_Thi");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Ket_Qua_Thi",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ket_Qua_Thi_UserId",
                table: "Ket_Qua_Thi",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ket_Qua_Thi_AspNetUsers_UserId",
                table: "Ket_Qua_Thi",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ket_Qua_Thi_AspNetUsers_UserId",
                table: "Ket_Qua_Thi");

            migrationBuilder.DropIndex(
                name: "IX_Ket_Qua_Thi_UserId",
                table: "Ket_Qua_Thi");

            migrationBuilder.AddColumn<int>(
                name: "NguoiDungMaNguoiDung",
                table: "Phan_Tich_AI",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NguoiDungMaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Ket_Qua_Thi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NguoiDungMaNguoiDung",
                table: "Ket_Qua_Thi",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Nguoi_Dung",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiemMucTieu = table.Column<float>(type: "real", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nguoi_Dung", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Phan_Tich_AI_NguoiDungMaNguoiDung",
                table: "Phan_Tich_AI",
                column: "NguoiDungMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_Nhat_Ky_Hoc_Tap_NguoiDungMaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap",
                column: "NguoiDungMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_Ket_Qua_Thi_NguoiDungMaNguoiDung",
                table: "Ket_Qua_Thi",
                column: "NguoiDungMaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_Ket_Qua_Thi_Nguoi_Dung_NguoiDungMaNguoiDung",
                table: "Ket_Qua_Thi",
                column: "NguoiDungMaNguoiDung",
                principalTable: "Nguoi_Dung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_Nhat_Ky_Hoc_Tap_Nguoi_Dung_NguoiDungMaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap",
                column: "NguoiDungMaNguoiDung",
                principalTable: "Nguoi_Dung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_Phan_Tich_AI_Nguoi_Dung_NguoiDungMaNguoiDung",
                table: "Phan_Tich_AI",
                column: "NguoiDungMaNguoiDung",
                principalTable: "Nguoi_Dung",
                principalColumn: "MaNguoiDung");
        }
    }
}
