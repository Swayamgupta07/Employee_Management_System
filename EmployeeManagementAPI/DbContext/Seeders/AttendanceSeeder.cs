using EmployeeManagementAPI.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;

public static class AttendanceSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>().HasData(
            new Attendance
            {
                Id = 1,
                EmployeeId = 1, 
                Date = DateTime.Parse("2025-09-21"),
                Status = "Present",
                Notes = "On time",
                CreatedAt = DateTime.Parse("2025-09-21"),
                UpdatedAt = DateTime.Parse("2025-09-21")
            },
            new Attendance
            {
                Id = 2,
                EmployeeId = 2, 
                Date = DateTime.Parse("2025-09-21"),
                Status = "Present",
                Notes = "On time",
                CreatedAt = DateTime.Parse("2025-09-21"),
                UpdatedAt = DateTime.Parse("2025-09-21")
            }
        );
    }
}
