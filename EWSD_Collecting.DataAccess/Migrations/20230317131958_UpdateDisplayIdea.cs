using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEWSD_Collecting.Migrations
{
    public partial class UpdateDisplayIdea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_IdentityUserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_AspNetUsers_IdentityUserId",
                table: "Ideas");

            migrationBuilder.RenameColumn(
                name: "IdentityUserId",
                table: "Views",
                newName: "ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "IdentityUserId",
                table: "Ideas",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ideas_IdentityUserId",
                table: "Ideas",
                newName: "IX_Ideas_ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "isName",
                table: "Comments",
                newName: "isDisplay");

            migrationBuilder.RenameColumn(
                name: "IdentityUserId",
                table: "Comments",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_IdentityUserId",
                table: "Comments",
                newName: "IX_Comments_ApplicationUserId");

            migrationBuilder.AddColumn<bool>(
                name: "isAgree",
                table: "Ideas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDisplay",
                table: "Ideas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_ApplicationUserId",
                table: "Comments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_AspNetUsers_ApplicationUserId",
                table: "Ideas",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_ApplicationUserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_AspNetUsers_ApplicationUserId",
                table: "Ideas");

            migrationBuilder.DropColumn(
                name: "isAgree",
                table: "Ideas");

            migrationBuilder.DropColumn(
                name: "isDisplay",
                table: "Ideas");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Views",
                newName: "IdentityUserId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Ideas",
                newName: "IdentityUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ideas_ApplicationUserId",
                table: "Ideas",
                newName: "IX_Ideas_IdentityUserId");

            migrationBuilder.RenameColumn(
                name: "isDisplay",
                table: "Comments",
                newName: "isName");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Comments",
                newName: "IdentityUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ApplicationUserId",
                table: "Comments",
                newName: "IX_Comments_IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_IdentityUserId",
                table: "Comments",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_AspNetUsers_IdentityUserId",
                table: "Ideas",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
