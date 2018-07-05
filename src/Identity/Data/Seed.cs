using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hub256.Identity.Data
{
    static class Seed
    {
        static readonly List<Client> _clients = new List<Client>();
        static readonly List<ApiResource> _apiResources = new List<ApiResource>();
        static readonly List<IdentityResource> _identityResources = new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<Client> Clients => _clients;
        public static IEnumerable<ApiResource> ApiResources => _apiResources;
        public static IEnumerable<IdentityResource> IdentityResources => _identityResources;

        public static void AddSwaggerClient(string clientId, string clientDisplayName, string apiBaseUrl, string resource, string resourceDisplayName)
        {
            _apiResources.Add(new ApiResource(resource, resourceDisplayName));
            _clients.Add(new Client
            {
                ClientId = clientId,
                ClientName = clientDisplayName,
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = { $"{apiBaseUrl}/swagger/o2c.html" },
                PostLogoutRedirectUris = { $"{apiBaseUrl}/swagger/" },

                AllowedScopes =
                    {
                        resource
                    }
            });
        }

        static Seed()
        {
            //AddSwaggerClient();
        }
    }
}
