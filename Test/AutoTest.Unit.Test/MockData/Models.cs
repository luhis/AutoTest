using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Unit.Test.MockData
{
    public static class Models
    {
        public static Event GetEvent(ulong eventId, ulong clubId = 1) => new Event(eventId, clubId, "Kestrel Farm", new System.DateTime(), 1, 1, "", new EventType[] { EventType.AutoTest }, "", TimingSystem.StopWatch, new System.DateTime(), new System.DateTime(), 2, new System.DateTime());

        public static Profile GetProfile(string email) => new Profile(email, "First", "Last", Age.Junior, false);
    }
}
