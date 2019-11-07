using Microsoft.Extensions.Logging;

namespace EventManagement.WebApp.Shared.Hangfire
{
    public class HangfireConsoleLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new HangfireConsoleLogger();
        }

        public void Dispose()
        {
        }
    }
}
