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
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
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

    public async Task SeedDatabaseAsync(CancellationToken cancellationToken = default)
    {
        await Database.EnsureCreatedAsync(cancellationToken);
        if (!Database.IsInMemory())
        {
            if (await Clubs.FindAsync([1UL], cancellationToken) is null)
            {
                var brmc = new Club(1, "Brighton and Hove Motor Club", "bhmc@paypal.com", "https://www.bhmc.club");
                brmc.SetAdminEmails([new AuthorisationEmail("mccorry@gmail.com"), new AuthorisationEmail("briandyer68@hotmail.com")]);
                Clubs.Add(brmc);
            }
            var eventId = 2UL;

            if (await Events.FindAsync([eventId], cancellationToken) is null)
            {
                var e1 = new Event(eventId, 1, "Kev's Farm", new DateTime(2024, 3, 1), 10, 2, [EventType.AutoTest], TimingSystem.StopWatch, new DateTime(2000, 1, 1), new DateTime(2030, 1, 1), 10, DateTime.UtcNow);
                e1.SetCourses(Enumerable.Range(0, 10).Select(x => new Course(x, "")).ToArray());
                Events.Add(e1);
            }

            if (await Events.FindAsync([3UL], cancellationToken) is null)
            {
                var e2 = new Event(3, 1, "Kev's Farm 2", new DateTime(2024, 1, 1), 10, 2, [EventType.AutoTest], TimingSystem.StopWatch, new DateTime(2000, 1, 1), new DateTime(2030, 1, 1), 10, DateTime.UtcNow);
                e2.SetCourses(Enumerable.Range(0, 10).Select(x => new Course(x, "")).ToArray());
                Events.Add(e2);
            }

            if (await Entrants.FindAsync([4UL], cancellationToken) is null)
            {
                var en1 = new Entrant(4, 1, "Matt", "McCorry", "test@email.com", "A", 3UL, Age.Senior, false, null);
                en1.SetVehicle(new Vehicle("Vauxhall", "Corsa", 1229, Induction.NA, "AA05AAA"));
                en1.SetMsaMembership(new MsaMembership("Clubman", 1234));
                Entrants.Add(en1);
            }

            var entrantId = 5UL;
            if (await Entrants.FindAsync([entrantId], cancellationToken) is null)
            {
                var en2 = new Entrant(entrantId, 2, "Matt", "McCorry", "test@email.com", "A", eventId, Age.Senior, false, null);
                en2.SetVehicle(new Vehicle("Vauxhall", "Corsa", 1229, Induction.NA, "AA05AAA"));
                en2.SetMsaMembership(new MsaMembership("Clubman", 1234));
                Entrants.Add(en2);
            }

            var marshalId = 6UL;
            if (await Marshals.FindAsync([marshalId], cancellationToken) is null)
            {
                var m = new Marshal(marshalId, "Matt", "McCorry", "mccorry@gmail.com", eventId, 69, "Play");
                Marshals.Add(m);
            }

            if (await TestRuns.FindAsync([100UL], cancellationToken) is null)
            {
                TestRuns.Add(new TestRun(98, eventId, 0, 45200, entrantId, DateTime.UtcNow, marshalId));
                TestRuns.Add(new TestRun(99, eventId, 0, 43800, entrantId, DateTime.UtcNow, marshalId));
                TestRuns.Add(new TestRun(100, eventId, 1, 45200, entrantId, DateTime.UtcNow, marshalId));
                TestRuns.Add(new TestRun(101, eventId, 1, 43800, entrantId, DateTime.UtcNow, marshalId));
                TestRuns.Add(new TestRun(102, eventId, 2, 51200, entrantId, DateTime.UtcNow, marshalId));
                TestRuns.Add(new TestRun(103, eventId, 2, 49900, entrantId, DateTime.UtcNow, marshalId));
                TestRuns.Add(new TestRun(104, eventId, 3, 38700, entrantId, DateTime.UtcNow, marshalId));
                TestRuns.Add(new TestRun(105, eventId, 3, 40100, entrantId, DateTime.UtcNow, marshalId));
            }

            await SaveChangesAsync(cancellationToken);
        }
    }
}
