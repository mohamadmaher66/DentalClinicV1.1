using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDBContext.Migrations
{
    public partial class FixAllManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointmentAppointmentAddition",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(nullable: false),
                    AppointmentAdditionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentAppointmentAddition", x => new { x.AppointmentId, x.AppointmentAdditionId });
                    table.ForeignKey(
                        name: "FK_AppointmentAppointmentAddition_AppointmentAddition_AppointmentAdditionId",
                        column: x => x.AppointmentAdditionId,
                        principalTable: "AppointmentAddition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentAppointmentAddition_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentAppointmentAddition_AppointmentAdditionId",
                table: "AppointmentAppointmentAddition",
                column: "AppointmentAdditionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentAppointmentAddition");
        }
    }
}
