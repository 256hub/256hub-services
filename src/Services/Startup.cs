using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hub256.Services
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            // Configuration = configuration;
            var builder = new ConfigurationBuilder()
                //.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName.ToLowerInvariant()}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public ILoggerFactory LoggerFactory { get; }
        public IContainer RootContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
          

            services.AddLogging();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            RootContainer = containerBuilder.Build();

            return new AutofacServiceProvider(RootContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug();
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            }

            app.MapIsolated<CheckIn.Startup>("/checkin", this);
            app.MapIsolated<Identity.Startup>("/identity", this);
        }

        public class MyMiddleware
        {
            private readonly RequestDelegate _next;

            public MyMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext httpContext, ITestService svc)
            {
                httpContext.Response.StatusCode = 200;
                await httpContext.Response.WriteAsync(svc.GetHelloValue());
            }
        }

        public interface ITestService
        {
            string GetHelloValue();
        }

        class Test1TestService : ITestService
        {
            public string GetHelloValue()
            {
                return "Hello test 1";
            }
        }

        class Test2TestService : ITestService
        {
            public string GetHelloValue()
            {
                return "Hello test 2";
            }
        }
    }
}
