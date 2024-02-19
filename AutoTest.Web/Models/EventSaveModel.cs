using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoTest.Domain.Enums;

namespace AutoTest.Web.Models
{
    public class EventSaveModel
    {
        [Required]
        public ulong ClubId { get; set; }

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CourseCount { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MaxAttemptsPerCourse { get; set; }

        public string Regulations { get; set; } = string.Empty;

        public string Maps { get; set; } = string.Empty;

        [Required]
        public ICollection<EventType> EventTypes { get; set; } = Array.Empty<EventType>();

        [Required]
        public DateTime EntryOpenDate { get; set; }

        [Required]
        public DateTime EntryCloseDate { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public uint MaxEntrants { get; set; }

        [Required]
        public TimingSystem TimingSystem { get; set; }
    }
}
