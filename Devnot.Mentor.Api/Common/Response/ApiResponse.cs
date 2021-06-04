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

        public ApiResponse(bool success) : base(success)
        {
        }

        public ApiResponse(bool success, string message) : base(success, message)
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

        public ApiResponse(bool success)
        {
            Success = success;
        }

        public ApiResponse(bool success, string message)
        {
            Success = success;
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
    }
}
