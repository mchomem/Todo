namespace Todo.API.Responses;

/// <summary>
/// Represents a standard response returned by an API operation, including the result data, a success indicator, and an
/// optional message.
/// </summary>
/// <remarks>Use this class to encapsulate the outcome of an API call, providing both the result and contextual
/// information such as success status and messages. This pattern helps standardize API responses and simplifies error
/// handling and client-side processing.</remarks>
/// <typeparam name="T">The type of the data returned by the API operation. Must be a reference type.</typeparam>
public class ApiResponse<T> where T : class
{
    /// <summary>
    /// Default constructor for ApiResponse.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="message"></param>
    /// <param name="success"></param>
    public ApiResponse(T data, string message = "Operation successfully completed..", bool success = true)
    {
        Data = data;
        Message = message;
        Success = success;
    }

    /// <summary>
    /// A generic property that holds the data returned by the API operation.
    /// </summary>
    public T Data { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the operation completed successfully or not.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// A message providing additional information about the operation's outcome.
    /// </summary>
    public string Message { get; set; }
}
