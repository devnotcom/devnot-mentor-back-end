namespace DevnotMentor.Api.Common.Response
{
    public class SuccessApiResponse<T> : ApiResponse<T>
    {
        public SuccessApiResponse(T data) : base(success: true)
        {
            Data = data;
        }

        public SuccessApiResponse(T data, string message) : base(success: true, message: message)
        {
            Data = data;
        }
    }

    /// <summary>
    /// You can use it when method worked successfully.
    /// </summary>
    public class SuccessApiResponse : ApiResponse
    {
        public SuccessApiResponse() : base(success: true)
        {
        }

        public SuccessApiResponse(string message) : base(success: true, message: message)
        {
        }
    }
}