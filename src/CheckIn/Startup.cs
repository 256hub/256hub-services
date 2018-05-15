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
    public class Startup: ICommonStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public void ConfigureServices(IServiceCollection services, IConfiguration Configuration, IHostingEnvironment Environment)
        {
            var assembly = typeof(Startup).GetTypeInfo().Assembly;
            var part = new AssemblyPart(assembly);
            services.AddMvc()
                .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(part));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("<html><body><h1 style=\"color: green;\" >Hello from CheckIn Service</h1></body></html>");
            //});
        }       
    }
}
