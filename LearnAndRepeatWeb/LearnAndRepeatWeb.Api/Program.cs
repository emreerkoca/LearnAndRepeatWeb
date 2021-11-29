using LearnAndRepeatWeb.Infrastructure.DatabaseMigrations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LearnAndRepeatWeb.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FluentMigratorConfigurator.MigrateUp();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
