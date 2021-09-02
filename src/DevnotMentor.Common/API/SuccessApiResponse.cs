namespace DevnotMentor.Common.API
{
    public class SuccessApiResponse<T> : ApiResponse<T>
    {
        public SuccessApiResponse(ResponseStatus status, T data, string message) : base(status, message)
        {
            Data = data;
        }

        public SuccessApiResponse(ResponseStatus status, T data) : base(status, ResultMessage.Success)
        {
            Data = data;
        }

        public SuccessApiResponse(T data) : base(ResponseStatus.Ok, ResultMessage.Success)
        {
            Data = data;
        }
    }

    /// <summary>
    /// You can use it when method worked successfully.
    /// </summary>
    public class SuccessApiResponse : ApiResponse
    {
        public SuccessApiResponse(ResponseStatus status, string message) : base(status, message)
        {
        }

        public SuccessApiResponse(ResponseStatus status) : base(status, ResultMessage.Success)
        {
        }

        public SuccessApiResponse() : base(ResponseStatus.Ok, ResultMessage.Success)
        {
        }
    }
}