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

            if (await Events.FindAsync([2UL], cancellationToken) is null)
            {
                var e1 = new Event(2, 1, "Kev's Farm", new DateTime(2024, 3, 1), 10, 2, [EventType.AutoTest], TimingSystem.StopWatch, new DateTime(2000, 1, 1), new DateTime(2030, 1, 1), 10, DateTime.UtcNow);
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
                var en1 = new Entrant(4, 1, "Matt", "McCorry", "test@email.com", "A", 1, Age.Senior, false, null);
                en1.SetVehicle(new Vehicle("Vauxhall", "Corsa", 1229, Induction.NA, "AA05AAA"));
                en1.SetMsaMembership(new MsaMembership("Clubman", 1234));
                Entrants.Add(en1);
            }

            if (await Entrants.FindAsync([5UL], cancellationToken) is null)
            {
                var en2 = new Entrant(5, 2, "Matt", "McCorry", "test@email.com", "A", 2, Age.Senior, false, null);
                en2.SetVehicle(new Vehicle("Vauxhall", "Corsa", 1229, Induction.NA, "AA05AAA"));
                en2.SetMsaMembership(new MsaMembership("Clubman", 1234));
                Entrants.Add(en2);
            }

            if (await Marshals.FindAsync([6UL], cancellationToken) is null)
            {
                var m = new Marshal(6, "Matt", "McCorry", "mccorry@gmail.com", 1, 69, "Play");
                Marshals.Add(m);
            }

            if (await TestRuns.FindAsync([100UL], cancellationToken) is null)
            {
                TestRuns.Add(new TestRun(100, 2, 1, 45200, 4, DateTime.UtcNow, 6));
                TestRuns.Add(new TestRun(101, 2, 1, 43800, 4, DateTime.UtcNow, 6));
                TestRuns.Add(new TestRun(102, 2, 2, 51200, 4, DateTime.UtcNow, 6));
                TestRuns.Add(new TestRun(103, 2, 2, 49900, 4, DateTime.UtcNow, 6));
                TestRuns.Add(new TestRun(104, 2, 3, 38700, 4, DateTime.UtcNow, 6));
                TestRuns.Add(new TestRun(105, 2, 3, 40100, 4, DateTime.UtcNow, 6));
            }

            await SaveChangesAsync(cancellationToken);
        }
    }
}
