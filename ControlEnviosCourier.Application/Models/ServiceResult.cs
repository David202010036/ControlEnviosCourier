namespace ControlEnviosCourier.Application.Models
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int StatusCode { get; set; }

        public static ServiceResult<T> SuccessResponse(T data, int statusCode = 200, string message = "")
        {
            return new ServiceResult<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ServiceResult<T> ErrorResponse(string message, int statusCode = 400)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode
            };
        }
    }
}
