using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.Helpers
{
    public class AppConfig
    {
        public int PasswordHashIteration { get; set; }
        public string PepperKey { get; set; }
    }
}
