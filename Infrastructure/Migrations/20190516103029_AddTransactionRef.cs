using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddTransactionRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Balance",
                table: "TransactionHistory",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "Amount",
                table: "TransactionHistory",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "Amount",
                table: "Transaction",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "AdminNote",
                table: "Transaction",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefData",
                table: "Transaction",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminNote",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "RefData",
                table: "Transaction");

            migrationBuilder.AlterColumn<int>(
                name: "Balance",
                table: "TransactionHistory",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "TransactionHistory",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "Transaction",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
