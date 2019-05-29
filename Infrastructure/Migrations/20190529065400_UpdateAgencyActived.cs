using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdateAgencyActived : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Published",
                table: "Agency",
                newName: "Actived");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Account",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Actived",
                table: "Agency",
                newName: "Published");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Account",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
