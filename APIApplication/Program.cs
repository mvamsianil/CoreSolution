using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace APIApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
            .UseIISIntegration()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>();
                //.ConfigureWebHostDefaults(webBuilder =>
                //{
                //    webBuilder.UseStartup<Startup>();
                //    webBuilder.Start();
                //});
    }
}
