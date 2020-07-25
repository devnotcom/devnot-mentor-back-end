using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Devnot.Mentor.Api.Common
{
    public class ApiResponse<T>: ApiResponse
    {
        public T Data { get; set; }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
