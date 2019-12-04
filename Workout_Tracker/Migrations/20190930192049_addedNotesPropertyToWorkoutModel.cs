using Microsoft.EntityFrameworkCore.Migrations;

namespace Workout_Tracker.Migrations
{
    public partial class addedNotesPropertyToWorkoutModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Workouts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Workouts");
        }
    }
}
