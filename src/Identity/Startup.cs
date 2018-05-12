using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hub256.Common;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hub256.Identity
{
    public class Startup:ICommonStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostingEnvironment env)
        {
            services.AddIdentityServer()
               .AddInMemoryClients(new[]
               {
                    new Client
                    {
                        ClientName="Test",
                        ClientId="test"
                    }
               }).
               AddInMemoryApiResources(new[]
               {
                    new ApiResource
                    {
                        DisplayName="TestRes",
                        Name="TestRes",
                        Scopes=new[]
                        {
                            new Scope
                            {
                                Name="Scope"
                            }
                        }
                    }
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("<html><body><h1 style=\"color: red;\" >Hello world from Identity.</h1></body></html>");
            });
        }        
    }
}
