using EventManagement.Infrastructure.Data;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public DatabaseClientStore(EventsDbContext dbContext,
                                   ILogger<DatabaseClientStore> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            Guid clientGuid;
            if (!Guid.TryParse(clientId, out clientGuid))
            {
                _logger.LogWarning("The client id \"{clientId}\" is no valid GUID.", clientId);
                return null;
            }

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
                    AllowedScopes = { EventManagementConstants.AdminApi.ScopeName }
                };
            }
            return null;
        }
    }
}