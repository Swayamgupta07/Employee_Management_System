using EmployeeManagementAPI.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AttendanceBuilder: IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.HasIndex(a => new { a.EmployeeId, a.Date }).IsUnique();

    }
}
