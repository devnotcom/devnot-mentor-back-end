using System.Text.Json.Serialization;

namespace DevnotMentor.Common.API
{
    /// <summary>
    /// Api response with data.
    /// </summary>
    /// <typeparam name="T">Response data</typeparam>
    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse(ResponseStatus status, string message) : base(status, message)
        {
        }

        /// <summary>
        /// Response data.
        /// </summary>
        public T Data { get; set; }
    }


    /// <summary>
    /// Api response.
    /// </summary>
    public class ApiResponse
    {
        public ApiResponse()
        {
        }

        public ApiResponse(ResponseStatus status, string message)
        {
            StatusCode = status;
            Success = StatusCode < ResponseStatus.BadRequest;
            Message = message;
        }

        /// <summary>
        /// You can check method worked successfully?
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Result message. It provides error message when success is false. Otherwise success message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// HTTP response status code
        /// </summary>
        [JsonIgnore]
        public ResponseStatus StatusCode { get; set; }
    }
}
