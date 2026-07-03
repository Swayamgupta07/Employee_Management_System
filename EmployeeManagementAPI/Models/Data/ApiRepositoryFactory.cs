namespace EmployeeManagementAPI.Models.Data
{
    public static class ApiResponseFactory
    {
        public static ApiResponse<T> Success<T>(T data, string message = "")
            => new() { Success = true, Message = message, Data = data };

        public static ApiResponse<string> Fail(string message, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
            => new() { Success = false, Message = message };

        public static ApiResponse<string> Fail(string message)
            => new() { Success = false, Message = message };

        public static ApiResponse<string> SuccessResponse(string data, string message = "")
        {
            return new ApiResponse<string>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }
        public static ApiResponse<string> FailResponse(string message)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }

    }
}
