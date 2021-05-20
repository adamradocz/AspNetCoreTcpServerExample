using AspNetCoreTcpServerExample.ConnectionHandlers;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreTcpServerExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                    {
                        // TCP 8007
                        options.ListenLocalhost(8007, builder =>
                        {
                            builder.UseConnectionHandler<MyEchoConnectionHandler>();
                        });

                        //HTTP 5000
                        options.ListenLocalhost(5000);

                        // HTTPS 5001
                        options.ListenLocalhost(5001, builder =>
                        {
                            builder.UseHttps();
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
