using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static void AddSwaggerClient(string clientId, string clientDisplayName, IEnumerable<(string Resource, string DisplayName)> resources)
        {
            foreach (var res in resources)
                _apiResources.Add(new ApiResource(res.Resource, res.DisplayName));

            _clients.Add(new Client
            {
                ClientId = clientId,
                ClientName = clientDisplayName,
                ClientSecrets = new[] { new Secret("swaggeruisecret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,

                //RedirectUris = { $"{apiBaseUrl}/swagger/o2c.html" },
                //PostLogoutRedirectUris = { $"{apiBaseUrl}/swagger/" },

                AllowedScopes = resources.Select(x => x.Resource).ToList()
            });
        }

        static Seed()
        {
            AddSwaggerClient("swaggerui", "Swagger UI client",
                new[] 
                {
                    ("checkin", "Checkin api scope")
                });
        }
    }
}
