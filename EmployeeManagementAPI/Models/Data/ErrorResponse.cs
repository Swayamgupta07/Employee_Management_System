namespace EmployeeManagementAPI.Models.Data
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string ErrorType { get; set; }
        public string Details { get; set; }
        public string InnerException { get; set; }
        public DateTime Timestamp { get; set; }
        public string Path { get; set; }
        public string TraceId { get; set; }  
    }
}
