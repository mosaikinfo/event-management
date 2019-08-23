using EventManagement.Identity;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EventManagement.WebApp
{
    internal class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            var requiredClaims = new[] { "role" };

            return new List<ApiResource>
            {
                new ApiResource("eventmanagement.admin", "Event Management Admin API", requiredClaims)
            };
        }

        /// <summary>
        /// Returns a list of local clients that run under
        /// the same host as the identity provider.
        /// </summary>
        public static IEnumerable<Client> GetLocalClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "admin-app",
                    ClientName = "Admin App",
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris = {
                        "~/auth-callback",
                        "~/silent-refresh.html"
                    },
                    PostLogoutRedirectUris = { "~" },

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "eventmanagement.admin" }
                }
            };
        }
    }

    /// <summary>
    /// Client store to provide the static local api clients (eg: Admin App).
    /// </summary>
    internal class EventManagementLocalClientStore : LocalClientStore
    {
        public EventManagementLocalClientStore(IHttpContextAccessor httpContextAccessor)
            : base(new InMemoryClientStore(IdentityServerConfig.GetLocalClients()), httpContextAccessor)
        {
        }
    }

    /// <summary>
    /// Client store to provide the static local clients and 
    /// api clients that are stored in the database.
    /// </summary>
    internal class EventManagementClientStore: HybridClientStore
    {
        public EventManagementClientStore(EventManagementLocalClientStore localClientStore,
                                          IEventManagementClientStore apiClientStore) 
            : base(localClientStore, apiClientStore)
        {

        }
    }
}