using EventManagement.ApplicationCore.Models;
using System.Collections.Generic;

namespace EventManagement.ApplicationCore.Interfaces
{
    /// <summary>
    /// Initial data for seeding the event management data store.
    /// </summary>
    public interface ISeedData
    {
        IList<User> Users { get; }
    }
}