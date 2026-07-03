using EmployeeManagementAPI.Models.Data;
using Microsoft.EntityFrameworkCore;

public static class DepartmentSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Human Resources", Description = "HR Department" },
            new Department { Id = 2, Name = "Information Technology", Description = "IT Department" },
            new Department { Id = 3, Name = "Development", Description = "Development Department" },
            new Department { Id = 4, Name = "Analyst", Description = "Analyst Department" }
        );
    }
}
