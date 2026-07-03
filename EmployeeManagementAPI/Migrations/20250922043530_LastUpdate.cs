using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class LastUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 22, 4, 35, 29, 712, DateTimeKind.Utc).AddTicks(70));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 22, 4, 35, 29, 712, DateTimeKind.Utc).AddTicks(73));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 22, 4, 35, 29, 712, DateTimeKind.Utc).AddTicks(74));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 22, 4, 35, 29, 712, DateTimeKind.Utc).AddTicks(74));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$z0vEo7gyO0ce2qVZoB2A8.CL8hSJKEpO1uUZY.tpNgwEouUpV26ze");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$8cF6fRp5tWr97PgPveuyu.cBYxdiI9hrWFCUk1MdPl1sQbgypvWFi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$s0Uqe2WVFBNQbrR6cXnvNunk1vFbl08k8jz/uke8Ib/3cVHIon4Gu");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$5okhnByp4Kcb0BW.gNLAsOGMAKZDSlkytbf9HeuBFZIbE/.BxsQ0C");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$0KzVkrLfa7tjnICqViuraOqcEt5ctarLucnl/qkGDpFQmXjTW5MUK");
        }
    }
}
