using System;
using System.Runtime.Serialization;

namespace EventManagement.Infrastructure.Data
{
    public class DbQueryException : Exception
    {
        public DbQueryException()
        {
        }

        public DbQueryException(string message) : base(message)
        {
        }

        public DbQueryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DbQueryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}