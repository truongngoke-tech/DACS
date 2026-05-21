using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class AddChiTietToKetQua : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ket_Qua_Thi_Nguoi_Dung_MaNguoiDung",
                table: "Ket_Qua_Thi");

            migrationBuilder.DropForeignKey(
                name: "FK_Nhat_Ky_Hoc_Tap_Nguoi_Dung_MaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap");

            migrationBuilder.DropForeignKey(
                name: "FK_Phan_Tich_AI_Nguoi_Dung_MaNguoiDung",
                table: "Phan_Tich_AI");

            migrationBuilder.DropIndex(
                name: "IX_Phan_Tich_AI_MaNguoiDung",
                table: "Phan_Tich_AI");

            migrationBuilder.DropIndex(
                name: "IX_Nhat_Ky_Hoc_Tap_MaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap");

            migrationBuilder.DropIndex(
                name: "IX_Ket_Qua_Thi_MaNguoiDung",
                table: "Ket_Qua_Thi");

            migrationBuilder.DropColumn(
                name: "MaNguoiDung",
                table: "Phan_Tich_AI");

            migrationBuilder.DropColumn(
                name: "MaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap");

            migrationBuilder.DropColumn(
                name: "MaNguoiDung",
                table: "Ket_Qua_Thi");

            migrationBuilder.AddColumn<int>(
                name: "NguoiDungMaNguoiDung",
                table: "Phan_Tich_AI",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Phan_Tich_AI",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NguoiDungMaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Nhat_Ky_Hoc_Tap",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HoTen",
                table: "Nguoi_Dung",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Nguoi_Dung",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ChiTietJson",
                table: "Ket_Qua_Thi",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NguoiDungMaNguoiDung",
                table: "Ket_Qua_Thi",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Ket_Qua_Thi",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhuongAnD",
                table: "Cau_Hoi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhuongAnC",
                table: "Cau_Hoi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhuongAnB",
                table: "Cau_Hoi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhuongAnA",
                table: "Cau_Hoi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MucDoKho",
                table: "Cau_Hoi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DapAnDung",
                table: "Cau_Hoi",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "UserId",
                table: "Phan_Tich_AI");

            migrationBuilder.DropColumn(
                name: "NguoiDungMaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Nhat_Ky_Hoc_Tap");

            migrationBuilder.DropColumn(
                name: "ChiTietJson",
                table: "Ket_Qua_Thi");

            migrationBuilder.DropColumn(
                name: "NguoiDungMaNguoiDung",
                table: "Ket_Qua_Thi");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ket_Qua_Thi");

            migrationBuilder.AddColumn<int>(
                name: "MaNguoiDung",
                table: "Phan_Tich_AI",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "HoTen",
                table: "Nguoi_Dung",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Nguoi_Dung",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaNguoiDung",
                table: "Ket_Qua_Thi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PhuongAnD",
                table: "Cau_Hoi",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhuongAnC",
                table: "Cau_Hoi",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhuongAnB",
                table: "Cau_Hoi",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhuongAnA",
                table: "Cau_Hoi",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MucDoKho",
                table: "Cau_Hoi",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DapAnDung",
                table: "Cau_Hoi",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Phan_Tich_AI_MaNguoiDung",
                table: "Phan_Tich_AI",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_Nhat_Ky_Hoc_Tap_MaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_Ket_Qua_Thi_MaNguoiDung",
                table: "Ket_Qua_Thi",
                column: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_Ket_Qua_Thi_Nguoi_Dung_MaNguoiDung",
                table: "Ket_Qua_Thi",
                column: "MaNguoiDung",
                principalTable: "Nguoi_Dung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nhat_Ky_Hoc_Tap_Nguoi_Dung_MaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap",
                column: "MaNguoiDung",
                principalTable: "Nguoi_Dung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Phan_Tich_AI_Nguoi_Dung_MaNguoiDung",
                table: "Phan_Tich_AI",
                column: "MaNguoiDung",
                principalTable: "Nguoi_Dung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
