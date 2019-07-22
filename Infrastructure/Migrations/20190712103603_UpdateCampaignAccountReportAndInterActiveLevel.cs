using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdateCampaignAccountReportAndInterActiveLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InteractiveLevel",
                table: "CampaignAccount",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportNote",
                table: "CampaignAccount",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportStatus",
                table: "CampaignAccount",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InteractiveLevel",
                table: "CampaignAccount");

            migrationBuilder.DropColumn(
                name: "ReportNote",
                table: "CampaignAccount");

            migrationBuilder.DropColumn(
                name: "ReportStatus",
                table: "CampaignAccount");
        }
    }
}
