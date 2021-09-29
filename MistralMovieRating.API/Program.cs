using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MistralMovieRating.API.Helpers;
using Microsoft.Extensions.Hosting;


namespace MistralMovieRating.API
{
    class Program
    {
        private const string SeedArgs = "/seed";

        public static IConfiguration LoggerConfiguration { get; } = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("serilog.json", optional: true, reloadOnChange: true)
        .Build();

        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(LoggerConfiguration)
                .CreateLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();

                var seed = args.Any(x => x == SeedArgs);
                if (seed)
                {
                    args = args.Except(new[] { SeedArgs }).ToArray();
                }

                // Executes migrations if needed and seeds data if param is set to true
                await DbMigrationsHelpers.EnsureDatabasesMigrated(host, true);

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("serilog.json", optional: true, reloadOnChange: true);
                    configApp.AddJsonFile("seedData.json", optional: true, reloadOnChange: true);
                    configApp.AddEnvironmentVariables();
                    configApp.AddCommandLine(args);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options => options.AddServerHeader = false);
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((hostContext, loggerConfig) =>
                {
                    loggerConfig
                        .ReadFrom.Configuration(hostContext.Configuration)
                        .Enrich.WithProperty("ApplicationName", hostContext.HostingEnvironment.ApplicationName);
                });
    }
}
