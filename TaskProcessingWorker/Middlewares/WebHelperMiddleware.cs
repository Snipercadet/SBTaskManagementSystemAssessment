using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SBTaskManagement.Application.Helpers;

namespace SBTaskManagementSystemAssessment.Middlewares
{
    public class WebHelperMiddleware
    {
        private readonly RequestDelegate _next;

        public WebHelperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IHttpContextAccessor httpContextAccessor)
        {
            // Configure the WebHelper service with IHttpContextAccessor
            WebHelper.Configure(httpContextAccessor);

            // Continue the request pipeline
            await _next(context);
        }
       
    }

    public static class WebHelperMiddlewareExtension
    {
        public static IApplicationBuilder UseWebHelper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebHelperMiddleware>();
        }
    }
}
