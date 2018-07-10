﻿using System;
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
    public class CommonStartup : ICommonStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostingEnvironment env)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var part = new AssemblyPart(assembly);

            services.AddMvcCore()
                .AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV")
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddMvc()
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
            services.AddApiVersioning(o => o.ReportApiVersions = true);

            this.ConfigureSwaggerServices(services, configuration, env);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            ConfigureSwagger(app, env);
            app.UseMvc();
        }

        public virtual void ConfigureSwaggerServices(IServiceCollection services, IConfiguration configuration, IHostingEnvironment env)
        {
            services.AddSwaggerDiscovery(configuration);
            
        }

        public virtual void ConfigureSwagger(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwaggerDiscovery();
        }       
    }
}
