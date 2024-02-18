using System;
using System.Collections.Generic;
using AutoTest.Domain.Enums;
using Microsoft.VisualBasic;

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

        public ICollection<EventType> EventTypes { get; set; } = Array.Empty<EventType>();

        public DateTime EntryOpenDate { get; set; }

        public DateTime EntryCloseDate { get; set; }

        public uint MaxEntrants { get; set; }

        public TimingSystem TimingSystem { get; set; }
    }
}
