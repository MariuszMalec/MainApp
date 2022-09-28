using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracking.Migrations
{
    public partial class initMsql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 9, 28, 21, 57, 40, 621, DateTimeKind.Local).AddTicks(3933), "pssg@example.com", "Patryk", "Szwermer", "" },
                    { 2, new DateTime(2022, 9, 28, 21, 57, 40, 626, DateTimeKind.Local).AddTicks(1833), "ps@example.com", "Przemyslaw", "Sawicki", "" },
                    { 3, new DateTime(2022, 9, 28, 21, 57, 40, 626, DateTimeKind.Local).AddTicks(1862), "md@example.com", "Marcin", "Dabrowski", "" },
                    { 4, new DateTime(2022, 9, 28, 21, 57, 40, 626, DateTimeKind.Local).AddTicks(1867), "pk@example.com", "Piotr", "Katny", "" },
                    { 5, new DateTime(2022, 9, 28, 21, 57, 40, 626, DateTimeKind.Local).AddTicks(1869), "md@example.com", "Marcin", "Dudzic", "" },
                    { 6, new DateTime(2022, 9, 28, 21, 57, 40, 626, DateTimeKind.Local).AddTicks(1875), "mk@example.com", "Maciej", "Krakowiak", "" },
                    { 7, new DateTime(2022, 9, 28, 21, 57, 40, 626, DateTimeKind.Local).AddTicks(1878), "mc@example.com", "Mateusz", "Cebula", "" },
                    { 8, new DateTime(2022, 9, 28, 21, 57, 40, 626, DateTimeKind.Local).AddTicks(1880), "jk@example.com", "Jakub", "Nowikowski", "" },
                    { 9, new DateTime(2022, 9, 28, 21, 57, 40, 626, DateTimeKind.Local).AddTicks(1883), "jc@example.com", "Jan", "Choma", "" },
                    { 10, new DateTime(2022, 9, 28, 21, 57, 40, 626, DateTimeKind.Local).AddTicks(1887), "mp@example.com", "Marcin", "Przypek", "" },
                    { 11, new DateTime(2022, 9, 28, 21, 57, 40, 626, DateTimeKind.Local).AddTicks(1890), "ms@example.com", "Michal", "Sosnowski", "" },
                    { 12, new DateTime(2022, 9, 28, 21, 57, 40, 626, DateTimeKind.Local).AddTicks(1893), "mt@example.com", "Maciej", "Tyszka", "" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
