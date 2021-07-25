using Microsoft.EntityFrameworkCore.Migrations;

namespace DBContext.Migrations
{
    public partial class FixOnDeleteOnPatientMedicalHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedicalHistory_MedicalHistory_MedicalHistoryId",
                table: "PatientMedicalHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedicalHistory_Patient_PatientId",
                table: "PatientMedicalHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedicalHistory_MedicalHistory_MedicalHistoryId",
                table: "PatientMedicalHistory",
                column: "MedicalHistoryId",
                principalTable: "MedicalHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedicalHistory_Patient_PatientId",
                table: "PatientMedicalHistory",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedicalHistory_MedicalHistory_MedicalHistoryId",
                table: "PatientMedicalHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedicalHistory_Patient_PatientId",
                table: "PatientMedicalHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedicalHistory_MedicalHistory_MedicalHistoryId",
                table: "PatientMedicalHistory",
                column: "MedicalHistoryId",
                principalTable: "MedicalHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedicalHistory_Patient_PatientId",
                table: "PatientMedicalHistory",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
