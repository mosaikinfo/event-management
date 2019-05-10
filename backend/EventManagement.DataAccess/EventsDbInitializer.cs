using EventManagement.DataAccess.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventManagement.DataAccess
{
    public class EventsDbInitializer
    {
        private readonly EventsDbContext _context;
        private readonly ILogger _logger;

        public EventsDbInitializer(EventsDbContext context, ILogger<EventsDbInitializer> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// If it's an empty database after a fresh setup we're
        /// inserting some initial data.
        /// </summary>
        public void EnsureInitialData(IEventsDbInitialData initialData)
        {
            if (initialData.Users != null && !_context.Users.Any())
            {
                _logger.LogInformation("Data seeding initial Users.");
                foreach (var user in initialData.Users)
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                }
            }
        }
    }

    public interface IEventsDbInitialData
    {
        IList<User> Users { get; }
    }
}