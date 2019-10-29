using System;
using System.Runtime.Serialization;

namespace EventManagement.ApplicationCore.Exceptions
{
    /// <summary>
    /// This exception is thrown by the Event Management application.
    /// </summary>
    public class EventManagementException : Exception
    {
        public EventManagementException()
        {
        }

        public EventManagementException(string message) : base(message)
        {
        }

        public EventManagementException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EventManagementException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
