using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.Helpers
{
    public class WebHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static ClaimsPrincipal CurrentUser => _httpContextAccessor?.HttpContext?.User;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static Guid UserId
        {
            get
            {

                var userId = _httpContextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type?.ToLower() == ClaimTypeHelper.UserId?.ToLower()).FirstOrDefault()?.Value ?? "";

                Guid.TryParse(userId, out Guid id);

                return id;
            }
        }
    }
}
