using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9952465f-eecf-47a0-a86b-511ddb2bf4cd", "5fc0d6e5-5320-4c20-9767-9318a80d0ea5", "User", "USER" },
                    { "c6ca4556-85fc-4ed5-b870-e87714a7d044", "d35d3f42-6db1-4565-9fd3-b0e563ed245f", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9952465f-eecf-47a0-a86b-511ddb2bf4cd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c6ca4556-85fc-4ed5-b870-e87714a7d044");
        }
    }
}
