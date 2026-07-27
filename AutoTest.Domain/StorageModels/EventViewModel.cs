using System;
using System.Collections.Generic;
using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels;

public record EventViewModel(
    ulong EventId,
    ulong ClubId,
    string Location,
    DateTime StartTime,
    int CourseCount,
    int MaxAttemptsPerCourse,
    ICollection<EventType> EventTypes,
    TimingSystem TimingSystem,
    DateTime EntryOpenDate,
    DateTime EntryCloseDate,
    uint MaxEntrants,
    EventStatus EventStatus,
    DateTime Created,
    bool HasRegulations,
    bool HasMaps);
