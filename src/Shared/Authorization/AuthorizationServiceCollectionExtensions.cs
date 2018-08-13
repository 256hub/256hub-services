using Hub256.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthorizationServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration, ServiceInfo serviceinfo)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var identityUrl = configuration.GetEndpointOptions().IdentityUrl;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                //options.RequireHttpsMetadata = false;
                options.Audience = serviceinfo.ServiceName;
                //options.Events = new JwtBearerEvents
                //{
                //    OnAuthenticationFailed = c =>
                //       {
                //           return Task.CompletedTask;
                //       },
                //    OnChallenge=c=>
                //    {
                //        return Task.CompletedTask;
                //    }
                //};
            });

            return services;
        }
    }
}
