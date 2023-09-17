using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.Exceptions
{
    public class RestException : Exception
    {
        public string ErrorMessage { get; set; }
        public HttpStatusCode Code { get; set; }
        public object Errors { get; set; }
        public RestException(HttpStatusCode code, string message, object errors =  null):base(message)
        {
            ErrorMessage = message;
            Code = code;
            Errors = errors;
        }
    }
}
