using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackingApp.Infrastructure.Migrations
{
    public partial class updateEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "startFrom",
                table: "events",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByNameId",
                table: "events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "events",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByNameId",
                table: "events");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "events");

            migrationBuilder.AlterColumn<long>(
                name: "startFrom",
                table: "events",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
