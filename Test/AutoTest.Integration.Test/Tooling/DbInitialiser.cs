using System;
using System.Linq;
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;

namespace AutoTest.Integration.Test.Tooling;

public static class DbInitialiser
{
    public static void InitializeDbForTests(AutoTestContext db)
    {
        if (!db.Clubs.Where(a => a.ClubId == TestIds.ClubId).Any())
        {
            var club = new Club(TestIds.ClubId, "BHMC", "", "www.club.com");
            club.SetAdminEmails(new[] { new AuthorisationEmail("user@test.com") });
            db.Clubs.Add(club);
        }
        if (!db.Events.Where(a => a.EventId == TestIds.EventId).Any())
        {
            var @event = new Event(TestIds.EventId, TestIds.ClubId, "", DateTime.Today, 10, 2, new[] { EventType.AutoTest }, TimingSystem.StopWatch, DateTime.Today.Date, DateTime.Today.Date.AddDays(7), 10, DateTime.UtcNow);
            @event.SetCourses(Enumerable.Range(0, 10).Select(a => new Course(a, "")).ToArray());
            db.Events.Add(@event);
        }
        if (!db.Marshals.Where(a => a.MarshalId == TestIds.MarshalId).Any())
        {
            db.Marshals.Add(new Marshal(TestIds.MarshalId, "Dave", "Marshal", "test@test.com", TestIds.EventId, 123, "role"));
        }
        if (!db.Entrants.Where(a => a.EntrantId == TestIds.EntrantId).Any())
        {
            db.Entrants.Add(new Entrant(TestIds.EntrantId, 2, "Dave", "Entrant", "test@test.com", "A", TestIds.EventId, Age.Senior, false, null));
        }
        if (!db.Notifications.Where(a => a.NotificationId == TestIds.NotificationId).Any())
        {
            db.Notifications.Add(new Notification(TestIds.NotificationId, TestIds.EventId, "test message", new DateTime(), "Test User"));
        }
        if (!db.TestRuns.Where(a => a.TestRunId == TestIds.TestRunId).Any())
        {
            db.TestRuns.Add(new TestRun(TestIds.TestRunId, TestIds.EventId, TestIds.TestNumber, 60_000, 1, new DateTime(), 1));
        }
        db.SaveChanges();
    }
}
