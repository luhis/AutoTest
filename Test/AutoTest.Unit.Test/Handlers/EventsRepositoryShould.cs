using System;
using System.Linq;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence.Repositories;
using AutoTest.Unit.Test.Fixtures;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class EventsRepositoryShould
{
    [Fact]
    public async Task GetByIdReturnsEvent()
    {
        using var db = InMemDbFixture.GetDbContext();
        IEventsRepository sut = new EventsRepository(db);

        var evnt = new Event(1, 1, "Test Farm", new DateTime(2024, 6, 1), 2, 1, new[] { EventType.AutoTest }, TimingSystem.StopWatch, new DateTime(2000, 1, 1), new DateTime(2030, 1, 1), 10, DateTime.UtcNow);
        evnt.SetCourses(new[] { new Course(0, ""), new Course(1, "") });
        db.Events.Add(evnt);
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await sut.GetById(1, TestContext.Current.CancellationToken);

        result.Should().NotBeNull();
        result!.Location.Should().Be("Test Farm");
    }

    [Fact]
    public async Task GetByIdReturnsNullWhenNotFound()
    {
        using var db = InMemDbFixture.GetDbContext();
        IEventsRepository sut = new EventsRepository(db);

        var result = await sut.GetById(999, TestContext.Current.CancellationToken);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllReturnsAllEvents()
    {
        using var db = InMemDbFixture.GetDbContext();
        IEventsRepository sut = new EventsRepository(db);

        db.Events.AddRange(
            new Event(1, 1, "Farm 1", new DateTime(2024, 1, 1), 1, 1, new[] { EventType.AutoTest }, TimingSystem.StopWatch, new DateTime(2000, 1, 1), new DateTime(2030, 1, 1), 10, DateTime.UtcNow),
            new Event(2, 1, "Farm 2", new DateTime(2024, 6, 1), 1, 1, new[] { EventType.AutoTest }, TimingSystem.StopWatch, new DateTime(2000, 1, 1), new DateTime(2030, 1, 1), 10, DateTime.UtcNow));
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = (await sut.GetAll(TestContext.Current.CancellationToken)).ToArray();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpsertAddsNewEvent()
    {
        using var db = InMemDbFixture.GetDbContext();
        IEventsRepository sut = new EventsRepository(db);

        var evnt = new Event(1, 1, "New Farm", new DateTime(2024, 6, 1), 1, 1, new[] { EventType.AutoTest }, TimingSystem.StopWatch, new DateTime(2000, 1, 1), new DateTime(2030, 1, 1), 10, DateTime.UtcNow);
        evnt.SetCourses(new[] { new Course(0, "") });
        await sut.Upsert(evnt, TestContext.Current.CancellationToken);

        var result = await db.Events.FindAsync(new object[] { 1UL }, TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        result!.Location.Should().Be("New Farm");
    }

    [Fact]
    public async Task DeleteRemovesEvent()
    {
        using var db = InMemDbFixture.GetDbContext();
        IEventsRepository sut = new EventsRepository(db);

        var evnt = new Event(1, 1, "To Delete", new DateTime(2024, 6, 1), 1, 1, new[] { EventType.AutoTest }, TimingSystem.StopWatch, new DateTime(2000, 1, 1), new DateTime(2030, 1, 1), 10, DateTime.UtcNow);
        evnt.SetCourses(new[] { new Course(0, "") });
        db.Events.Add(evnt);
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        await sut.Delete(evnt, TestContext.Current.CancellationToken);

        var result = await db.Events.FindAsync(new object[] { 1UL }, TestContext.Current.CancellationToken);
        result.Should().BeNull();
    }
}
