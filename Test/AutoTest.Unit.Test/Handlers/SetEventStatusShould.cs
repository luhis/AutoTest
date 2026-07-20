using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using AwesomeAssertions.ArgumentMatchers.Moq;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class SetEventStatusShould
{
    private readonly IRequestHandler<SetEventStatus> sut;
    private readonly MockRepository mr;
    private readonly Mock<IEventsRepository> events;
    private readonly Mock<IEventNotifier> notifier;

    public SetEventStatusShould()
    {
        mr = new MockRepository(MockBehavior.Strict);
        events = mr.Create<IEventsRepository>();
        notifier = mr.Create<IEventNotifier>();
        sut = new SetEventStatusHandler(events.Object, notifier.Object);
    }

    [Fact]
    public async Task SetStatus()
    {
        var eventId = 11ul;
        var clubId = 2ul;
        events.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(Models.GetEvent(eventId, clubId));
        var toSave = Models.GetEvent(eventId, clubId);
        toSave.SetEventStatus(Domain.Enums.EventStatus.Open);
        events.Setup(a => a.Upsert(Its.EquivalentTo(toSave), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        notifier.Setup(a => a.EventStatusChanged(eventId, Domain.Enums.EventStatus.Open, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        await sut.Handle(new SetEventStatus(eventId, Domain.Enums.EventStatus.Open), TestContext.Current.CancellationToken);
        mr.VerifyAll();
    }
}
