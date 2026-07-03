using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Attendances",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) });

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
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Email" },
                values: new object[] { new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "aniket.gupta@company.com" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$xU2Zffui1tzKqCi8au3OWOkXsH7UZ2Mj8/uauf5mTU2ZiBNo1Vh9e" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$.WP3Fgrzr5lJKZU5r2z7mOv9l7E2viU9nibipcP90dZ0OhTYpylEK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$v4mVB9nX9ahExc4WzJfGZeVxf9BBr3/cuDPcEJAQXjzbhLJqXTRa2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Attendances");

            migrationBuilder.UpdateData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 1, 14, 3, 28, DateTimeKind.Utc).AddTicks(8770));

            migrationBuilder.UpdateData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 1, 14, 3, 28, DateTimeKind.Utc).AddTicks(8781));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 1, 14, 2, 617, DateTimeKind.Utc).AddTicks(6467));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 1, 14, 2, 617, DateTimeKind.Utc).AddTicks(6470));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 1, 14, 2, 617, DateTimeKind.Utc).AddTicks(6471));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 1, 14, 2, 617, DateTimeKind.Utc).AddTicks(6472));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 1, 14, 3, 28, DateTimeKind.Utc).AddTicks(8722));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Email" },
                values: new object[] { new DateTime(2025, 9, 21, 1, 14, 3, 28, DateTimeKind.Utc).AddTicks(8725), "aniket.gupta@techno.com" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 21, 1, 14, 2, 617, DateTimeKind.Utc).AddTicks(6593), "$2a$11$ICARGj0bNbtUy5g5qEXceOmjI3PposXlvAfUPvpHG4ljbl6wBKTA." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 21, 1, 14, 2, 753, DateTimeKind.Utc).AddTicks(5437), "$2a$11$GFMMmB5ZLfKGcMCihzUVa.fglos3q3waxItYglZ4XQdw991v2Rv8." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 21, 1, 14, 2, 891, DateTimeKind.Utc).AddTicks(5936), "$2a$11$2OlptlY808Kl4YPeg3JYSuA2nKBP.eMpILIR8ScZ9pxvAsniSenim" });
        }
    }
}
