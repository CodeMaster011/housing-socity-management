using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HSM.WebApp.Migrations
{
    public partial class ChargesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Charges",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OnArea = table.Column<double>(type: "double", nullable: true),
                    OnFlatAmount = table.Column<double>(type: "double", nullable: true),
                    MaxAmount = table.Column<double>(type: "double", nullable: true),
                    MinAmount = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charges", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Charges");
        }
    }
}
