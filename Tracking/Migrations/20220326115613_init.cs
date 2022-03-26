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
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "Phone" },
                values: new object[] { 1, new DateTime(2022, 3, 26, 11, 56, 13, 272, DateTimeKind.Utc).AddTicks(5197), "ps@example.com", "Patryk", "Szwermer", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "Phone" },
                values: new object[] { 2, new DateTime(2022, 3, 26, 11, 56, 13, 272, DateTimeKind.Utc).AddTicks(5468), "ps@example.com", "Przemyslaw", "sawicki", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "Phone" },
                values: new object[] { 3, new DateTime(2022, 3, 26, 11, 56, 13, 272, DateTimeKind.Utc).AddTicks(5471), "md@example.com", "Marcin", "Dabrowski", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "Phone" },
                values: new object[] { 4, new DateTime(2022, 3, 26, 11, 56, 13, 272, DateTimeKind.Utc).AddTicks(5472), "pk@example.com", "Piotr", "Katny", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "Phone" },
                values: new object[] { 5, new DateTime(2022, 3, 26, 11, 56, 13, 272, DateTimeKind.Utc).AddTicks(5474), "md@example.com", "Marcin", "Dudzic", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "Phone" },
                values: new object[] { 6, new DateTime(2022, 3, 26, 11, 56, 13, 272, DateTimeKind.Utc).AddTicks(5477), "mk@example.com", "Maciej", "Krakowiak", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "Phone" },
                values: new object[] { 7, new DateTime(2022, 3, 26, 11, 56, 13, 272, DateTimeKind.Utc).AddTicks(5479), "jk@example.com", "Jakub", "Nowikowski", "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trainers");
        }
    }
}
