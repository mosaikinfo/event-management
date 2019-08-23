using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Threading.Tasks;

namespace EventManagement.Identity
{
    /// <summary>
    /// A <see cref="IClientStore"/> implementation that combines
    /// the configuration data from multiple client stores.
    /// </summary>
    public class HybridClientStore : IClientStore
    {
        private readonly IClientStore[] _innerStores;

        /// <summary>
        /// Initialize <see cref="HybridClientStore"/>.
        /// </summary>
        /// <param name="clientStores">
        /// The list of client stores that should be used to access the
        /// client configuration data.
        /// 
        /// The order is important.
        /// </param>
        public HybridClientStore(params IClientStore[] clientStores)
        {
            if (clientStores == null)
                throw new ArgumentNullException(nameof(clientStores));
            if (clientStores.Length == 0)
                throw new ArgumentException("The list may not be empty.");

            _innerStores = clientStores;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            Client client = null;
            foreach(IClientStore store in _innerStores)
            {
                client = await store.FindClientByIdAsync(clientId);

                if (client != null)
                    break;
            }
            return client;
        }
    }
}
