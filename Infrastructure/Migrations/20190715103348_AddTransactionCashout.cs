using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddTransactionCashout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CashoutDate",
                table: "Transaction",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCashOut",
                table: "Transaction",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashoutDate",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "IsCashOut",
                table: "Transaction");
        }
    }
}
