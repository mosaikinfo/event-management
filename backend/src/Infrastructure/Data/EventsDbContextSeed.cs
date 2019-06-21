using EventManagement.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace EventManagement.Infrastructure.Data
{
    public class EventsDbContextSeed
    {
        private readonly EventsDbContext _context;
        private readonly ILogger _logger;

        public EventsDbContextSeed(EventsDbContext context, ILogger<EventsDbContextSeed> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// If it's an empty database after a fresh setup we're
        /// inserting some initial data.
        /// </summary>
        public void Seed(ISeedData initialData)
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
}