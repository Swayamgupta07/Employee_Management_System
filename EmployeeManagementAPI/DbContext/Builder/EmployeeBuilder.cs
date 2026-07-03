using EmployeeManagementAPI.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagementAPI.Dbcontext.Builder
{
    public class EmployeeBuilder : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasIndex(e => e.Email).IsUnique();
            builder.Property(e => e.Salary).HasColumnType("decimal(18,2)");
        }
    }
}
