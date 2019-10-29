using EventManagement.Infrastructure.Data;
using EventManagement.WebApp.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace EventManagement.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                SetupDatabase(scope.ServiceProvider);
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    // Enables the "Log stream" feature of Azure App Service.
                    logging.AddAzureWebAppDiagnostics();

                    // Enables logging to the Hangfire Dashboard Console.
                    logging.AddHangfireConsole();
                });

        private static void SetupDatabase(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            var env = services.GetRequiredService<IHostingEnvironment>();
            try
            {
                var dbContext = services.GetRequiredService<EventsDbContext>();
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred migrating the database schema.");
            }
            if (env.IsDevelopment() || env.IsStaging())
            {
                try
                {
                    var dbSeed = services.GetRequiredService<EventsDbContextSeed>();
                    dbSeed.Seed(new TestData());
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the database.");
                }
            }
        }
    }
}