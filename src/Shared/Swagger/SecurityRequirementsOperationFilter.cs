using Hub256.Common;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub256.Swagger
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        readonly ServiceInfo _serviceInfo;

        public SecurityRequirementsOperationFilter(ServiceInfo serviceInfo)
        {
            _serviceInfo = serviceInfo;
        }
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // Policy names map to scopes
            var hasAuthorization = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                        .Union(context.MethodInfo.GetCustomAttributes(true))
                                        .OfType<AuthorizeAttribute>().Any();
            
            if (hasAuthorization)
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                {
                    new Dictionary<string, IEnumerable<string>>
                    {
                        { "oauth2", new[] { _serviceInfo.ServiceName } }
                    }
                };
            }
        }
    }
}
