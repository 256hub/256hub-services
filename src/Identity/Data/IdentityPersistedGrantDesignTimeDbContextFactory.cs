using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hub256.Identity.Data
{
    class IdentityPersistedGrantDesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityPersistedGrantDbContext>
    {
        const string DEFAULT_CONNECTION_STRING = @"Data Source=(LocalDb)\MSSQLLocalDB;database=Hub256Identity;trusted_connection=yes;";
        public IdentityPersistedGrantDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<IdentityPersistedGrantDbContext>();
            builder.UseSqlServer(DEFAULT_CONNECTION_STRING);
            builder.EnableSensitiveDataLogging();
            return new IdentityPersistedGrantDbContext(builder.Options, new OperationalStoreOptions());
        }
    }
}
