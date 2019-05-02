using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class RemoveAccountDistrictCityRelastionship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_City_CityId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Account_District_DistrictId",
                table: "Account");

            migrationBuilder.AlterColumn<int>(
                name: "DistrictId",
                table: "Account",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Account",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Account_City_CityId",
                table: "Account",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_District_DistrictId",
                table: "Account",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_City_CityId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Account_District_DistrictId",
                table: "Account");

            migrationBuilder.AlterColumn<int>(
                name: "DistrictId",
                table: "Account",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Account",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_City_CityId",
                table: "Account",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_District_DistrictId",
                table: "Account",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
