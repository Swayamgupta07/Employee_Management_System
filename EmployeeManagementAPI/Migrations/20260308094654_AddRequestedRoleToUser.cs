using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestedRoleToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestedRole",
                table: "Users",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 8, 9, 46, 53, 152, DateTimeKind.Utc).AddTicks(223));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 8, 9, 46, 53, 152, DateTimeKind.Utc).AddTicks(228));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 8, 9, 46, 53, 152, DateTimeKind.Utc).AddTicks(229));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 8, 9, 46, 53, 152, DateTimeKind.Utc).AddTicks(229));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "RequestedRole" },
                values: new object[] { "$2a$11$EhOEedCtivkV.upuiTDVF.UkEaVbC6WRYchow7HevzjxW1h.iYtm2", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "RequestedRole" },
                values: new object[] { "$2a$11$HM.LdfNDZ8mo4Gfm03zMtumXSIJ8NuitnzW3gUcnXfssnZrqBwd3O", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PasswordHash", "RequestedRole" },
                values: new object[] { "$2a$11$h1GdSlRNwPZSxjNYtdfRSuYdBn.iIAnd0dSs4vS6qoTKIfarquyvO", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PasswordHash", "RequestedRole" },
                values: new object[] { "$2a$11$/y6XbemADqAofB8vMn.eOeRfX0QTzS8CwYUOjlMtetM93Kgk/7cJ.", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestedRole",
                table: "Users");

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
    }
}
