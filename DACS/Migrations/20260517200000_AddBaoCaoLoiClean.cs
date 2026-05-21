using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    public partial class AddBaoCaoLoiClean : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Chi tao bang Bao_Cao_Loi, khong dong cham AspNetUserTokens
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Bao_Cao_Loi' AND xtype='U')
                BEGIN
                    CREATE TABLE [Bao_Cao_Loi] (
                        [Id]          INT IDENTITY(1,1) NOT NULL,
                        [UserId]      NVARCHAR(450)     NOT NULL,
                        [MaDeThi]     INT               NOT NULL,
                        [MaCauHoi]    INT               NULL,
                        [LoaiBaoCao]  NVARCHAR(MAX)     NOT NULL,
                        [MoTa]        NVARCHAR(MAX)     NOT NULL,
                        [TrangThai]   NVARCHAR(MAX)     NOT NULL DEFAULT N'Cho xu ly',
                        [GhiChuAdmin] NVARCHAR(MAX)     NULL,
                        [NgayTao]     DATETIME2         NOT NULL DEFAULT GETDATE(),
                        [NgayXuLy]    DATETIME2         NULL,
                        CONSTRAINT [PK_Bao_Cao_Loi] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_BaoCao_User]  FOREIGN KEY ([UserId])  REFERENCES [AspNetUsers]([Id]) ON DELETE CASCADE,
                        CONSTRAINT [FK_BaoCao_DeThi] FOREIGN KEY ([MaDeThi]) REFERENCES [De_Thi]([MaDeThi]) ON DELETE CASCADE
                    );
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sysobjects WHERE name='Bao_Cao_Loi' AND xtype='U') DROP TABLE [Bao_Cao_Loi];");
        }
    }
}
