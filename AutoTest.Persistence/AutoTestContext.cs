using System;

namespace AutoTest.Persistence
{
    using AutoTest.Domain.StorageModels;
    using AutoTest.Persistence.Setup;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    public class AutoTestContext : DbContext
    {
        public AutoTestContext(DbContextOptions<AutoTestContext> options)
            : base(options)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Club>? Clubs { get; private set; }
        public DbSet<Event>? Events { get; private set; }
        public DbSet<Entrant>? Entrants { get; private set; }
        public DbSet<TestRun>? TestRuns { get; private set; }
        public DbSet<Test>? Tests { get; private set; }
        public DbSet<User>? Users { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupAdminEmails.Setup(modelBuilder.Entity<AdminEmail>());
            SetupClub.Setup(modelBuilder.Entity<Club>());
            SetupEntrant.Setup(modelBuilder.Entity<Entrant>());
            SetupEvent.Setup(modelBuilder.Entity<Event>());
            SetupTest.Setup(modelBuilder.Entity<Test>());
            SetupTestRun.Setup(modelBuilder.Entity<TestRun>());
            SetupTestRun.Setup(modelBuilder.Entity<Penalty>());
            SetupUser.Setup(modelBuilder.Entity<User>());
        }

        public void SeedDatabase()
        {
            if (this.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                this.Database.Migrate();
                if (this.Clubs != null && !this.Clubs.Any())
                {
                    this.Clubs.Add(new Club(1, "BRMC", "brmc@paypal.com", "https://www.bognor-regis-mc.co.uk"));
                }
                if (this.Events != null && !this.Events.Any())
                {
                    this.Events.Add(new Event(1, 1, "Kestrel Farm", new DateTime(2000, 1, 1)));
                }
                if (this.Tests != null && !this.Tests.Any())
                {
                    this.Tests.Add(new Test(1, 1, 1, null));
                }
                if (this.Entrants != null && !this.Entrants.Any())
                {
                    this.Entrants.Add(new Entrant(1, "Matt", "McCorry", "AA05AAA", "A", 1, true));
                }

                this.SaveChanges();
            }
        }
    }
}
