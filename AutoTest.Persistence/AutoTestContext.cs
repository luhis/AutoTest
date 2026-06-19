using System;

namespace AutoTest.Persistence
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Domain.Enums;
    using AutoTest.Domain.StorageModels;
    using AutoTest.Persistence.Setup;
    using Microsoft.EntityFrameworkCore;

    using Microsoft.EntityFrameworkCore.Diagnostics;

    public class AutoTestContext : DbContext
    {
        public AutoTestContext(DbContextOptions<AutoTestContext> options)
            : base(options)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(CosmosEventId.NoPartitionKeyDefined));
        }

        public DbSet<Club> Clubs { get; private set; }
        public DbSet<Event> Events { get; private set; }
        public DbSet<Entrant> Entrants { get; private set; }
        public DbSet<Marshal> Marshals { get; private set; }
        public DbSet<TestRun> TestRuns { get; private set; }
        public DbSet<Profile> Users { get; private set; }
        public DbSet<Notification> Notifications { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupClub.Setup(modelBuilder.Entity<Club>());
            SetupEntrant.Setup(modelBuilder.Entity<Entrant>());
            SetupMarshal.Setup(modelBuilder.Entity<Marshal>());
            SetupEvent.Setup(modelBuilder.Entity<Event>());
            SetupTestRun.Setup(modelBuilder.Entity<TestRun>());
            SetupProfile.Setup(modelBuilder.Entity<Profile>());
            SetupNotification.Setup(modelBuilder.Entity<Notification>());
        }

        public async Task SeedDatabaseAsync()
        {
            if (this.Database.IsInMemory())
            {
                await this.Database.EnsureCreatedAsync();
            }

            if (await this.Clubs.FindAsync(1UL, CancellationToken.None) == null)
            {
                var brmc = new Club(1, "Brighton and Hove Motor Club", "bhmc@paypal.com", "https://www.bhmc.club");
                brmc.SetAdminEmails(new[] { new AuthorisationEmail("mccorry@gmail.com"), new AuthorisationEmail("briandyer68@hotmail.com") });
                this.Clubs.Add(brmc);
            }

            if (await this.Events.FindAsync(1UL, CancellationToken.None) == null)
            {
                var e1 = new Event(1, 1, "Kev's Farm", new DateTime(2024, 3, 1), 10, 2, string.Empty, new[] { EventType.AutoTest }, string.Empty, TimingSystem.StopWatch, new DateTime(2000, 1, 1), new DateTime(2030, 1, 1), 10, DateTime.UtcNow);
                e1.SetCourses(Enumerable.Range(0, 10).Select(x => new Course(x, "")).ToArray());
                this.Events.Add(e1);
            }

            if (await this.Events.FindAsync(2UL, CancellationToken.None) == null)
            {
                var e2 = new Event(2, 1, "Kev's Farm 2", new DateTime(2024, 1, 1), 10, 2, string.Empty, new[] { EventType.AutoTest }, string.Empty, TimingSystem.StopWatch, new DateTime(2000, 1, 1), new DateTime(2030, 1, 1), 10, DateTime.UtcNow);
                e2.SetCourses(Enumerable.Range(0, 10).Select(x => new Course(x, "")).ToArray());
                this.Events.Add(e2);
            }

            if (await this.Entrants.FindAsync(1UL, CancellationToken.None) == null)
            {
                var en1 = new Entrant(1, 1, "Matt", "McCorry", "test@email.com", "A", 1, Age.Senior, false, null);
                en1.SetVehicle(new Vehicle("Vauxhall", "Corsa", 1229, Induction.NA, "AA05AAA"));
                en1.SetMsaMembership(new MsaMembership("Clubman", 1234));
                this.Entrants.Add(en1);
            }

            if (await this.Entrants.FindAsync(2UL, CancellationToken.None) == null)
            {
                var en2 = new Entrant(2, 2, "Matt", "McCorry", "test@email.com", "A", 2, Age.Senior, false, null);
                en2.SetVehicle(new Vehicle("Vauxhall", "Corsa", 1229, Induction.NA, "AA05AAA"));
                en2.SetMsaMembership(new MsaMembership("Clubman", 1234));
                this.Entrants.Add(en2);
            }

            if (await this.Marshals.FindAsync(1UL, CancellationToken.None) == null)
            {
                var m = new Marshal(1, "Matt", "McCorry", "mccorry@gmail.com", 2, 69, "Play");
                this.Marshals.Add(m);
            }

            await this.SaveChangesAsync();
        }
    }
}
