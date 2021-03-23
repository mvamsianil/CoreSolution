using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIApplication.Middleware
{
    public class JwtTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtTokenMiddleware> _logger;
        public JwtTokenMiddleware(RequestDelegate next, ILoggerFactory loggerfactory)
        {
            _next = next;
            _logger = loggerfactory.CreateLogger<JwtTokenMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"Token Invoked");
            await this._next(context);
        }
    }
}
