using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HSM.WebApp.Migrations
{
    public partial class TransactionPassthroughAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsContra",
                table: "Transactions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PassthroughId",
                table: "Transactions",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "Ledgers",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CalculatedOn",
                table: "Ledgers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TransactionPassthrough",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Balance = table.Column<double>(type: "double", nullable: false),
                    CalculatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionPassthrough", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PassthroughId",
                table: "Transactions",
                column: "PassthroughId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionPassthrough_PassthroughId",
                table: "Transactions",
                column: "PassthroughId",
                principalTable: "TransactionPassthrough",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionPassthrough_PassthroughId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionPassthrough");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_PassthroughId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "IsContra",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PassthroughId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "CalculatedOn",
                table: "Ledgers");
        }
    }
}
