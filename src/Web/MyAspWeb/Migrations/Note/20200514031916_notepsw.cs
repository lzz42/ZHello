using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAspWeb.Migrations.Note
{
    public partial class notepsw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attachment",
                table: "Notes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Notes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Notes");
        }
    }
}
