using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsPro.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToIncident : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Incidents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Open");

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 1,
                column: "Status",
                value: "Open");

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 2,
                column: "Status",
                value: "Open");

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 3,
                column: "Status",
                value: "Open");

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 4,
                column: "Status",
                value: "Open");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Incidents");
        }
    }
}
