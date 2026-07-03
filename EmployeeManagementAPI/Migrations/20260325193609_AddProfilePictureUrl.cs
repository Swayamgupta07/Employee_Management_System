using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddProfilePictureUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 19, 36, 6, 942, DateTimeKind.Utc).AddTicks(940));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 19, 36, 6, 942, DateTimeKind.Utc).AddTicks(949));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 19, 36, 6, 942, DateTimeKind.Utc).AddTicks(951));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 19, 36, 6, 942, DateTimeKind.Utc).AddTicks(952));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "ProfilePictureUrl" },
                values: new object[] { "$2a$11$3ILVdroQyJT0O8zqekBLw.sVYGArB1QZWBP6ejlpmeB47a2LJzyW2", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "ProfilePictureUrl" },
                values: new object[] { "$2a$11$r4d.9SrSA4GmZJrSUQVcueyLORrEfA.39X/BNhFmJOz/tDD2aoj72", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PasswordHash", "ProfilePictureUrl" },
                values: new object[] { "$2a$11$OEhi/cprrPcw62shFRrcMO7gbr19BaoudpZiT94Zf81Tp.d8.FBg6", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PasswordHash", "ProfilePictureUrl" },
                values: new object[] { "$2a$11$kCHDjA0dVCldg4lR55tWmOpuyHs/pS.p7YvFPZbdN6gEebhWl9Zqe", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "Users");

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
                column: "PasswordHash",
                value: "$2a$11$4Yj3PAHJZV4TQ6M7o9G86uv92qbm7wikt1YIZJgTG56UsAI.EU812");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$2fsNDJfeVGp9SjzawjtdleMkzDOD0KQe7pQHvt4lAtt9aHsx9Ek5O");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$ecr1wZGDEvnv.1OEjV3d6uya.rZNiBmL09MYyNs43diCmucMh/y/y");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$sx.BSKPf3cvh6r41WF.jIOpPJLzK8jY.81VHhykkZDBfix.Kqq5mi");
        }
    }
}
