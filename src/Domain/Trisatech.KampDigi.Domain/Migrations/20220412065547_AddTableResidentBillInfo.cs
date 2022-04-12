using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trisatech.KampDigi.Domain.Migrations
{
    public partial class AddTableResidentBillInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "ResidentBills",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ResidentBills");
        }
    }
}
