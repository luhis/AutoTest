
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;
using Microsoft.Extensions.Logging;

namespace AutoTest.Unit.Test.MockData
{
    public static class Models
    {
        public static Event GetEvent(ulong eventId) => new Event(eventId, 1, "", new System.DateTime(), 1, 1, "", new EventType[] { }, "", TimingSystem.StopWatch, new System.DateTime(), new System.DateTime(), 2);
    }
}
