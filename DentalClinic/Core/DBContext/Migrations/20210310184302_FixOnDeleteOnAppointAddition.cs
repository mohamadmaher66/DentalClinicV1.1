using Microsoft.EntityFrameworkCore.Migrations;

namespace DBContext.Migrations
{
    public partial class FixOnDeleteOnAppointAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentAppointmentAddition_AppointmentAddition_AppointmentAdditionId",
                table: "AppointmentAppointmentAddition");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentAppointmentAddition_Appointment_AppointmentId",
                table: "AppointmentAppointmentAddition");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentAppointmentAddition_AppointmentAddition_AppointmentAdditionId",
                table: "AppointmentAppointmentAddition",
                column: "AppointmentAdditionId",
                principalTable: "AppointmentAddition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentAppointmentAddition_Appointment_AppointmentId",
                table: "AppointmentAppointmentAddition",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentAppointmentAddition_AppointmentAddition_AppointmentAdditionId",
                table: "AppointmentAppointmentAddition");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentAppointmentAddition_Appointment_AppointmentId",
                table: "AppointmentAppointmentAddition");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentAppointmentAddition_AppointmentAddition_AppointmentAdditionId",
                table: "AppointmentAppointmentAddition",
                column: "AppointmentAdditionId",
                principalTable: "AppointmentAddition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentAppointmentAddition_Appointment_AppointmentId",
                table: "AppointmentAppointmentAddition",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
