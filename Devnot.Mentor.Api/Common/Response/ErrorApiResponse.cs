namespace DevnotMentor.Api.Common.Response
{
    public class ErrorApiResponse<T> : ApiResponse<T>
    {
        public ErrorApiResponse(T data) : base(success: false)
        {
            Data = data;
        }

        public ErrorApiResponse(T data, string message) : base(success: false, message: message)
        {
            Data = data;
        }
    }

    /// <summary>
    /// You can use it when method worked wrong.
    /// </summary>
    public class ErrorApiResponse : ApiResponse
    {
        public ErrorApiResponse() : base(success: false)
        {
        }

        public ErrorApiResponse(string message) : base(success: false, message: message)
        {
        }
    }
}