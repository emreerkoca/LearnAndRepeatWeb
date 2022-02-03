using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LearnAndRepeatWeb.Infrastructure.DatabaseMigrations
{
    public class FluentMigratorConfigurator
    {
        public static void MigrateUp(string connectionString)
        {
            var serviceProvider = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(migrationRunnerBuilder => migrationRunnerBuilder
                    .AddSqlServer()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(FluentMigratorConfigurator).Assembly).For.Migrations())
                .AddLogging(loggingBuilder => loggingBuilder.AddFluentMigratorConsole())
                .BuildServiceProvider(false);

            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                runner.MigrateUp();
            }
        }
    }
}
