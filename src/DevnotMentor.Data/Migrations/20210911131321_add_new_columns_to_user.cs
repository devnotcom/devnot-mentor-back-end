using Microsoft.EntityFrameworkCore.Migrations;

namespace DevnotMentor.Data.Migrations
{
    public partial class add_new_columns_to_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMentee",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMentor",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMentee",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsMentor",
                table: "Users");
        }
    }
}
