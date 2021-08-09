namespace DevnotMentor.Api.Common.Response
{
    public class SuccessApiResponse<T> : ApiResponse<T>
    {
        public SuccessApiResponse(ResponseStatus status, T data) : base(status)
        {
            ResponseStatus = status;
            Data = data;
        }

        public SuccessApiResponse(ResponseStatus status, T data, string message) : base(status, message: message)
        {
            Data = data;
        }

        public SuccessApiResponse(T data, string message) : base(ResponseStatus.Ok, message)
        {
        }

        public SuccessApiResponse(T data) : base(ResponseStatus.Ok)
        {
        }
    }

    /// <summary>
    /// You can use it when method worked successfully.
    /// </summary>
    public class SuccessApiResponse : ApiResponse
    {

        public SuccessApiResponse(ResponseStatus status) : base(status)
        {
        }

        public SuccessApiResponse(ResponseStatus status, string message) : base(status, message: message)
        {
        }

        public SuccessApiResponse(string message) : base(ResponseStatus.Ok, message)
        {
        }
    }
}