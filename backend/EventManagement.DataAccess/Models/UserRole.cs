namespace EventManagement.DataAccess.Models
{
    public enum UserRole
    {
        /// <summary>
        /// Administrators can create new events and manage permissions.
        /// </summary>
        Admin,

        /// <summary>
        /// Event managers can manage tickets for an event
        /// and manage permissions for pre-sellers.
        /// </summary>
        EventManager,

        /// <summary>
        /// Pre-sellers can report ticket sales.
        /// </summary>
        Preseller
    }
}
