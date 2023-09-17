using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.Helpers
{
    public class JwtConfigSettings
    {
        public string Secret { get; set; }
        public int TokenLifespan { get; set; }
        public string ValidIssuer { get; set; }
        public string Audience { get; set; }
    }
}
