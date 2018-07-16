using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hub256.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hub256.CheckIn
{
    public class Startup : CommonStartup
    {
        readonly ServiceInfo _serviceInfo;
        public override ServiceInfo ServiceInfo => _serviceInfo;

        public Startup()
        {
            _serviceInfo = new ServiceInfo("checkin", "Checkin service",
                "Checkin service is used to provide 256 hub members authentication and checkin process into hub");

            _serviceInfo.RequiredScopes.Add(("checkin", "Checkin api scope"));
        }       
    }
}
