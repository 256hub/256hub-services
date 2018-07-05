using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Hub256.Common
{
    public interface ICommonStartup
    {       
        void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostingEnvironment env);
        void Configure(IApplicationBuilder app, IHostingEnvironment env);
    }
}
