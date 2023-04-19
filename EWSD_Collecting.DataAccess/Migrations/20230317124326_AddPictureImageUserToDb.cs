using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEWSD_Collecting.Migrations
{
    public partial class AddPictureImageUserToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "AspNetUsers");
        }
    }
}
