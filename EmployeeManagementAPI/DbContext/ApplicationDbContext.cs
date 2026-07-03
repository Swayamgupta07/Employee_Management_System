using EmployeeManagementAPI.Dbcontext.Builder;
using EmployeeManagementAPI.Models.Data;
using EmployeeManagementAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EmployeeManagementAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
        {
            public ApplicationDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                var connectionString = "Server=localhost;Database=EmployeeManagementDB;User=appuser;Password=appuserpassword;CharSet=utf8mb4;";
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                return new ApplicationDbContext(optionsBuilder.Options);
            }
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmployeeBuilder());
            modelBuilder.ApplyConfiguration(new UserBuilder());
            modelBuilder.ApplyConfiguration(new AttendanceBuilder());
            modelBuilder.ApplyConfiguration(new DepartmentBuilder());

            DepartmentSeeder.Seed(modelBuilder);
            UserSeeder.Seed(modelBuilder);
            EmployeeSeeder.Seed(modelBuilder);
            AttendanceSeeder.Seed(modelBuilder);
        }
    }
}
