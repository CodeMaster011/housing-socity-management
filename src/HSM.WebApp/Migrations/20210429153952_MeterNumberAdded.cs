using Microsoft.EntityFrameworkCore.Migrations;

namespace HSM.WebApp.Migrations
{
    public partial class MeterNumberAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeterNumber",
                table: "Units",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LedgerId",
                table: "Charges",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Charges_LedgerId",
                table: "Charges",
                column: "LedgerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Charges_Ledgers_LedgerId",
                table: "Charges",
                column: "LedgerId",
                principalTable: "Ledgers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charges_Ledgers_LedgerId",
                table: "Charges");

            migrationBuilder.DropIndex(
                name: "IX_Charges_LedgerId",
                table: "Charges");

            migrationBuilder.DropColumn(
                name: "MeterNumber",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "Charges");
        }
    }
}
