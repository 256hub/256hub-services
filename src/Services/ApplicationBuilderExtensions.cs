using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hub256.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static TStartup MapIsolated<TStartup>(this IApplicationBuilder app, PathString path, Startup rootStartup) where TStartup: Hub256.Common.ICommonStartup, new()
        {
            var startup = new TStartup();
            var branchName = path.ToString();
            var brScope = rootStartup.RootContainer.BeginLifetimeScope(branchName, b =>
            {
                var services = new ServiceCollection();
                startup.ConfigureServices(services, rootStartup.Configuration, rootStartup.Environment);
                services.AddSingleton<WrappedHttpContextAccessor>();
                services.AddSingleton<IHttpContextAccessor>(sp => sp.GetRequiredService<WrappedHttpContextAccessor>());
                b.Populate(services, branchName);
            });

            var serviceProvider = new AutofacServiceProvider(brScope);
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            startup.BranchServiceProvider = serviceProvider;

            app.Map(path, branch =>
            {
                var wrappedAccessor = serviceProvider.GetRequiredService<WrappedHttpContextAccessor>();
                wrappedAccessor.Wrap(branch.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

                branch.ApplicationServices = serviceProvider;
                branch.Use(async (ctx, next) =>
                {
                    using (var scope = scopeFactory.CreateScope())
                    {
                        ctx.RequestServices = scope.ServiceProvider;

                        // Call the next delegate/middleware in the pipeline
                        await next();
                    }
                });
                startup.Configure(branch, rootStartup.Environment);
            });

            rootStartup.KnownServices.Add(path, startup.ServiceInfo);
            return startup;
        }

        class WrappedHttpContextAccessor : IHttpContextAccessor
        {
            IHttpContextAccessor _internalAccessor;
            public HttpContext HttpContext { get => _internalAccessor.HttpContext; set => _internalAccessor.HttpContext = value; }

            public void Wrap(IHttpContextAccessor contextAccessor)
            {
                _internalAccessor = contextAccessor;
            }
        }
    }
}
