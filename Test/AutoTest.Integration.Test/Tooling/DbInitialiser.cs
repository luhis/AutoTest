using System;
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
            var club = new Club(clubId, "BHMC", "", "www.club.com");
            club.SetAdminEmails(new[] { new AuthorisationEmail("user@test.com") });
            db.Clubs.Add(club);
            var eventId = 22ul;
            db.Events.Add(new Event(eventId, clubId, "", DateTime.Today, 10, 2, "", new[] { EventType.AutoTest }, string.Empty, TimingSystem.StopWatch, DateTime.Today.Date, DateTime.Today.Date.AddDays(7), 10, DateTime.UtcNow));
            db.Marshals.Add(new Marshal(1, "Dave", "Marshal", "test@test.com", eventId, 123, "role"));
            db.Entrants.Add(new Entrant(1, 2, "Dave", "Entrant", "test@test.com", EventType.AutoTest, "A", eventId, "BRMC", "123", Age.Senior, false));
            db.SaveChanges();
        }
    }
}
