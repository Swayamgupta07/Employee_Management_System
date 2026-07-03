using EmployeeManagementAPI.Models.Data;
using Microsoft.EntityFrameworkCore;

public static class UserSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@company.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.Parse("2025-01-01")
            },
            new User
            {
                Id = 2,
                Username = "hruser",
                Email = "hr@company.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("HrUser123!"),
                Role = "HR",
                IsActive = true,
                CreatedAt = DateTime.Parse("2025-01-01")
            },
            new User
            {
                Id = 3,
                Username = "employee1",
                Email = "employee1@company.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("EmpPass123!"),
                Role = "Employee",
                IsActive = true,
                CreatedAt = DateTime.Parse("2025-01-01")
            },
            new User
            {
                Id = 4,
                Username = "Swayam",
                Email = "swayamgupta09@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Swayam@123"),
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.Parse("2025-01-01")
            }
        );
    }
}
