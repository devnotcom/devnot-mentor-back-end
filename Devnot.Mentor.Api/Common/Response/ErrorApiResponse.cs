namespace DevnotMentor.Api.Common.Response
{
    public class ErrorApiResponse<T> : ApiResponse<T>
    {
        public ErrorApiResponse(ResponseStatus status, T data) : base(status)
        {
            ResponseStatus = status;
            Data = data;
        }

        public ErrorApiResponse(ResponseStatus status, T data, string message) : base(status, message: message)
        {
            Data = data;
        }

        public ErrorApiResponse(T data, string message) : base(ResponseStatus.BadRequest, message)
        {
        }
    }

    /// <summary>
    /// You can use it when method worked wrong.
    /// </summary>
    public class ErrorApiResponse : ApiResponse
    {
        public ErrorApiResponse(ResponseStatus status) : base(status)
        {
        }

        public ErrorApiResponse(ResponseStatus status, string message) : base(status, message: message)
        {
        }

        public ErrorApiResponse(string message) : base(ResponseStatus.BadRequest, message)
        {
        }
    }
}