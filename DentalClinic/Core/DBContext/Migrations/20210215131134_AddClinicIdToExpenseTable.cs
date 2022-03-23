using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDBContext.Migrations
{
    public partial class AddClinicIdToExpenseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClinicId",
                table: "Expense",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "Expense");
        }
    }
}
