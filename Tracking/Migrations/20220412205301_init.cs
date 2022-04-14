using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracking.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

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
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 1, new DateTime(2022, 4, 12, 22, 53, 1, 219, DateTimeKind.Local).AddTicks(4744), "pssg@example.com", "Patryk", "Szwermer", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 2, new DateTime(2022, 4, 12, 22, 53, 1, 222, DateTimeKind.Local).AddTicks(3916), "ps@example.com", "Przemyslaw", "Sawicki", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 3, new DateTime(2022, 4, 12, 22, 53, 1, 222, DateTimeKind.Local).AddTicks(3940), "md@example.com", "Marcin", "Dabrowski", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 4, new DateTime(2022, 4, 12, 22, 53, 1, 222, DateTimeKind.Local).AddTicks(3944), "pk@example.com", "Piotr", "Katny", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 5, new DateTime(2022, 4, 12, 22, 53, 1, 222, DateTimeKind.Local).AddTicks(3946), "md@example.com", "Marcin", "Dudzic", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 6, new DateTime(2022, 4, 12, 22, 53, 1, 222, DateTimeKind.Local).AddTicks(3951), "mk@example.com", "Maciej", "Krakowiak", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 7, new DateTime(2022, 4, 12, 22, 53, 1, 222, DateTimeKind.Local).AddTicks(3954), "mc@example.com", "Mateusz", "Cebula", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 8, new DateTime(2022, 4, 12, 22, 53, 1, 222, DateTimeKind.Local).AddTicks(3956), "jk@example.com", "Jakub", "Nowikowski", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 9, new DateTime(2022, 4, 12, 22, 53, 1, 222, DateTimeKind.Local).AddTicks(3959), "jc@example.com", "Jan", "Choma", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 10, new DateTime(2022, 4, 12, 22, 53, 1, 222, DateTimeKind.Local).AddTicks(3962), "mp@example.com", "Marcin", "Przypek", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 11, new DateTime(2022, 4, 12, 22, 53, 1, 222, DateTimeKind.Local).AddTicks(3965), "ms@example.com", "Michal", "Sosnowski", "" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 12, new DateTime(2022, 4, 12, 22, 53, 1, 222, DateTimeKind.Local).AddTicks(3967), "mt@example.com", "Maciej", "Tyszka", "" });
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
