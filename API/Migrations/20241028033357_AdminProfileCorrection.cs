using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class AdminProfileCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Administrators",
                keyColumn: "Id",
                keyValue: 1,
                column: "Profie",
                value: "Admin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Administrators",
                keyColumn: "Id",
                keyValue: 1,
                column: "Profie",
                value: "Adm");
        }
    }
}
