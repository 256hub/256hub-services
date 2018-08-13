using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Hub256.Common.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

namespace Hub256.Common
{
    public abstract class CommonStartup : ICommonStartup
    {
        public IServiceProvider BranchServiceProvider { get; set; }

        public abstract ServiceInfo ServiceInfo { get; }

        public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostingEnvironment env)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var part = new AssemblyPart(assembly);

            services.AddSingleton(this.ServiceInfo);

            services.AddMvcCore()
                .AddVersionedApiExplorer(o =>
                {
                    o.GroupNameFormat = "'v'VVV";
                    o.DefaultApiVersion = new ApiVersion(1, 0);
                    o.SubstituteApiVersionInUrl = true;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddMvc(o=>
                {
                    if (this.ServiceInfo.UsesAuthentication
                    o.AddAuthorizationOptions();
                })
                .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(part))
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        //CamelCaseText = true
                    });
                    options.SerializerSettings.Converters.Add(new JsonNullDateTimeConverter(options.SerializerSettings.DateFormatString, "0000"));
                });

            if (this.ServiceInfo.UsesSwagger)
            {
                this.ConfigureSwaggerServices(services, configuration, env);
            }

            if (this.ServiceInfo.UsesAuthentication)
            {
                ConfigureAuthorizationServices(services, configuration, env);
            }

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (this.ServiceInfo.UsesSwagger)
            {
                ConfigureSwagger(app, env);
            }
            
            app.UseMvc();
        }

        public virtual void ConfigureSwaggerServices(IServiceCollection services, IConfiguration configuration, IHostingEnvironment env)
        {
            services.AddSwaggerDiscovery(configuration, this);
        }

        public virtual void ConfigureSwagger(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwaggerDiscovery();
        }

        public virtual void ConfigureAuthorizationServices(IServiceCollection services, IConfiguration configuration, IHostingEnvironment env)
        {
            services.ConfigureAuthorization(configuration, this.ServiceInfo);
        }

        public virtual void ConfigureAuthorization(IApplicationBuilder app, IHostingEnvironment env)
        {
        }
    }
}
