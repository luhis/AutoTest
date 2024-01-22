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
            db.Events!.Add(new Event(22, 1, "", DateTime.Today, 10, 2, "", new[] { EventType.AutoTest }, string.Empty, TimingSystem.StopWatch, DateTime.Today.Date, DateTime.Today.Date.AddDays(7), 10));
            db.Marshals!.Add(new Marshal(1, "Dave", "Marshal", "test@test.com", 1, 123, "role"));
            db.Entrants!.Add(new Entrant(1, 2, "Dave", "Marshal", "test@test.com", EventType.AutoTest, "A", 1, "BRMC", 123, Age.Senior));
            db.SaveChanges();
        }
    }
}
