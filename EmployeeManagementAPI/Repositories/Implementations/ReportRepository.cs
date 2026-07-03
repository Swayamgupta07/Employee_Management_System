using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models.Data;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EmployeeManagementAPI.Repositories.Interfaces;
namespace EmployeeManagementAPI.Repositories.Implementations
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConverter _pdfConverter;

        public ReportRepository(ApplicationDbContext context, IConverter pdfConverter)
        {
            _context = context;
            _pdfConverter = pdfConverter;
        }

        public async Task<byte[]> GenerateEmployeeExcelReportStreamAsync(CancellationToken cancellationToken)
        {
            var employees = await _context.Employees.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Employees");

            // Headers
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "First Name";
            worksheet.Cell(1, 3).Value = "Last Name";
            worksheet.Cell(1, 4).Value = "Email";
            worksheet.Cell(1, 5).Value = "Phone";

            // Data
            int currentRow = 2;
            foreach (var emp in employees)
            {
                worksheet.Cell(currentRow, 1).Value = emp.Id;
                worksheet.Cell(currentRow, 2).Value = emp.FirstName;
                worksheet.Cell(currentRow, 3).Value = emp.LastName;
                worksheet.Cell(currentRow, 4).Value = emp.Email;
                worksheet.Cell(currentRow, 5).Value = emp.Phone;
                currentRow++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        public async Task<byte[]> GenerateEmployeePdfReportStreamAsync(CancellationToken cancellationToken)
        {
            var employees = await _context.Employees.ToListAsync();

            // Create simple HTML for employees
            var sb = new StringBuilder();
            sb.Append("<html><head><style>");
            sb.Append("table { width: 100%; border-collapse: collapse; }");
            sb.Append("th, td { border: 1px solid black; padding: 8px; text-align: left; }");
            sb.Append("</style></head><body>");
            sb.Append("<h1>Employee Report</h1>");
            sb.Append("<table>");
            sb.Append("<tr><th>Id</th><th>First Name</th><th>Last Name</th><th>Email</th></tr>");

            foreach (var e in employees)
            {
                sb.Append($"<tr><td>{e.Id}</td><td>{e.FirstName}</td><td>{e.LastName}</td><td>{e.Email}</td></tr>");
            }

            sb.Append("</table>");
            sb.Append("</body></html>");

            var globalSettings = new GlobalSettings
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait,
                Margins = new MarginSettings { Top = 10, Bottom = 10 }
            };

            var objectSettings = new ObjectSettings
            {
                HtmlContent = sb.ToString(),
                WebSettings = { DefaultEncoding = "utf-8" },
            };

            var pdfDoc = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return _pdfConverter.Convert(pdfDoc);
        }
    }
}
