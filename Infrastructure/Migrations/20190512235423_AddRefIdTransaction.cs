using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddRefIdTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_CampaignType_CampaignTypeId",
                table: "Campaign");

            migrationBuilder.AddColumn<int>(
                name: "RefId",
                table: "Transaction",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_CampaignType_CampaignTypeId",
                table: "Campaign",
                column: "CampaignTypeId",
                principalTable: "CampaignType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_CampaignType_CampaignTypeId",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "RefId",
                table: "Transaction");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_CampaignType_CampaignTypeId",
                table: "Campaign",
                column: "CampaignTypeId",
                principalTable: "CampaignType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
