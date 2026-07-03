using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class Final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 9, 21, 1, 14, 3, 28, DateTimeKind.Utc).AddTicks(8770), new DateTime(2025, 9, 21, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 9, 21, 1, 14, 3, 28, DateTimeKind.Utc).AddTicks(8781), new DateTime(2025, 9, 21, 0, 0, 0, 0, DateTimeKind.Utc) });

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
                columns: new[] { "CreatedAt", "Description", "Name" },
                values: new object[] { new DateTime(2025, 9, 21, 1, 14, 2, 617, DateTimeKind.Utc).AddTicks(6471), "Development Department", "Development" });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description", "Name" },
                values: new object[] { new DateTime(2025, 9, 21, 1, 14, 2, 617, DateTimeKind.Utc).AddTicks(6472), "Analyst Department", "Analyst" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Email", "FirstName", "LastName" },
                values: new object[] { new DateTime(2025, 9, 21, 1, 14, 3, 28, DateTimeKind.Utc).AddTicks(8722), "swayam.gupta@company.com", "Swayam", "Gupta" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Email", "FirstName", "LastName" },
                values: new object[] { new DateTime(2025, 9, 21, 1, 14, 3, 28, DateTimeKind.Utc).AddTicks(8725), "aniket.gupta@techno.com", "Aniket", "Gupta" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 9, 20, 19, 56, 1, 948, DateTimeKind.Utc).AddTicks(1893), new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 9, 20, 19, 56, 1, 948, DateTimeKind.Utc).AddTicks(1902), new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 56, 1, 463, DateTimeKind.Utc).AddTicks(7142));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 56, 1, 463, DateTimeKind.Utc).AddTicks(7145));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description", "Name" },
                values: new object[] { new DateTime(2025, 9, 20, 19, 56, 1, 463, DateTimeKind.Utc).AddTicks(7146), "Finance Department", "Finance" });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description", "Name" },
                values: new object[] { new DateTime(2025, 9, 20, 19, 56, 1, 463, DateTimeKind.Utc).AddTicks(7147), "Marketing Department", "Marketing" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Email", "FirstName", "LastName" },
                values: new object[] { new DateTime(2025, 9, 20, 19, 56, 1, 948, DateTimeKind.Utc).AddTicks(1842), "john.doe@company.com", "John", "Doe" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Email", "FirstName", "LastName" },
                values: new object[] { new DateTime(2025, 9, 20, 19, 56, 1, 948, DateTimeKind.Utc).AddTicks(1845), "jane.smith@company.com", "Jane", "Smith" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 20, 19, 56, 1, 463, DateTimeKind.Utc).AddTicks(7283), "$2a$11$5bha7qwgleihq3op.c7yhuEN0hzNwgrIewLpovVs7hSKPhwjVvK/O" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 20, 19, 56, 1, 612, DateTimeKind.Utc).AddTicks(7185), "$2a$11$.ylqWx5ene8mKkK8G.dIXu3.UTHrVloPOW4BAiqrkNbZulEMbru9W" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 20, 19, 56, 1, 781, DateTimeKind.Utc).AddTicks(287), "$2a$11$bZvDlMQAdya0wTQCQlBLh.dnz./cwBTruACnmEHVGfMMGkNBieKVe" });
        }
    }
}
