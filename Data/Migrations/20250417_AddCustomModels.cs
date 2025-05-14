using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ST10122162_Agri_Energy_Connect_Platform.Data.Migrations
{
    public partial class AddCustomModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add FirstName and LastName to AspNetUsers
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FarmerId",
                table: "AspNetUsers",
                nullable: true);

            // Create Farmers table
            migrationBuilder.CreateTable(
                name: "Farmers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FarmName = table.Column<string>(maxLength: 100, nullable: false),
                    Address = table.Column<string>(maxLength: 200, nullable: false),
                    ContactNumber = table.Column<string>(nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farmers", x => x.Id);
                });

            // Create Products table
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Category = table.Column<string>(maxLength: 50, nullable: false),
                    ProductionDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    Unit = table.Column<string>(maxLength: 20, nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    FarmerId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Add foreign key from AspNetUsers to Farmers
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FarmerId",
                table: "AspNetUsers",
                column: "FarmerId",
                unique: true,
                filter: "[FarmerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_FarmerId",
                table: "Products",
                column: "FarmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Farmers_FarmerId",
                table: "AspNetUsers",
                column: "FarmerId",
                principalTable: "Farmers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove foreign key from AspNetUsers to Farmers
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Farmers_FarmerId",
                table: "AspNetUsers");

            // Drop Products table
            migrationBuilder.DropTable(
                name: "Products");

            // Drop Farmers table
            migrationBuilder.DropTable(
                name: "Farmers");

            // Remove columns from AspNetUsers
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FarmerId",
                table: "AspNetUsers");
        }
    }
}
