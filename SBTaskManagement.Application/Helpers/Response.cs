using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.Helpers
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class SuccessResponse<T> : Response
    {
        public SuccessResponse()
        {
            Success = true;
        }
        public T Data { get; set;}
    }

    public class ErrorResponse<T> : Response
    {
        public T Error { get; set; }
    }
}
