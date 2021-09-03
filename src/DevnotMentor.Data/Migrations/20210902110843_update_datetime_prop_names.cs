using Microsoft.EntityFrameworkCore.Migrations;

namespace DevnotMentor.Data.Migrations
{
    public partial class update_datetime_prop_names : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MentorStartDate",
                table: "Mentorships",
                newName: "StartedAt");

            migrationBuilder.RenameColumn(
                name: "MentorEndDate",
                table: "Mentorships",
                newName: "FinishedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartedAt",
                table: "Mentorships",
                newName: "MentorStartDate");

            migrationBuilder.RenameColumn(
                name: "FinishedAt",
                table: "Mentorships",
                newName: "MentorEndDate");
        }
    }
}
