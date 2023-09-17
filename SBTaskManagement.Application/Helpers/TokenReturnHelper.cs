using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.Helpers
{
    public class TokenReturnHelper
    {
        public string AccessToken { get; set; }
        public DateTimeOffset ExpiresIn { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
    }
}
