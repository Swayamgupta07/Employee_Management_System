using EmployeeManagementAPI.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;

public static class EmployeeSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1,
                FirstName = "Swayam",
                LastName = "Gupta",
                Email = "swayam.gupta@company.com",
                Phone = "123-456-7890",
                DepartmentId = 2, 
                Position = "Software Engineer",
                Salary = 60000m,
                IsActive = true,
                CreatedAt = DateTime.Parse("2025-01-01")
            },
            new Employee
            {
                Id = 2,
                FirstName = "Aniket",
                LastName = "Gupta",
                Email = "aniket.gupta@company.com",
                Phone = "098-765-4321",
                DepartmentId = 1, 
                Position = "HR Manager",
                Salary = 75000m,
                IsActive = true,
                CreatedAt = DateTime.Parse("2025-01-01")
            }
        );
    }
}
