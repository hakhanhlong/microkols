using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdateAccountProviderFollowerFriend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FollowersCount",
                table: "AccountProvider",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FriendsCount",
                table: "AccountProvider",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowersCount",
                table: "AccountProvider");

            migrationBuilder.DropColumn(
                name: "FriendsCount",
                table: "AccountProvider");
        }
    }
}
