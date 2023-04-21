using Microsoft.EntityFrameworkCore.Migrations;

namespace FMStyles_API.Migrations
{
    public partial class Editdbb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeleteFlag",
                table: "Suppliers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteFlag",
                table: "Suppliers");
        }
    }
}
