using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chu_De",
                columns: table => new
                {
                    MaChuDe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenChuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaChuDeCha = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chu_De", x => x.MaChuDe);
                    table.ForeignKey(
                        name: "FK_Chu_De_Chu_De_MaChuDeCha",
                        column: x => x.MaChuDeCha,
                        principalTable: "Chu_De",
                        principalColumn: "MaChuDe");
                });

            migrationBuilder.CreateTable(
                name: "Nguoi_Dung",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiemMucTieu = table.Column<float>(type: "real", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nguoi_Dung", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "De_Thi",
                columns: table => new
                {
                    MaDeThi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDeThi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoCauHoi = table.Column<int>(type: "int", nullable: true),
                    ThoiGian = table.Column<int>(type: "int", nullable: true),
                    MaChuDe = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_De_Thi", x => x.MaDeThi);
                    table.ForeignKey(
                        name: "FK_De_Thi_Chu_De_MaChuDe",
                        column: x => x.MaChuDe,
                        principalTable: "Chu_De",
                        principalColumn: "MaChuDe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Phan_Tich_AI",
                columns: table => new
                {
                    MaPhanTich = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaChuDeYeu = table.Column<int>(type: "int", nullable: false),
                    LoiKhuyenAI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CapNhatLanCuoi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phan_Tich_AI", x => x.MaPhanTich);
                    table.ForeignKey(
                        name: "FK_Phan_Tich_AI_Chu_De_MaChuDeYeu",
                        column: x => x.MaChuDeYeu,
                        principalTable: "Chu_De",
                        principalColumn: "MaChuDe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Phan_Tich_AI_Nguoi_Dung_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "Nguoi_Dung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cau_Hoi",
                columns: table => new
                {
                    MaCauHoi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhuongAnA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhuongAnB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhuongAnC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhuongAnD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DapAnDung = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    MucDoKho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaChuDe = table.Column<int>(type: "int", nullable: false),
                    MaDeThi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cau_Hoi", x => x.MaCauHoi);
                    table.ForeignKey(
                        name: "FK_Cau_Hoi_Chu_De_MaChuDe",
                        column: x => x.MaChuDe,
                        principalTable: "Chu_De",
                        principalColumn: "MaChuDe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cau_Hoi_De_Thi_MaDeThi",
                        column: x => x.MaDeThi,
                        principalTable: "De_Thi",
                        principalColumn: "MaDeThi");
                });

            migrationBuilder.CreateTable(
                name: "Ket_Qua_Thi",
                columns: table => new
                {
                    MaKetQua = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaDeThi = table.Column<int>(type: "int", nullable: false),
                    TongDiem = table.Column<float>(type: "real", nullable: false),
                    SoCauDung = table.Column<int>(type: "int", nullable: false),
                    NgayThi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ket_Qua_Thi", x => x.MaKetQua);
                    table.ForeignKey(
                        name: "FK_Ket_Qua_Thi_De_Thi_MaDeThi",
                        column: x => x.MaDeThi,
                        principalTable: "De_Thi",
                        principalColumn: "MaDeThi",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ket_Qua_Thi_Nguoi_Dung_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "Nguoi_Dung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nhat_Ky_Hoc_Tap",
                columns: table => new
                {
                    MaNhatKy = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaCauHoi = table.Column<int>(type: "int", nullable: false),
                    DapAnDaChon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LaDung = table.Column<bool>(type: "bit", nullable: false),
                    ThoiGianLamGiay = table.Column<int>(type: "int", nullable: false),
                    NgayThucHien = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nhat_Ky_Hoc_Tap", x => x.MaNhatKy);
                    table.ForeignKey(
                        name: "FK_Nhat_Ky_Hoc_Tap_Cau_Hoi_MaCauHoi",
                        column: x => x.MaCauHoi,
                        principalTable: "Cau_Hoi",
                        principalColumn: "MaCauHoi",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nhat_Ky_Hoc_Tap_Nguoi_Dung_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "Nguoi_Dung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cau_Hoi_MaChuDe",
                table: "Cau_Hoi",
                column: "MaChuDe");

            migrationBuilder.CreateIndex(
                name: "IX_Cau_Hoi_MaDeThi",
                table: "Cau_Hoi",
                column: "MaDeThi");

            migrationBuilder.CreateIndex(
                name: "IX_Chu_De_MaChuDeCha",
                table: "Chu_De",
                column: "MaChuDeCha");

            migrationBuilder.CreateIndex(
                name: "IX_De_Thi_MaChuDe",
                table: "De_Thi",
                column: "MaChuDe");

            migrationBuilder.CreateIndex(
                name: "IX_Ket_Qua_Thi_MaDeThi",
                table: "Ket_Qua_Thi",
                column: "MaDeThi");

            migrationBuilder.CreateIndex(
                name: "IX_Ket_Qua_Thi_MaNguoiDung",
                table: "Ket_Qua_Thi",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_Nhat_Ky_Hoc_Tap_MaCauHoi",
                table: "Nhat_Ky_Hoc_Tap",
                column: "MaCauHoi");

            migrationBuilder.CreateIndex(
                name: "IX_Nhat_Ky_Hoc_Tap_MaNguoiDung",
                table: "Nhat_Ky_Hoc_Tap",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_Phan_Tich_AI_MaChuDeYeu",
                table: "Phan_Tich_AI",
                column: "MaChuDeYeu");

            migrationBuilder.CreateIndex(
                name: "IX_Phan_Tich_AI_MaNguoiDung",
                table: "Phan_Tich_AI",
                column: "MaNguoiDung");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Ket_Qua_Thi");

            migrationBuilder.DropTable(
                name: "Nhat_Ky_Hoc_Tap");

            migrationBuilder.DropTable(
                name: "Phan_Tich_AI");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Cau_Hoi");

            migrationBuilder.DropTable(
                name: "Nguoi_Dung");

            migrationBuilder.DropTable(
                name: "De_Thi");

            migrationBuilder.DropTable(
                name: "Chu_De");
        }
    }
}
