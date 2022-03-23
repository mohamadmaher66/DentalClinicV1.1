using DentalClinicAPI.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DentalClinicAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.MigrateDatabase();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
