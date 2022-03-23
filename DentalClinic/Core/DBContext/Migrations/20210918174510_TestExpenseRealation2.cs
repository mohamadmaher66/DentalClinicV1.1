using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDBContext.Migrations
{
    public partial class TestExpenseRealation2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "User",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Patient",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MedicalHistory",
                newName: "MedicalHistoryId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Expense",
                newName: "ExpenseId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Clinic",
                newName: "ClinicId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Attachment",
                newName: "AttachmenthId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AppointmentTooth",
                newName: "AppointmentToothId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AppointmentCategory",
                newName: "AppointmentCategoryId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AppointmentAddition",
                newName: "AppointmentAdditionId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Appointment",
                newName: "AppointmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "User",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Patient",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MedicalHistoryId",
                table: "MedicalHistory",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ExpenseId",
                table: "Expense",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ClinicId",
                table: "Clinic",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AttachmenthId",
                table: "Attachment",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AppointmentToothId",
                table: "AppointmentTooth",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AppointmentCategoryId",
                table: "AppointmentCategory",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AppointmentAdditionId",
                table: "AppointmentAddition",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "Appointment",
                newName: "Id");
        }
    }
}
