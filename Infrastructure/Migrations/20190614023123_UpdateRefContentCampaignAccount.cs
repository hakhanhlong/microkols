using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdateRefContentCampaignAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notification");

            migrationBuilder.AddColumn<string>(
                name: "RefContent",
                table: "CampaignAccount",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefContent",
                table: "CampaignAccount");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Notification",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notification",
                nullable: true);
        }
    }
}
