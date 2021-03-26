using APIApplication.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using System.IO;
using APIApplication.Services;
using APIApplication.Helpers;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

namespace APIApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSignInKey(),
                ValidateIssuer = true,
                ValidIssuer = GetIssuer(),
                ValidateAudience = true,
                ValidAudience = GetAudience(),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddCors();
            services.AddControllers();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddScoped<IUserService, UserService>();
            services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.Audience = "https://localhost:44397";
                    o.Authority = "https://localhost:44397";
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = tokenValidationParameters;
                    o.Configuration = new OpenIdConnectConfiguration();
                });
            IdentityModelEventSource.ShowPII = true;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error");

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseMiddleware<JwtMiddleware>();
            app.UseLogUrl();
            app.UseEndpoints(x => x.MapControllers());
            
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "logs", "")),
                RequestPath = "/Logs"
            });
        }

        static private SymmetricSecurityKey GetSignInKey()
        {
            const string secretKey = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            return signingKey;
        }

        static private string GetIssuer()
        {
            return "https://localhost:44397";
        }

        static private string GetAudience()
        {
            return "https://localhost:44397";
        }
    }
}
