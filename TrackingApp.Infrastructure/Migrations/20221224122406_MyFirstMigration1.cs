using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackingApp.Infrastructure.Migrations
{
    public partial class MyFirstMigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    eventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    eventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    timeFrom = table.Column<int>(type: "int", nullable: false),
                    timeTo = table.Column<int>(type: "int", nullable: false),
                    isRepeated = table.Column<bool>(type: "bit", nullable: false),
                    repeatedEvery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startFrom = table.Column<int>(type: "int", nullable: false),
                    endsOn = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.eventId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "events");
        }
    }
}
