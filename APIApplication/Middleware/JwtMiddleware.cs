using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using APIApplication.Helpers;
using System.IdentityModel.Tokens.Jwt;
using APIApplication.Services;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;

namespace APIApplication.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;
        private readonly AppSettings _appSettings;
        public JwtMiddleware(RequestDelegate next, ILoggerFactory loggerfactory, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
            _logger = loggerfactory.CreateLogger<JwtMiddleware>();
        }

        public async Task Invoke(HttpContext context, IUserService userService) 
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachUserToContext(context, userService, token);

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters { 
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error occured : {ex}");
            }
        }
    }
}
