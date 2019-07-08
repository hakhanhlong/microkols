using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddCampaignCodeIgnoreCampaignTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Campaign",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IgnoreCampaignTypes",
                table: "Account",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "IgnoreCampaignTypes",
                table: "Account");
        }
    }
}
