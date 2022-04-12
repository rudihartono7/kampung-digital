using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trisatech.KampDigi.Domain.Migrations
{
    public partial class AlterTableUser1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ResidentId",
                table: "Users",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResidentId",
                table: "Users");
        }
    }
}
