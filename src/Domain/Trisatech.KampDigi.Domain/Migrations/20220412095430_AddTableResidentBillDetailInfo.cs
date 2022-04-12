using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trisatech.KampDigi.Domain.Migrations
{
    public partial class AddTableResidentBillDetailInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResidentBillBaseInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Nominal = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    MontlyBillOpenDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DueDateNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedDate = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AuditActivty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResidentBillBaseInfos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ResidentBillDetailInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ResidentBillBaseInfoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nominal = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Desc = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedDate = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AuditActivty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResidentBillDetailInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResidentBillDetailInfos_ResidentBillBaseInfos_ResidentBillBa~",
                        column: x => x.ResidentBillBaseInfoId,
                        principalTable: "ResidentBillBaseInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ResidentBillDetailInfos_ResidentBillBaseInfoId",
                table: "ResidentBillDetailInfos",
                column: "ResidentBillBaseInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResidentBillDetailInfos");

            migrationBuilder.DropTable(
                name: "ResidentBillBaseInfos");
        }
    }
}
