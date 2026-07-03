using EmployeeManagementAPI.Models.Data;
using EmployeeManagementAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAttendanceRepository : IRepository<Attendance>
{
    Task<IEnumerable<Attendance>> GetAttendanceByEmployeeAsync(int employeeId);
    Task<IEnumerable<Attendance>> GetAttendanceByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Attendance>> GetMonthlyAttendanceReportAsync(int month, int year);
}

