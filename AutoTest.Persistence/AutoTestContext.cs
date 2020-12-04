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
            this.ChangeTracker!.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Club>? Clubs { get; private set; }
        public DbSet<Event>? Events { get; private set; }
        public DbSet<Entrant>? Entrants { get; private set; }
        public DbSet<TestRun>? TestRuns { get; private set; }
        public DbSet<Profile>? Users { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupClub.Setup(modelBuilder.Entity<Club>());
            SetupEntrant.Setup(modelBuilder.Entity<Entrant>());
            SetupEvent.Setup(modelBuilder.Entity<Event>());
            SetupTestRun.Setup(modelBuilder.Entity<TestRun>());
            SetupProfile.Setup(modelBuilder.Entity<Profile>());
        }

        public void SeedDatabase()
        {
            if (this.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                this.Database.EnsureCreated();
                if (this.Clubs != null && this.Clubs.SingleOrDefault(a => a.ClubId == 1) == null)
                {
                    var brmc = new Club(1, "BRMC", "brmc@paypal.com", "https://www.bognor-regis-mc.co.uk");
                    brmc.SetAdminEmails(new[] { new AuthorisationEmail("mccorry@gmail.com") });
                    this.Clubs.Add(brmc);
                }
                if (this.Events != null && this.Events.SingleOrDefault(a => a.EventId == 1) == null)
                {
                    var e = new Event(1, 1, "Kestrel Farm", new DateTime(2000, 1, 1), 10, 2, string.Empty, EventType.AutoTest);
                    e.SetTests(new[] { new Test(1, "") });
                    this.Events.Add(e);
                }
                if (this.Entrants != null && this.Entrants.SingleOrDefault(a => a.EntrantId == 1) == null)
                {
                    var e = new Entrant(1, 1, "Matt", "McCorry", "A", 1, true, "BRMC", "msa");
                    e.SetVehicle(new Vehicle("Vauxhall", "Corsa", 2005, 1229, "AA05AAA"));
                    this.Entrants.Add(e);
                }

                this.SaveChanges();
            }
        }
    }
}
