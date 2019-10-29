using System;
using System.Runtime.Serialization;

namespace EventManagement.ApplicationCore.Exceptions
{
    /// <summary>
    /// The exception is thrown when a requested ticket doesn't exist.
    /// </summary>
    public class TicketNotFoundException : EventManagementException
    {
        public TicketNotFoundException()
        {
        }

        public TicketNotFoundException(string message) : base(message)
        {
        }

        public TicketNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TicketNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
