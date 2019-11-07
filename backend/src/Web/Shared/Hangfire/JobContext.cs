using Hangfire.Server;
using System;

namespace EventManagement.WebApp.Shared.Hangfire
{
    public class JobContext : IServerFilter
    {
        [ThreadStatic]
        private static string _jobId;

        public static string JobId
        {
            get { return _jobId; }
            set { _jobId = value; }
        }

        [ThreadStatic]
        private static PerformingContext _current;

        public static PerformingContext Current
        {
            get { return _current; }
            set { _current = value; }
        }

        public void OnPerforming(PerformingContext context)
        {
            Current = context;
            JobId = context.BackgroundJob.Id;
        }

        public void OnPerformed(PerformedContext filterContext)
        {
        }
    }
}
