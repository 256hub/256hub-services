using System;
using System.Collections.Generic;
using System.Text;

namespace Hub256.Common
{
    public class ServiceInfo
    {
        HashSet<(string Scope, string DisplayName)> _requiredScopes;
        HashSet<string> _apiVersions;


        public string ServiceName { get; }

        public string ServiceDisplayName { get; }

        public string ServiceDescription { get; }

        public bool UsesAuthentication { get; }

        public bool UsesSwagger { get; }


        public ICollection<(string Scope, string DisplayName)> RequiredScopes => _requiredScopes = _requiredScopes ?? new HashSet<(string Scope, string DisplayName)>();

        public ICollection<string> ApiVersions => _apiVersions = _apiVersions ?? new HashSet<string>();

        public ServiceInfo(string serviceName, 
                           string serviceDisplayName, 
                           string serviceDescription = null, 
                           bool usesAuthentication = true,
                           bool usesSwagger = true)
        {
            this.ServiceName = serviceName;
            this.ServiceDisplayName = serviceDisplayName;
            this.ServiceDescription = serviceDescription;
            this.UsesAuthentication = usesAuthentication;
            this.UsesSwagger = usesSwagger;
        }
    }
}
