using Microsoft.EntityFrameworkCore.Migrations;

namespace Workout_Tracker.Migrations
{
    public partial class correctedcompletedspellinginexercise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Compelted",
                table: "Exercises",
                newName: "Completed");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Completed",
                table: "Exercises",
                newName: "Compelted");
        }
    }
}
