using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddOtpSecurity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FailedLoginAttempts",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutEnd",
                table: "Users",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtpCode",
                table: "Users",
                type: "varchar(6)",
                maxLength: 6,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpExpiry",
                table: "Users",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 8, 10, 40, 10, 19, DateTimeKind.Utc).AddTicks(8848));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 8, 10, 40, 10, 19, DateTimeKind.Utc).AddTicks(8853));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 8, 10, 40, 10, 19, DateTimeKind.Utc).AddTicks(8855));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 8, 10, 40, 10, 19, DateTimeKind.Utc).AddTicks(8856));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FailedLoginAttempts", "IsEmailVerified", "LockoutEnd", "OtpCode", "OtpExpiry", "PasswordHash" },
                values: new object[] { 0, false, null, null, null, "$2a$11$4Yj3PAHJZV4TQ6M7o9G86uv92qbm7wikt1YIZJgTG56UsAI.EU812" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FailedLoginAttempts", "IsEmailVerified", "LockoutEnd", "OtpCode", "OtpExpiry", "PasswordHash" },
                values: new object[] { 0, false, null, null, null, "$2a$11$2fsNDJfeVGp9SjzawjtdleMkzDOD0KQe7pQHvt4lAtt9aHsx9Ek5O" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FailedLoginAttempts", "IsEmailVerified", "LockoutEnd", "OtpCode", "OtpExpiry", "PasswordHash" },
                values: new object[] { 0, false, null, null, null, "$2a$11$ecr1wZGDEvnv.1OEjV3d6uya.rZNiBmL09MYyNs43diCmucMh/y/y" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FailedLoginAttempts", "IsEmailVerified", "LockoutEnd", "OtpCode", "OtpExpiry", "PasswordHash" },
                values: new object[] { 0, false, null, null, null, "$2a$11$sx.BSKPf3cvh6r41WF.jIOpPJLzK8jY.81VHhykkZDBfix.Kqq5mi" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailedLoginAttempts",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OtpCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OtpExpiry",
                table: "Users");

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
                column: "PasswordHash",
                value: "$2a$11$EhOEedCtivkV.upuiTDVF.UkEaVbC6WRYchow7HevzjxW1h.iYtm2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$HM.LdfNDZ8mo4Gfm03zMtumXSIJ8NuitnzW3gUcnXfssnZrqBwd3O");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$h1GdSlRNwPZSxjNYtdfRSuYdBn.iIAnd0dSs4vS6qoTKIfarquyvO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$/y6XbemADqAofB8vMn.eOeRfX0QTzS8CwYUOjlMtetM93Kgk/7cJ.");
        }
    }
}
