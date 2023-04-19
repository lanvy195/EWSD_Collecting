using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEWSD_Collecting.Migrations
{
    public partial class AddComment11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Comments_IdeaId",
                table: "Comments",
                column: "IdeaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Ideas_IdeaId",
                table: "Comments",
                column: "IdeaId",
                principalTable: "Ideas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Ideas_IdeaId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_IdeaId",
                table: "Comments");
        }
    }
}
