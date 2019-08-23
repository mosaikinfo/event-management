using EventManagement.Infrastructure.Data;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Identity
{
    /// <summary>
    /// Client store for clients in the Event Managemement database.
    /// </summary>
    public class DatabaseClientStore : IEventManagementClientStore
    {
        private readonly EventsDbContext _dbContext;

        public DatabaseClientStore(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var clientGuid = new Guid(clientId);

            var client = await _dbContext.Clients
                .AsNoTracking()
                .Where(c => c.Id == clientGuid)
                .SingleOrDefaultAsync();

            if (client != null)
            {
                return new Client
                {
                    ClientId = client.Id.ToString(),
                    ClientName = client.Name,
                    ClientSecrets = { new Secret(client.Secret) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "eventmanagement.admin" }
                };
            }
            return null;
        }
    }
}
