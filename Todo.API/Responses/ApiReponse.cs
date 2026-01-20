namespace Todo.API.Responses;

public class ApiResponse<T> where T : class
{
    public ApiResponse(T data, string message = "Operation successfully completed..", bool success = true)
    {
        Data = data;
        Message = message;
        Success = success;
    }

    public T Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
}
