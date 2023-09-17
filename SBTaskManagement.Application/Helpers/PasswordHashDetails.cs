using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.Helpers
{
    public class PasswordHashDetails
    {
        public string Salt { get; set; }
        public string HashedValue { get; set; }
    }
}
