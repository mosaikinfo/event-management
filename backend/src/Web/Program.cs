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
                .UseStartup<Startup>();

        private static void SetupDatabase(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                var dbContext = services.GetRequiredService<EventsDbContext>();
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred migrating the DB.");
            }
            try
            {
                var dbSeed = services.GetRequiredService<EventsDbContextSeed>();
                dbSeed.Seed(new TestData());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }
    }
}