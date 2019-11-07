using EventManagement.WebApp.Shared.Hangfire;

namespace Microsoft.Extensions.Logging
{
    public static class HangfireConsoleLoggingBuilderExtensions
    {
        public static ILoggingBuilder AddHangfireConsole(this ILoggingBuilder factory)
        {
            factory.AddProvider(new HangfireConsoleLoggerProvider());
            return factory;
        }
    }
}
