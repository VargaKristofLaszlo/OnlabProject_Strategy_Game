using Game.Shared.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models.Models;
using System.Threading;
using System.Threading.Tasks;

namespace BackEnd.Models.Models
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {

        public ApplicationDbContext(
           DbContextOptions options,
           IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Barrack> Barracks { get; set; }
        public DbSet<BuildingUpgradeCost> BuildingUpgradeCosts { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CityHall> CityHalls { get; set; }
        public DbSet<CityWall> CityWalls { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<ResourceProduction> ResourceProductions { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<MaxBuildingStage> MaxBuildingStages { get; set; }
        public DbSet<UnitsInCity> UnitsInCities { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<SypReport> SpyReports { get; set; }
        public DbSet<UpgradeQueueItem> UpgradeQueueItems { get; set; }
        public DbSet<UnitProductionQueueItem> UnitProductionQueueItems { get; set; }
        public DbSet<Castle> Castles { get; set; }
        public DbSet<Tavern> Taverns { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasMany(p => p.Cities)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<UnitsInCity>()
                .HasOne(pr => pr.Barrack)
                .WithMany(b => b.UnitsInCity)
                .HasForeignKey(pr => pr.BarrackId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UnitsInCity>()
                .HasOne(pr => pr.Unit)
                .WithMany(u => u.UnitsInCity)
                .HasForeignKey(pr => pr.UnitId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<City>()
                .HasOne(c => c.StoneProduction)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<City>()
                .HasOne(c => c.SilverProduction)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<City>()
                .HasOne(c => c.WoodProduction)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<City>()
                .HasOne(c => c.Castle)
                .WithOne(c => c.City)
                .HasForeignKey<Castle>(c => c.CityId);

            builder.Entity<City>()
                .HasOne(c => c.Tavern)
                .WithOne(c => c.City)
                .HasForeignKey<Tavern>(c => c.CityId);

            base.OnModelCreating(builder);
        }

    }
}
