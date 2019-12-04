using Microsoft.EntityFrameworkCore.Migrations;

namespace Workout_Tracker.Migrations
{
    public partial class addedCompleteToSetModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Exercise_Sets",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Exercise_Sets");
        }
    }
}
