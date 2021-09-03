namespace DevnotMentor.Common.API
{
    public class ErrorApiResponse<T> : ApiResponse<T>
    {
        public ErrorApiResponse(ResponseStatus status, T data, string message) : base(status, message: message)
        {
            Data = data;
        }

        public ErrorApiResponse(T data, string message) : base(ResponseStatus.BadRequest, message)
        {
            Data = data;
        }
    }

    /// <summary>
    /// You can use it when method worked wrong.
    /// </summary>
    public class ErrorApiResponse : ApiResponse
    {
        public ErrorApiResponse(ResponseStatus status, string message) : base(status, message: message)
        {
        }

        public ErrorApiResponse(string message) : base(ResponseStatus.BadRequest, message)
        {
        }
    }
}