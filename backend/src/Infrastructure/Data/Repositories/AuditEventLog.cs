using EventManagement.ApplicationCore.Auditing;
using EventManagement.ApplicationCore.Models;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Data.Repositories
{
    public class AuditEventLog : IAuditEventLog
    {
        private readonly EventsDbContext _context;

        public AuditEventLog(EventsDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(AuditEvent entry)
        {
            _context.AuditEventLog.Add(entry);
            return _context.SaveChangesAsync();
        }
    }
}