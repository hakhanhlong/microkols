using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdateCampaignTypeCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountCharge",
                table: "CampaignType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceCharge",
                table: "CampaignType",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountCharge",
                table: "CampaignType");

            migrationBuilder.DropColumn(
                name: "ServiceCharge",
                table: "CampaignType");
        }
    }
}
