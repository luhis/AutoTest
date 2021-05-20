using System;
using AutoTest.Domain.Enums;

namespace AutoTest.Web.Models
{
    public class EventSaveModel
    {
        public ulong ClubId { get; set; }

        public string Location { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public int TestCount { get; set; }

        public int MaxAttemptsPerTest { get; set; }

        public string Regulations { get; set; } = string.Empty;

        public string Maps { get; set; } = string.Empty;

        public EventType EventType { get; set; }
    }
}
