using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdateAccountInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "AccountProvider",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<bool>(
                name: "Actived",
                table: "Account",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Account",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Account",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "Account",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserCreated",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserModified",
                table: "Account",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Banner",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Published = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    UserCreated = table.Column<string>(nullable: true),
                    UserModified = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banner", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_DistrictId",
                table: "Account",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_District_DistrictId",
                table: "Account",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_District_DistrictId",
                table: "Account");

            migrationBuilder.DropTable(
                name: "Banner");

            migrationBuilder.DropIndex(
                name: "IX_Account_DistrictId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Actived",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "UserCreated",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "UserModified",
                table: "Account");

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "AccountProvider",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
