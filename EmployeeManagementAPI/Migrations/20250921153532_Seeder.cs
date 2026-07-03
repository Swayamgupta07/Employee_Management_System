using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class Seeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 15, 35, 31, 324, DateTimeKind.Utc).AddTicks(7429));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 15, 35, 31, 324, DateTimeKind.Utc).AddTicks(7432));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 15, 35, 31, 324, DateTimeKind.Utc).AddTicks(7433));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 15, 35, 31, 324, DateTimeKind.Utc).AddTicks(7434));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$F3dKawY9/OwCI2K5oIPLZ.0AHUW05V3kcbqOrreq8fTanlsThVM7i");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$j2iXZlDLzTl2jE6.Y22cTuDb8MG9P7zNElEWjKAj2NeJ7dGLE9kca");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$d/R7RAMB8H7NwubqR8bd5esagRXwNojMkeDJbCTZrqwn0Jes79jW6");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "PasswordHash", "Role", "Username" },
                values: new object[] { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "swayamgupta09@gmail.com", true, "$2a$11$0KzVkrLfa7tjnICqViuraOqcEt5ctarLucnl/qkGDpFQmXjTW5MUK", "Admin", "Swayam" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 14, 37, 47, 818, DateTimeKind.Utc).AddTicks(1801));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 14, 37, 47, 818, DateTimeKind.Utc).AddTicks(1805));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 14, 37, 47, 818, DateTimeKind.Utc).AddTicks(1806));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 14, 37, 47, 818, DateTimeKind.Utc).AddTicks(1807));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$qeI7FASSNpGefQdN0.bfmOge4k7uOGDCxbbDcX5T4X61Vu6ghHeLq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$t7/NRPxYIhnj38Bg9T07YuiMt7lH66ALwzLtrc.h.XtBlQ0n7fLz6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$eDWIwZcs88NVGzrGICJlWO8RiCdGKISOQNyZuWZBqGGo4Kj1kBWzy");
        }
    }
}
