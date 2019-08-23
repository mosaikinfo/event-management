using IdentityServer4.Stores;

namespace EventManagement.Identity
{
    /// <summary>
    /// Persistend client store for clients of the Event Management API.
    /// </summary>
    public interface IEventManagementClientStore : IClientStore
    {
    }
}
