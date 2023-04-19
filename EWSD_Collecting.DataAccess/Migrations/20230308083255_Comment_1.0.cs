using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEWSD_Collecting.Migrations
{
    public partial class Comment_10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDatetime",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDatetime",
                table: "Comments");
        }
    }
}
