using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JJS.Infrastructure.Identity.Migrations
{
    public partial class addauditcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                schema: "Identity",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                schema: "Identity",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Identity",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompany",
                schema: "Identity",
                table: "User",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Identity",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                schema: "Identity",
                table: "User",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsCompany",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastModified",
                schema: "Identity",
                table: "User");
        }
    }
}
