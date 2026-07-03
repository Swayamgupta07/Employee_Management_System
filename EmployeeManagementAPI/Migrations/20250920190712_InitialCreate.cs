using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Position = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CheckInTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    CheckOutTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    WorkingHours = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 20, 19, 7, 10, 723, DateTimeKind.Utc).AddTicks(7693), "HR Department", "Human Resources" },
                    { 2, new DateTime(2025, 9, 20, 19, 7, 10, 723, DateTimeKind.Utc).AddTicks(7697), "IT Department", "Information Technology" },
                    { 3, new DateTime(2025, 9, 20, 19, 7, 10, 723, DateTimeKind.Utc).AddTicks(7699), "Finance Department", "Finance" },
                    { 4, new DateTime(2025, 9, 20, 19, 7, 10, 723, DateTimeKind.Utc).AddTicks(7700), "Marketing Department", "Marketing" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 20, 19, 7, 10, 723, DateTimeKind.Utc).AddTicks(7896), "admin@company.com", true, "$2a$11$wMGjaogBkBELTPal2JQleuiWX6anOtGFSJakrTCu82e7NoPfXTxeC", "Admin", "admin" },
                    { 2, new DateTime(2025, 9, 20, 19, 7, 10, 976, DateTimeKind.Utc).AddTicks(4956), "hr@company.com", true, "$2a$11$mM5xjRsFv.vdv0U3t5sAkuE4WbS1/LskJ79c9xuQ7rfhROY97/zr.", "HR", "hruser" },
                    { 3, new DateTime(2025, 9, 20, 19, 7, 11, 162, DateTimeKind.Utc).AddTicks(4496), "employee1@company.com", true, "$2a$11$cbQcXBnMi2r63w3Cz1oIROqnjR2uAy5FA78yhNM9mEsVzgFNINo/6", "Employee", "employee1" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "Email", "FirstName", "HireDate", "IsActive", "LastName", "Phone", "Position", "Salary", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 20, 19, 7, 11, 354, DateTimeKind.Utc).AddTicks(2604), 2, "john.doe@company.com", "John", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Doe", "123-456-7890", "Software Engineer", 60000.00m, null },
                    { 2, new DateTime(2025, 9, 20, 19, 7, 11, 354, DateTimeKind.Utc).AddTicks(2609), 1, "jane.smith@company.com", "Jane", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Smith", "098-765-4321", "HR Manager", 75000.00m, null }
                });

            migrationBuilder.InsertData(
                table: "Attendances",
                columns: new[] { "Id", "CheckInTime", "CheckOutTime", "CreatedAt", "Date", "EmployeeId", "Notes", "Status", "WorkingHours" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2025, 9, 20, 19, 7, 11, 354, DateTimeKind.Utc).AddTicks(2677), new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1, "On time", "Present", null },
                    { 2, null, null, new DateTime(2025, 9, 20, 19, 7, 11, 354, DateTimeKind.Utc).AddTicks(2689), new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc), 2, "On time", "Present", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EmployeeId_Date",
                table: "Attendances",
                columns: new[] { "EmployeeId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
