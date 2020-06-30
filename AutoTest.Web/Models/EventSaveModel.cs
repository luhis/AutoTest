using System;

namespace AutoTest.Web.Models
{
    public class EventSaveModel
    {
        public ulong EventId { get; set; }

        public ulong ClubId { get; set; }

        public string Location { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public uint TestCount { get; set; }
    }
}
