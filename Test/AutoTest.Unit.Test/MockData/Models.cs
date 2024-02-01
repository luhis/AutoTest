
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Unit.Test.MockData
{
    public static class Models
    {
        public static Event GetEvent(ulong eventId) => new Event(eventId, 1, "", new System.DateTime(), 1, 1, "", new EventType[] { }, "", TimingSystem.StopWatch, new System.DateTime(), new System.DateTime(), 2);
        public static Profile GetProfile(string email) => new Profile(email, "First", "Last", Domain.Enums.Age.Junior, false);
    }
}
