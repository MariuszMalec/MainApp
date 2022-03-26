using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracking.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Action = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 1, new DateTime(2022, 3, 26, 23, 10, 39, 388, DateTimeKind.Local).AddTicks(1481), "ps@example.com", "Patryk", "Szwermer", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 2, new DateTime(2022, 3, 26, 23, 10, 39, 391, DateTimeKind.Local).AddTicks(3438), "ps@example.com", "Przemyslaw", "sawicki", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 3, new DateTime(2022, 3, 26, 23, 10, 39, 391, DateTimeKind.Local).AddTicks(3464), "md@example.com", "Marcin", "Dabrowski", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 4, new DateTime(2022, 3, 26, 23, 10, 39, 391, DateTimeKind.Local).AddTicks(3468), "pk@example.com", "Piotr", "Katny", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 5, new DateTime(2022, 3, 26, 23, 10, 39, 391, DateTimeKind.Local).AddTicks(3471), "md@example.com", "Marcin", "Dudzic", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 6, new DateTime(2022, 3, 26, 23, 10, 39, 391, DateTimeKind.Local).AddTicks(3477), "mk@example.com", "Maciej", "Krakowiak", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 7, new DateTime(2022, 3, 26, 23, 10, 39, 391, DateTimeKind.Local).AddTicks(3479), "jk@example.com", "Jakub", "Nowikowski", "" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");
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
