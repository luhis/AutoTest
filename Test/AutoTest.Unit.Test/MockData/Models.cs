using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Unit.Test.MockData;

public static class Models
{
    public static Event GetEvent(ulong eventId, ulong clubId = 1) => new(eventId, clubId, "Kestrel Farm", new System.DateTime(), 1, 1, new EventType[] { EventType.AutoTest }, TimingSystem.StopWatch, new System.DateTime(), new System.DateTime(), 2, new System.DateTime());

    public static Profile GetProfile(string email) => new(email, "First", "Last", Age.Junior, false);

    public static Entrant GetEntrant(ulong entrantId, ulong eventId) => new(entrantId, 1, "Joe", "Bloggs", "a@a.com", "A", eventId, Age.Senior, false, null);

    public static Marshal GetMarshal(ulong marshalId, ulong eventId, string email = "a@a.com") => new(marshalId, "Joe", "Bloggs", email, eventId, 123456, "");

    public static Club GetClub(ulong clubId) => new(clubId, "BRMC", "pay@brmc.org", "www.com");

    public static TestRun GetTestRun(ulong testRunId, ulong eventId, int testNumber = 3, ulong entrantId = 4, ulong marshalId = 5) => new(testRunId, eventId, testNumber, 60_000, entrantId, new System.DateTime(2000, 1, 1), marshalId);
}
