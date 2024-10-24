﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using MainApp.BLL.Exceptions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http.Extensions;

namespace MainApp.Web.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (ForbidException forbidException)
            {
                context.Response.StatusCode = 403;
            }
            catch (BadRequestException badRequestException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(badRequestException.Message);
            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;              
                await context.Response.WriteAsync($"{notFoundException.Message} {context.Response.StatusCode}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                //co otrzyma klient
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync($"Sth went wrong! {e.Message}");
            }
        }
    }
}
