namespace EventManagement.ApplicationCore.Interfaces
{
    public interface ITicketNumberService
    {
        /// <summary>
        /// Generate a random human-readable ticket number.
        /// </summary>
        /// <param name="evt">Event for the ticket.</param>
        string GenerateTicketNumber(Models.Event evt);
    }
}