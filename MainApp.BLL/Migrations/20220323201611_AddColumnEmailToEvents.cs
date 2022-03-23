using Microsoft.EntityFrameworkCore.Migrations;

namespace MainApp.BLL.Migrations
{
    public partial class AddColumnEmailToEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Events");
        }
    }
}
