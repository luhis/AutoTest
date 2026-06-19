using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AutoTest.Persistence;

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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutoTestContext).Assembly);
    }

    public async Task SeedDatabaseAsync()
    {
        await this.Database.EnsureCreatedAsync();
        if (!this.Database.IsInMemory())
        {
            if (await this.Clubs.FindAsync(1UL, CancellationToken.None) == null)
            {
                var brmc = new Club(1, "Brighton and Hove Motor Club", "bhmc@paypal.com", "https://www.bhmc.club");
                brmc.SetAdminEmails(new[] { new AuthorisationEmail("mccorry@gmail.com"), new AuthorisationEmail("briandyer68@hotmail.com") });
                this.Clubs.Add(brmc);
            }

            if (await this.Events.FindAsync(2UL, CancellationToken.None) == null)
            {
                var e1 = new Event(2, 1, "Kev's Farm", new DateTime(2024, 3, 1), 10, 2, string.Empty, new[] { EventType.AutoTest }, string.Empty, TimingSystem.StopWatch, new DateTime(2000, 1, 1), new DateTime(2030, 1, 1), 10, DateTime.UtcNow);
                e1.SetCourses(Enumerable.Range(0, 10).Select(x => new Course(x, "")).ToArray());
                this.Events.Add(e1);
            }

            if (await this.Events.FindAsync(3UL, CancellationToken.None) == null)
            {
                var e2 = new Event(3, 1, "Kev's Farm 2", new DateTime(2024, 1, 1), 10, 2, string.Empty, new[] { EventType.AutoTest }, string.Empty, TimingSystem.StopWatch, new DateTime(2000, 1, 1), new DateTime(2030, 1, 1), 10, DateTime.UtcNow);
                e2.SetCourses(Enumerable.Range(0, 10).Select(x => new Course(x, "")).ToArray());
                this.Events.Add(e2);
            }

            if (await this.Entrants.FindAsync(4UL, CancellationToken.None) == null)
            {
                var en1 = new Entrant(4, 1, "Matt", "McCorry", "test@email.com", "A", 1, Age.Senior, false, null);
                en1.SetVehicle(new Vehicle("Vauxhall", "Corsa", 1229, Induction.NA, "AA05AAA"));
                en1.SetMsaMembership(new MsaMembership("Clubman", 1234));
                this.Entrants.Add(en1);
            }

            if (await this.Entrants.FindAsync(5UL, CancellationToken.None) == null)
            {
                var en2 = new Entrant(5, 2, "Matt", "McCorry", "test@email.com", "A", 2, Age.Senior, false, null);
                en2.SetVehicle(new Vehicle("Vauxhall", "Corsa", 1229, Induction.NA, "AA05AAA"));
                en2.SetMsaMembership(new MsaMembership("Clubman", 1234));
                this.Entrants.Add(en2);
            }

            if (await this.Marshals.FindAsync(6UL, CancellationToken.None) == null)
            {
                var m = new Marshal(6, "Matt", "McCorry", "mccorry@gmail.com", 2, 69, "Play");
                this.Marshals.Add(m);
            }

            await this.SaveChangesAsync();
        }
    }
}
