using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddAccountFacebookPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "AccountProvider",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "AccountProvider",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Expired",
                table: "AccountProvider",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "AccountFbPost",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    UserCreated = table.Column<string>(nullable: true),
                    UserModified = table.Column<string>(nullable: true),
                    AccountId = table.Column<int>(nullable: false),
                    Picture = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    PostId = table.Column<string>(nullable: true),
                    PostTime = table.Column<DateTime>(nullable: false),
                    ShareCount = table.Column<int>(nullable: false),
                    LikeCount = table.Column<int>(nullable: false),
                    CommentCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountFbPost", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountFbPost");

            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "AccountProvider");

            migrationBuilder.DropColumn(
                name: "Expired",
                table: "AccountProvider");

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "AccountProvider",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
