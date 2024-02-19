using System;
using System.Linq;
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;

namespace AutoTest.Integration.Test.Tooling
{
    public static class DbInitialiser
    {
        public static void InitializeDbForTests(AutoTestContext db)
        {
            var clubId = 1ul;
            if (!db.Clubs.Where(a => a.ClubId == clubId).Any())
            {
                var club = new Club(clubId, "BHMC", "", "www.club.com");
                club.SetAdminEmails(new[] { new AuthorisationEmail("user@test.com") });
                db.Clubs.Add(club);
            }
            var eventId = 22ul;
            if (!db.Events.Where(a => a.EventId == eventId).Any())
            {
                var @event = new Event(eventId, clubId, "", DateTime.Today, 10, 2, "", new[] { EventType.AutoTest }, string.Empty, TimingSystem.StopWatch, DateTime.Today.Date, DateTime.Today.Date.AddDays(7), 10, DateTime.UtcNow);
                @event.SetCourses(Enumerable.Range(0, 10).Select(a => new Course(a, "")).ToArray());
                db.Events.Add(@event);
            }
            if (!db.Marshals.Where(a => a.MarshalId == 1).Any())
            {
                db.Marshals.Add(new Marshal(1, "Dave", "Marshal", "test@test.com", eventId, 123, "role"));
            }
            if (!db.Entrants.Where(a => a.EntrantId == 1).Any())
            {
                db.Entrants.Add(new Entrant(1, 2, "Dave", "Entrant", "test@test.com", "A", eventId, Age.Senior, false));
            }
            if (!db.Notifications.Where(a => a.NotificationId == 1).Any())
            {
                db.Notifications.Add(new Notification(1, eventId, "test message", new DateTime(), "Test User"));
            };
            if (!db.TestRuns.Where(a => a.TestRunId == 1).Any())
            {
                db.TestRuns.Add(new TestRun(1, eventId, 2, 60_000, 1, new DateTime(), 1));
            }
            db.SaveChanges();
        }
    }
}
