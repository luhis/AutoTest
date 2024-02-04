using System.Collections.Generic;
using System;
using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels
{
    public class Event
    {
        public Event(ulong eventId, ulong clubId, string location, DateTime startTime, int courseCount, int maxAttemptsPerCourse, string regulations, ICollection<EventType> eventTypes, string maps, TimingSystem timingSystem, DateTime entryOpenDate, DateTime entryCloseDate, uint maxEntrants, DateTime created)
        {
            EventId = eventId;
            ClubId = clubId;
            Location = location;
            StartTime = startTime;
            CourseCount = courseCount;
            MaxAttemptsPerCourse = maxAttemptsPerCourse;
            Regulations = regulations;
            EventTypes = eventTypes;
            Maps = maps;
            TimingSystem = timingSystem;
            EntryOpenDate = entryOpenDate;
            EntryCloseDate = entryCloseDate;
            MaxEntrants = maxEntrants;
            Created = created;
        }

        public ulong EventId { get; }

        public ulong ClubId { get; }

        public string Location { get; }

        public DateTime StartTime { get; }

        public int CourseCount { get; }

        public int MaxAttemptsPerCourse { get; }

        public ICollection<Course> Courses { get; private set; } = new List<Course>();

        public void SetCourses(ICollection<Course> courses)
        {
            if (courses.Count != this.CourseCount)
            {
                throw new ArgumentException("Invalid number of courses", nameof(courses));
            }
            Courses = courses;
        }

        public string Regulations { get; }

        public string Maps { get; }

        public EventStatus EventStatus { get; private set; }

        public ICollection<EventType> EventTypes { get; private set; } = new List<EventType>();
        public void SetEventTypes(ICollection<EventType> eventTypes) => EventTypes = eventTypes;

        public TimingSystem TimingSystem { get; }

        public DateTime EntryOpenDate { get; }

        public DateTime EntryCloseDate { get; }

        public DateTime Created { get; }

        public uint MaxEntrants { get; }

        public void SetEventStatus(EventStatus eventStatus) => this.EventStatus = eventStatus;

    }
}
