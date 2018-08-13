using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hub256.Identity.Data
{
    class Seeder
    {
        readonly IdentityConfigurationDbContext _configurationDb;
        readonly IdentityPersistedGrantDbContext _persistedGrantDb;
        readonly ILogger<Seeder> _logger;

        public Seeder(IdentityConfigurationDbContext configurationDb, IdentityPersistedGrantDbContext persistedGrantDb, ILogger<Seeder> logger)
        {
            _configurationDb = configurationDb;
            _persistedGrantDb = persistedGrantDb;
            _logger = logger;
        }

        public void EnsureCreatedAndSeeded()
        {
            EnsureCreatedAndMigrated().GetAwaiter().GetResult();
            EnsureSeedData().GetAwaiter().GetResult();            
        }

        async Task EnsureCreatedAndMigrated()
        {
            await _configurationDb.Database.MigrateAsync();
            await _persistedGrantDb.Database.MigrateAsync();
        }

        async Task EnsureSeedData()
        {
            _logger.LogInformation("Seeding database...");

            var seededClientsIds = await _configurationDb.Clients.Select(x => x.ClientId).ToArrayAsync();
            var notSeededClients = Seed.Clients.Where(x => !seededClientsIds.Contains(x.ClientId)).ToList();

            if (notSeededClients.Any())
            {
                _logger.LogInformation("Not seeded clients being populated");
                foreach (var client in notSeededClients.Select(x => x.ToEntity()))
                {
                    _configurationDb.Clients.Add(client);
                }
                await _configurationDb.SaveChangesAsync();
            }
            else
            {
                _logger.LogInformation("Clients already populated");
            }
            
            var seededIdResources = await _configurationDb.IdentityResources.Select(x => x.Name).ToArrayAsync();
            var notSeededIdResources = Seed.IdentityResources.Where(x => !seededIdResources.Contains(x.Name)).ToList();

            if (seededIdResources.Any())
            {
                _logger.LogInformation("Not seeded identityResources being populated");
                foreach (var resource in notSeededIdResources.Select(x => x.ToEntity()))
                {
                    _configurationDb.IdentityResources.Add(resource);
                }
                await _configurationDb.SaveChangesAsync();
            }
            else
            {
                _logger.LogInformation("IdentityResources already populated.");
            }

            var seededApiResources = await _configurationDb.ApiResources.Select(x => x.Name).ToArrayAsync();
            var notSeededApiResources = Seed.ApiResources.Where(x => !seededApiResources.Contains(x.Name)).ToList();

            if (notSeededApiResources.Any())
            {
                _logger.LogInformation("Not seeded apiResources being populated.");
                foreach (var resource in notSeededApiResources.Select(x => x.ToEntity()))
                {
                    _configurationDb.ApiResources.Add(resource);
                }
                await _configurationDb.SaveChangesAsync();
            }
            else
            {
                _logger.LogInformation("ApiResources already populated");
            }

            _logger.LogInformation("Done seeding database.");
        }

        //public async Task SeedData()
        //{
        //    // Attaching identity resources
        //    foreach (var idRes in Seed.IdentityResources)
        //    {
        //        var e = await _db.IdentityResources.AsNoTracking().FirstOrDefaultAsync(x => x.Name == idRes.Name);
        //        if (e == null)
        //        {
        //            e = idRes.ToEntity();
        //            _db.IdentityResources.Add(e);
        //        }
        //        else
        //        {
        //            var @new = idRes.ToEntity();
        //            SyncExistingIdentityResource(@new, e );
        //        }
        //    }

        //    // Attaching api resources
        //    foreach (var apiRes in Seed.ApiResources)
        //    {
        //        var e = await _db.ApiResources.AsNoTracking().FirstOrDefaultAsync(x => x.Name == apiRes.Name);
        //        if (e == null)
        //        {
        //            e = apiRes.ToEntity();
        //            _db.ApiResources.Add(e);
        //        }
        //        else
        //        {
        //            var @new = apiRes.ToEntity();
        //            SyncExistingApiResource(@new, e);
        //        }
        //    }

        //    // Attaching clients
        //    foreach (var client in Seed.Clients)
        //    {
        //        var e = await _db.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.ClientId == client.ClientId);
        //        if (e == null)
        //        {
        //            e = client.ToEntity();
        //            _db.Clients.Add(e);
        //        }
        //        else
        //        {
        //            var @new = client.ToEntity();
        //            SyncExistingClient(@new, e);
        //        }
        //    }
        //}

        //void SyncExistingIdentityResource(IdentityResource @new, IdentityResource old)
        //{           
        //    @new.Id = old.Id;
        //    foreach (var newClaim in @new.UserClaims)
        //    {
        //        var oldClaim = old.UserClaims.FirstOrDefault(x => x.Type == newClaim.Type);
        //        newClaim.Id = oldClaim.Id;
        //        newClaim.IdentityResource = @new;
        //        _db.Attach(newClaim).State = EntityState.Modified;
        //    }
        //    _db.Attach(@new).State = EntityState.Modified;
        //}

        //void SyncExistingApiResource(ApiResource @new, ApiResource old)
        //{
        //    @new.Id = old.Id;
        //    foreach (var newClaim in @new.UserClaims)
        //    {
        //        var oldClaim = old.UserClaims.FirstOrDefault(x => x.Type == newClaim.Type);
        //        if (oldClaim != null)
        //        {
        //            newClaim.Id = oldClaim.Id;
        //            _db.Attach(newClaim).State = EntityState.Modified;
        //        }
        //        newClaim.ApiResource = @new;
        //    }


        //    foreach (var newScope in @new.Scopes)
        //    {
        //        var oldScope = old.Scopes.FirstOrDefault(x => x.Name == newScope.Name);
        //        if (oldScope != null)
        //        {
        //            newScope.Id = oldScope.Id;
        //            foreach (var newApiClaim in newScope.UserClaims)
        //            {
        //                var oldApiClaim = oldScope.UserClaims.FirstOrDefault(x => x.Type == newApiClaim.Type);
        //                if (oldApiClaim != null)
        //                {
        //                    newApiClaim.Id = oldApiClaim.Id;
        //                    _db.Attach(newApiClaim).State = EntityState.Modified;
        //                }
        //                newApiClaim.ApiScope = newScope;
        //            }
        //            _db.Attach(newScope).State = EntityState.Modified;
        //        }
        //        newScope.ApiResource = @new;
        //    }
        //    _db.Attach(@new).State = EntityState.Modified;
        //}

        //void SyncExistingClient(Client @new, Client old)
        //{
        //    @new.Id = old.Id;



        //    foreach (var newScope in @new.AllowedScopes)
        //    {
        //        var oldScope = old.AllowedScopes.FirstOrDefault(x => x.Scope == newScope.Scope);
        //        if (oldScope != null)
        //        {
        //            newScope.Id = oldScope.Id;
        //            _db.Attach(newScope).State = EntityState.Modified;
        //        }
        //        newScope.Client = @new;
        //    }

        //    foreach (var newRestrict in @new.IdentityProviderRestrictions)
        //    {
        //        var oldRestrict = old.IdentityProviderRestrictions.FirstOrDefault(x => x.Provider == newRestrict.Provider);
        //        if (oldRestrict != null)
        //        {
        //            newRestrict.Id = oldRestrict.Id;
        //            _db.Attach(newRestrict).State = EntityState.Modified;
        //        }
        //        newRestrict.Client = @new;
        //    }

        //    foreach (var newClaim in @new.Claims)
        //    {
        //        var oldClaim = old.Claims.FirstOrDefault(x => x.Type == newClaim.Type);
        //        if (oldClaim != null)
        //        {
        //            newClaim.Id = oldClaim.Id;
        //            _db.Attach(newClaim).State = EntityState.Modified;
        //        }
        //        newClaim.Client = @new;
        //    }


        //    _db.Attach(@new).State = EntityState.Modified;
        //}
    }
}
