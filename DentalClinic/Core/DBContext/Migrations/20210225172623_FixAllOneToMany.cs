using Microsoft.EntityFrameworkCore.Migrations;

namespace DBContext.Migrations
{
    public partial class FixAllOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Expense_ClinicId",
                table: "Expense",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_AppointmentId",
                table: "Attachment",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentTooth_AppointmentId",
                table: "AppointmentTooth",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_CategoryId",
                table: "Appointment",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_ClinicId",
                table: "Appointment",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_PatientId",
                table: "Appointment",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_UserId",
                table: "Appointment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_AppointmentCategory_CategoryId",
                table: "Appointment",
                column: "CategoryId",
                principalTable: "AppointmentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Clinic_ClinicId",
                table: "Appointment",
                column: "ClinicId",
                principalTable: "Clinic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                table: "Appointment",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_User_UserId",
                table: "Appointment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentTooth_Appointment_AppointmentId",
                table: "AppointmentTooth",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Appointment_AppointmentId",
                table: "Attachment",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Clinic_ClinicId",
                table: "Expense",
                column: "ClinicId",
                principalTable: "Clinic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_AppointmentCategory_CategoryId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Clinic_ClinicId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_User_UserId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentTooth_Appointment_AppointmentId",
                table: "AppointmentTooth");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_Appointment_AppointmentId",
                table: "Attachment");

            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Clinic_ClinicId",
                table: "Expense");

            migrationBuilder.DropIndex(
                name: "IX_Expense_ClinicId",
                table: "Expense");

            migrationBuilder.DropIndex(
                name: "IX_Attachment_AppointmentId",
                table: "Attachment");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentTooth_AppointmentId",
                table: "AppointmentTooth");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_CategoryId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_ClinicId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_PatientId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_UserId",
                table: "Appointment");
        }
    }
}
