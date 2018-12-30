using IdentityServer4.Models;
using System.Collections.Generic;

namespace EventManagement.WebApp
{
    public class IdentityServerConfig
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
            return new List<ApiResource>
            {
                new ApiResource("eventmanagement.admin", "Event Management Admin API")
            };
        }

        public static IEnumerable<Client> GetClients()
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
                        "http://localhost:5000/auth-callback",
                        "http://localhost:5000/silent-refresh.html"
                    },
                    PostLogoutRedirectUris = { "http://localhost:5000/" },

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "eventmanagement.admin" }
                }
            };
        }
    }
}