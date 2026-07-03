namespace EmployeeManagementAPI.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<byte[]> GenerateEmployeeExcelReportStreamAsync(CancellationToken cancellationToken);
        Task<byte[]> GenerateEmployeePdfReportStreamAsync(CancellationToken cancellationToken);
    }
}
