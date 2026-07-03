using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSwayam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 14, 31, 45, 491, DateTimeKind.Utc).AddTicks(7031));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 14, 31, 45, 491, DateTimeKind.Utc).AddTicks(7036));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 14, 31, 45, 491, DateTimeKind.Utc).AddTicks(7037));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 14, 31, 45, 491, DateTimeKind.Utc).AddTicks(7038));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$xU2Zffui1tzKqCi8au3OWOkXsH7UZ2Mj8/uauf5mTU2ZiBNo1Vh9e");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$.WP3Fgrzr5lJKZU5r2z7mOv9l7E2viU9nibipcP90dZ0OhTYpylEK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$v4mVB9nX9ahExc4WzJfGZeVxf9BBr3/cuDPcEJAQXjzbhLJqXTRa2");
        }
    }
}
