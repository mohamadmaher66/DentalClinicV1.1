using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDBContext.Migrations
{
    public partial class AddAppointmentToothTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointmentTooth",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(nullable: false),
                    ToothNumber = table.Column<int>(nullable: false),
                    ToothPosition = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentTooth", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentTooth");
        }
    }
}
