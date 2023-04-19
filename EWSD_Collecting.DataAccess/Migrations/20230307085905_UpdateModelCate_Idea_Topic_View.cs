using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEWSD_Collecting.Migrations
{
    public partial class UpdateModelCate_Idea_Topic_View : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "View",
                table: "Ideas",
                newName: "Views");

            migrationBuilder.AddColumn<int>(
                name: "React",
                table: "Views",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "Ideas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Ideas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "React",
                table: "Views");

            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "Ideas");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Ideas");

            migrationBuilder.RenameColumn(
                name: "Views",
                table: "Ideas",
                newName: "View");
        }
    }
}
