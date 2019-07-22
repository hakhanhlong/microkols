using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdateCampaignAccountRatingReportImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InteractiveLevel",
                table: "CampaignAccount");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "CampaignAccount",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportImages",
                table: "CampaignAccount",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "CampaignAccount");

            migrationBuilder.DropColumn(
                name: "ReportImages",
                table: "CampaignAccount");

            migrationBuilder.AddColumn<int>(
                name: "InteractiveLevel",
                table: "CampaignAccount",
                nullable: true);
        }
    }
}
