using System;
using System.Collections.Generic;
using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels
{
    public class Event(ulong eventId, ulong clubId, string location, DateTime startTime, int courseCount, int maxAttemptsPerCourse, string regulations, ICollection<EventType> eventTypes, string maps, TimingSystem timingSystem, DateTime entryOpenDate, DateTime entryCloseDate, uint maxEntrants, DateTime created)
    {
        public ulong EventId { get; } = eventId;

        public ulong ClubId { get; } = clubId;

        public string Location { get; } = location;

        public DateTime StartTime { get; } = startTime;

        public int CourseCount { get; } = courseCount;

        public int MaxAttemptsPerCourse { get; } = maxAttemptsPerCourse;

        public ICollection<Course> Courses { get; private set; } = new List<Course>();

        public void SetCourses(ICollection<Course> courses)
        {
            if (courses.Count != this.CourseCount)
            {
                throw new ArgumentException("Invalid number of courses", nameof(courses));
            }
            Courses = courses;
        }

        public string Regulations { get; } = regulations;

        public string Maps { get; } = maps;

        public EventStatus EventStatus { get; private set; }

        public ICollection<EventType> EventTypes { get; private set; } = eventTypes;
        public void SetEventTypes(ICollection<EventType> eventTypes) => EventTypes = eventTypes;

        public TimingSystem TimingSystem { get; } = timingSystem;

        public DateTime EntryOpenDate { get; } = entryOpenDate;

        public DateTime EntryCloseDate { get; } = entryCloseDate;

        public DateTime Created { get; } = created;

        public uint MaxEntrants { get; } = maxEntrants;

        public void SetEventStatus(EventStatus eventStatus) => this.EventStatus = eventStatus;

    }
}
