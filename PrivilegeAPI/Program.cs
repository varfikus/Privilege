using Microsoft.EntityFrameworkCore;
using PrivilegeAPI.Context;

namespace PrivilegeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                //dbContext.Database.Migrate();
            }

            var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
            lifetime.ApplicationStarted.Register(() =>
            {
                Console.WriteLine("Server is running");
            });

            host.Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .UseUrls("http://0.0.0.0:5000")
                              .ConfigureLogging(logging =>
                              {
                                  logging.AddConsole();
                                  logging.SetMinimumLevel(LogLevel.Debug);
                              });
                });
    }
}
