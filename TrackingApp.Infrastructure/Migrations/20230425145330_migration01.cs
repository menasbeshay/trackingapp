using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackingApp.Infrastructure.Migrations
{
    public partial class migration01 : Migration
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
                    endsOn = table.Column<long>(type: "bigint", nullable: false),
                    CreatedByNameId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
