using System;

namespace AutoTest.Web.Models
{
    public class EventSaveModel
    {
        public ulong ClubId { get; set; }

        public string Location { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public int TestCount { get; set; }

        public int MaxAttemptsPerTest { get; set; }
    }
}
