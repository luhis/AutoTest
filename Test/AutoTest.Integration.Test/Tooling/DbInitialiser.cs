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
            db.Events!.Add(new Event(22, 1, "", DateTime.Today, 10, 2, "", EventType.AutoTest, string.Empty, TimingSystem.StopWatch, DateTime.Today.Date, DateTime.Today.Date.AddDays(7), 10));
            db.SaveChanges();
        }
    }
}
