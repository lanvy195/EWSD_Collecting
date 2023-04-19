using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEWSD_Collecting.Migrations
{
    public partial class UpdateDatetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateDatetime",
                table: "Departments",
                newName: "LastUpdate");

            migrationBuilder.RenameColumn(
                name: "CreateDatetime",
                table: "Categories",
                newName: "LastUpdate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdate",
                table: "Departments",
                newName: "CreateDatetime");

            migrationBuilder.RenameColumn(
                name: "LastUpdate",
                table: "Categories",
                newName: "CreateDatetime");
        }
    }
}
