using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tracking.Migrations.PostgresServerMigrations
{
    /// <inheritdoc />
    public partial class AddTrainerPictureToPg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrainerPicture",
                table: "Trainers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrainerPicture",
                table: "Trainers");
        }
    }
}
