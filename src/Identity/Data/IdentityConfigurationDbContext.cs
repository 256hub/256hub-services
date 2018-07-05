using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hub256.Identity.Data
{
    class IdentityConfigurationDbContext : ConfigurationDbContext<IdentityConfigurationDbContext>
    {
        const string TABLES_PREFIX = "id";
        public IdentityConfigurationDbContext(DbContextOptions<IdentityConfigurationDbContext> options, ConfigurationStoreOptions storeOptions) :
            base(options, storeOptions)
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
