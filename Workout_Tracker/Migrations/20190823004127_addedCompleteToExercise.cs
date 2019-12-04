using Microsoft.EntityFrameworkCore.Migrations;

namespace Workout_Tracker.Migrations
{
    public partial class addedCompleteToExercise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Compelted",
                table: "Exercises",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Compelted",
                table: "Exercises");
        }
    }
}
