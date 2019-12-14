using System;

namespace EventManagement.Identity
{
    /// <summary>
    /// Information about the context in which the current user is currently working.
    /// </summary>
    public class UserContext
    {
        /// <summary>
        /// Id of the current user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Id of the current event.
        /// </summary>
        public Guid? EventId { get; set; }
    }
}
