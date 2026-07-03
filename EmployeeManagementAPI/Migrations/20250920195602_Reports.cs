using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class Reports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 56, 1, 948, DateTimeKind.Utc).AddTicks(1893));

            migrationBuilder.UpdateData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 56, 1, 948, DateTimeKind.Utc).AddTicks(1902));

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
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 56, 1, 463, DateTimeKind.Utc).AddTicks(7146));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 56, 1, 463, DateTimeKind.Utc).AddTicks(7147));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 56, 1, 948, DateTimeKind.Utc).AddTicks(1842));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 56, 1, 948, DateTimeKind.Utc).AddTicks(1845));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 7, 11, 354, DateTimeKind.Utc).AddTicks(2677));

            migrationBuilder.UpdateData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 7, 11, 354, DateTimeKind.Utc).AddTicks(2689));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 7, 10, 723, DateTimeKind.Utc).AddTicks(7693));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 7, 10, 723, DateTimeKind.Utc).AddTicks(7697));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 7, 10, 723, DateTimeKind.Utc).AddTicks(7699));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 7, 10, 723, DateTimeKind.Utc).AddTicks(7700));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 7, 11, 354, DateTimeKind.Utc).AddTicks(2604));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 19, 7, 11, 354, DateTimeKind.Utc).AddTicks(2609));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 20, 19, 7, 10, 723, DateTimeKind.Utc).AddTicks(7896), "$2a$11$wMGjaogBkBELTPal2JQleuiWX6anOtGFSJakrTCu82e7NoPfXTxeC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 20, 19, 7, 10, 976, DateTimeKind.Utc).AddTicks(4956), "$2a$11$mM5xjRsFv.vdv0U3t5sAkuE4WbS1/LskJ79c9xuQ7rfhROY97/zr." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 20, 19, 7, 11, 162, DateTimeKind.Utc).AddTicks(4496), "$2a$11$cbQcXBnMi2r63w3Cz1oIROqnjR2uAy5FA78yhNM9mEsVzgFNINo/6" });
        }
    }
}
