using System;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;

namespace AutoTest.Integration.Test.Tooling
{
    public static class DbInitialiser
    {
        public static void InitializeDbForTests(AutoTestContext db)
        {
            db.Events.ThrowIfNull().Add(new Event(22, 1, "", DateTime.Today, 10, 2, "", EventType.AutoTest));
            db.SaveChanges();
        }
    }
}
