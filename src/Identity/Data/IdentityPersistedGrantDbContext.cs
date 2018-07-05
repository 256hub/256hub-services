using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hub256.Identity.Data
{
    class IdentityPersistedGrantDbContext : PersistedGrantDbContext<IdentityPersistedGrantDbContext>
    {
        const string TABLES_PREFIX = "id";
        public IdentityPersistedGrantDbContext(DbContextOptions<IdentityPersistedGrantDbContext> options, OperationalStoreOptions storeOptions) 
            : base(options, storeOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var rel = entity.Relational();
                rel.TableName = $"{TABLES_PREFIX}_{rel.TableName}";
            }
        }
    }
}
