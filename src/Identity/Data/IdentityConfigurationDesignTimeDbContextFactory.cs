using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hub256.Identity.Data
{
    class IdentityConfigurationDesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityConfigurationDbContext>
    {
        const string DEFAULT_CONNECTION_STRING = @"Data Source=(LocalDb)\MSSQLLocalDB;database=Hub256Identity;trusted_connection=yes;";
        public IdentityConfigurationDbContext CreateDbContext(string[] args)
        {            
            var builder = new DbContextOptionsBuilder<IdentityConfigurationDbContext>();
            builder.UseSqlServer(DEFAULT_CONNECTION_STRING);
            builder.EnableSensitiveDataLogging();
            return new IdentityConfigurationDbContext(builder.Options, new ConfigurationStoreOptions());
        }
    }
}
