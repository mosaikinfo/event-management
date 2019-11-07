using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System;

namespace EventManagement.WebApp.Shared.Hangfire
{
    public class HangfireConsoleLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            PerformingContext context = JobContext.Current;
            if (context != null)
            {
                if (logLevel == LogLevel.Critical || logLevel == LogLevel.Error)
                {
                    context.SetTextColor(ConsoleTextColor.Red);
                }
                else if (logLevel == LogLevel.Warning)
                {
                    context.SetTextColor(ConsoleTextColor.Yellow);
                }
                else
                {
                    context.ResetTextColor();
                }
                string message = formatter(state, exception);
                context.WriteLine(message);
            }
        }

        private sealed class NullScope : IDisposable
        {
            public static NullScope Instance { get; } = new NullScope();

            private NullScope()
            {
            }

            /// <inheritdoc />
            public void Dispose()
            {
                // Nothing to do
            }
        }
    }
}
