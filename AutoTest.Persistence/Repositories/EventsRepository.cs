using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Persistence.Repositories;

public class EventsRepository(AutoTestContext autoTestContext) : IEventsRepository
{
    Task<Event?> IEventsRepository.GetById(ulong eventId, CancellationToken cancellationToken)
    {
        return autoTestContext.Events.FindAsync([eventId], cancellationToken).AsTask();
    }

    Task<IEnumerable<Event>> IEventsRepository.GetAll(CancellationToken cancellationToken)
    {
        return autoTestContext.Events.OrderBy(a => a.StartTime).ToEnumerableAsync(cancellationToken);
    }

    async Task IEventsRepository.Upsert(Event @event, CancellationToken cancellationToken)
    {
        var existing = await autoTestContext.Events.FindAsync([@event.EventId], cancellationToken);
        if (existing is not null)
        {
            foreach (var course in existing.Courses)
            {
                @event.Courses.Add(course);
            }
        }
        SyncCourses(@event);
        await autoTestContext.Events.Upsert(@event, a => a.EventId == @event.EventId, cancellationToken);

        await autoTestContext.SaveChangesAsync(cancellationToken);
    }

    private static void SyncCourses(Event @event)
    {
        var courses = @event.Courses;
        var expectedOrdinals = Enumerable.Range(0, @event.CourseCount).ToArray();
        var toAddOrdinals = expectedOrdinals.Except(courses.Select(a => a.Ordinal));

        @event.SetCourses(courses.Where(a => expectedOrdinals.Contains(a.Ordinal)).Concat(toAddOrdinals.Select(a => new Course(a, ""))).ToArray());
    }

    Task IEventsRepository.Delete(Event @event, CancellationToken cancellationToken)
    {
        autoTestContext.Events.Remove(@event);
        return autoTestContext.SaveChangesAsync(cancellationToken);
    }
}
