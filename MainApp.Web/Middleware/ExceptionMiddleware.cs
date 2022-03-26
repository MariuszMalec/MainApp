using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MainApp.Web.Middleware
{
    public class MyExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger = Log.ForContext<MyExceptionMiddleware>();

        public MyExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.Error($"Something went wrong: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(@$"Error {exception}");
        }
    }
}
