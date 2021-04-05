using Microsoft.EntityFrameworkCore.Migrations;

namespace JJS.Infrastructure.Persistence.Migrations
{
    public partial class AlterColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessStream_Company_CompanyId",
                table: "BusinessStream");

            migrationBuilder.RenameColumn(
                name: "Dscription",
                table: "Company",
                newName: "Description");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "BusinessStream",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessStream_Company_CompanyId",
                table: "BusinessStream",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessStream_Company_CompanyId",
                table: "BusinessStream");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Company",
                newName: "Dscription");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "BusinessStream",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessStream_Company_CompanyId",
                table: "BusinessStream",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
