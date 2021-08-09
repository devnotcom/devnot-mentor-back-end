using System.Text.Json.Serialization;

namespace DevnotMentor.Api.Common.Response
{
    /// <summary>
    /// Api response with data.
    /// </summary>
    /// <typeparam name="T">Response data</typeparam>
    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse()
        {

        }
        
        public ApiResponse(ResponseStatus status) : base(status)
        {
        }

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
        public ApiResponse(ResponseStatus status)
        {
            ResponseStatus = status;
            Success = ResponseStatus < ResponseStatus.BadRequest;
        }

        public ApiResponse(ResponseStatus status, string message)
        {
            ResponseStatus = status;
            Success = ResponseStatus < ResponseStatus.BadRequest;
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
        
        [JsonIgnore]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
