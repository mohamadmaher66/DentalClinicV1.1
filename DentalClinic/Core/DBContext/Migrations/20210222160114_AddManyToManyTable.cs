using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDBContext.Migrations
{
    public partial class AddManyToManyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientMedicalHistory",
                columns: table => new
                {
                    PatientId = table.Column<int>(nullable: false),
                    MedicalHistoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientMedicalHistory", x => new { x.PatientId, x.MedicalHistoryId });
                    table.ForeignKey(
                        name: "FK_PatientMedicalHistory_MedicalHistory_MedicalHistoryId",
                        column: x => x.MedicalHistoryId,
                        principalTable: "MedicalHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientMedicalHistory_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedicalHistory_MedicalHistoryId",
                table: "PatientMedicalHistory",
                column: "MedicalHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientMedicalHistory");
        }
    }
}
