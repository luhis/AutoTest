using System;
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models;

namespace AutoTest.Web.Mapping
{
    public static class MapEvent
    {
        public static Event Map(ulong eventId, EventSaveModel @event)
        {
            var e = new Event(eventId, @event.ClubId, @event.Location, @event.StartTime, @event.TestCount,
                @event.MaxAttemptsPerTest, @event.Regulations, @event.EventTypes, @event.Maps, TimingSystem.StopWatch, @event.EntryOpenDate, @event.EntryCloseDate, @event.MaxEntrants, DateTime.UtcNow);
            return e;
        }
    }
}
