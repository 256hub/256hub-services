using Hub256.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    public static class MvcOptionsExtensions
    {
        public static void AddAuthorizationOptions(this MvcOptions options, ServiceInfo serviceInfo)
        {
            var policyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser();

            foreach (var (Scope, DisplayName) in serviceInfo.RequiredScopes)
                policyBuilder = policyBuilder.RequireClaim("scope", Scope);

            var requireLoyaltyServicesScope = policyBuilder.Build();
            options.Filters.Add(new AuthorizeFilter(requireLoyaltyServicesScope));
        }
    }
}
