using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Identity
{
    /// <summary>
    /// This client store is for the scenario when you have
    /// clients that run under the same host as IdentityServer.
    ///
    /// It allows relative redirects uris (eg: '~/oidc-signin')
    /// and converts them to absolute uris.
    /// </summary>
    public class LocalClientStore : IClientStore
    {
        private readonly IClientStore _inner;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalClientStore(IClientStore innerClientStore, IHttpContextAccessor httpContextAccessor)
        {
            _inner = innerClientStore
                ?? throw new ArgumentNullException(nameof(innerClientStore));
            _httpContextAccessor = httpContextAccessor
                ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = await _inner.FindClientByIdAsync(clientId);
            if (client != null)
            {
                string baseUri = GetBaseUri();
                client.RedirectUris = client.RedirectUris
                    .Select(u => MakeAbsoluteUri(baseUri, u)).ToList();
                client.PostLogoutRedirectUris = client.PostLogoutRedirectUris
                    .Select(u => MakeAbsoluteUri(baseUri, u)).ToList();
            }
            return client;
        }

        public static string MakeAbsoluteUri(string baseUri, string relativeUri)
        {
            if (relativeUri.StartsWith("~/"))
            {
                relativeUri = relativeUri.Substring(2, relativeUri.Length - 2);
                if (!baseUri.EndsWith('/'))
                {
                    baseUri += '/';
                }
                return new Uri(new Uri(baseUri), relativeUri).AbsoluteUri;
            }
            return relativeUri;
        }

        private string GetBaseUri()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}{request.PathBase}";
        }
    }
}