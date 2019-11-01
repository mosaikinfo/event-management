using EventManagement.ApplicationCore.Models;
using System.Threading.Tasks;

namespace EventManagement.ApplicationCore.Auditing
{
    /// <summary>
    /// Event log to record audit events.
    /// </summary>
    public interface IAuditEventLog
    {
        /// <summary>
        /// Record an audit event.
        /// </summary>
        /// <param name="entry">Details about the audit event.</param>
        Task AddAsync(AuditEvent entry);
    }
}