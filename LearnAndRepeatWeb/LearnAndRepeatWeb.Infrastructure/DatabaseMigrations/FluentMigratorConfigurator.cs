using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LearnAndRepeatWeb.Infrastructure.DatabaseMigrations
{
    public class FluentMigratorConfigurator
    {
        private const string connectionString = "Server=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=LearnAndRepeatWeb;";

        public static void MigrateUp()
        {
            var serviceProvider = CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                runner.MigrateUp();
            }
        }

        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(migrationRunnerBuilder => migrationRunnerBuilder
                    .AddSqlServer()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(FluentMigratorConfigurator).Assembly).For.Migrations())
                .AddLogging(loggingBuilder => loggingBuilder.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
    }
}
