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
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IEventsRepository> _events;
    private readonly Mock<IEventNotifier> _notifier;
    private readonly IRequestHandler<SetEventStatus> _sut;

    public SetEventStatusShould()
    {
        _events = _mr.Create<IEventsRepository>();
        _notifier = _mr.Create<IEventNotifier>();
        _sut = new SetEventStatusHandler(_events.Object, _notifier.Object);
    }

    [Fact]
    public async Task SetStatus()
    {
        var eventId = 11ul;
        var clubId = 2ul;
        _events.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(Models.GetEvent(eventId, clubId));
        var toSave = Models.GetEvent(eventId, clubId);
        toSave.SetEventStatus(Domain.Enums.EventStatus.Open);
        _events.Setup(a => a.Upsert(Its.EquivalentTo(toSave), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        _notifier.Setup(a => a.EventStatusChanged(eventId, Domain.Enums.EventStatus.Open, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        await _sut.Handle(new SetEventStatus(eventId, Domain.Enums.EventStatus.Open), TestContext.Current.CancellationToken);
        _mr.VerifyAll();
    }
}
