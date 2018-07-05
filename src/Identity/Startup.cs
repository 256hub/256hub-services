using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hub256.Common;
using Hub256.Identity.Data;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hub256.Identity
{
    public class Startup : ICommonStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostingEnvironment env)
        {
            var currentAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var connectionString = configuration.GetConnectionString("MainConnection");// @"Data Source=(LocalDb)\MSSQLLocalDB;database=Hub256Identity;trusted_connection=yes;";
            services.AddIdentityServer()
                .AddConfigurationStore<IdentityConfigurationDbContext>(o =>
                   {
                       o.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(currentAssembly));
                   })
                .AddOperationalStore<IdentityPersistedGrantDbContext>(options =>
                     {
                         options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(currentAssembly));
                         // this enables automatic token cleanup. this is optional.
                         options.EnableTokenCleanup = true;
                         options.TokenCleanupInterval = 30; // interval in seconds
                     });

            services.AddTransient<Seeder>();


            //IdentityServer4.EntityFramework.DbContexts.PersistedGrantDbContext
            // var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;


            // services.AddIdentityServer()
            //     .AddClientStore<ClientStore>()
            //     //.AddClientStore<>();
            //     //.AddClientStoreCache<>();
            //     //.AddCorsPolicyService<>();
            //     //.AddResourceStore<>

            //// IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext
            //     // this adds the config data from DB (clients, resources, CORS)
            //     .AddConfigurationStore(options =>
            //     {
            //         options.ConfigureDbContext = builder =>
            //             builder.UseSqlServer(connectionString,
            //                 sql => sql.MigrationsAssembly(migrationsAssembly));
            //     })
            //     .AddOperationalStore(options =>
            //     {
            //         options.ConfigureDbContext = builder =>
            //             builder.UseSqlServer(connectionString,
            //                 sql => sql.MigrationsAssembly(migrationsAssembly));

            //         // this enables automatic token cleanup. this is optional.
            //         options.EnableTokenCleanup = true;
            //         options.TokenCleanupInterval = 30; // interval in seconds
            //     });
            //.AddInMemoryClients(new[]
            //{
            //     new Client
            //     {
            //         ClientName="Test",
            //         ClientId="test"
            //     }
            //}).
            //AddInMemoryApiResources(new[]
            //{
            //     new ApiResource
            //     {
            //         DisplayName="TestRes",
            //         Name="TestRes",
            //         Scopes=new[]
            //         {
            //             new Scope
            //             {
            //                 Name="Scope"
            //             }
            //         }
            //     }
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var seeder = app.ApplicationServices.GetRequiredService<Seeder>();
            seeder.EnsureCreatedAndSeeded();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           

            app.UseIdentityServer();
        }
    }
}
