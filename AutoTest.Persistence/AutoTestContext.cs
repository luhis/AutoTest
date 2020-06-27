namespace AutoTest.Persistence
{
    using AutoTest.Domain.StorageModels;
    using AutoTest.Persistence.Setup;
    using Microsoft.EntityFrameworkCore;

    public class AutoTestContext : DbContext
    {
        public AutoTestContext(DbContextOptions<AutoTestContext> options)
            : base(options)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Club>? Clubs { get; private set; }
        public DbSet<Event>? Events { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupAdminEmails.Setup(modelBuilder.Entity<AdminEmail>());
            SetupClub.Setup(modelBuilder.Entity<Club>());
            SetupEntrant.Setup(modelBuilder.Entity<Entrant>());
            SetupEvent.Setup(modelBuilder.Entity<Event>());
            SetupTest.Setup(modelBuilder.Entity<Test>());
            SetupTestRun.Setup(modelBuilder.Entity<TestRun>());
            SetupUser.Setup(modelBuilder.Entity<User>());
        }
    }
}
