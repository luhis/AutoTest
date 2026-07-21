using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using AwesomeAssertions.ArgumentMatchers.Moq;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class SetEntrantStatusShould
{
    private readonly IRequestHandler<SetEntrantStatus> _sut;
    private readonly MockRepository _mr;
    private readonly Mock<IEntrantsRepository> _entrants;

    public SetEntrantStatusShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _entrants = _mr.Create<IEntrantsRepository>();
        _sut = new SetEntrantStatusHandler(_entrants.Object);
    }

    [Fact]
    public async Task SetStatus()
    {
        var eventId = 11ul;
        var entrantId = 11ul;
        _entrants.Setup(a => a.GetById(eventId, entrantId, TestContext.Current.CancellationToken)).ReturnsAsync(Models.GetEntrant(entrantId, eventId));
        var toSave = Models.GetEntrant(entrantId, eventId);
        toSave.SetEntrantStatus(Domain.Enums.EntrantStatus.Withdrawn);
        _entrants.Setup(a => a.Upsert(Its.EquivalentTo(toSave, o => o.Excluding(a => a.EmergencyContact).Excluding(a => a.MsaMembership).Excluding(a => a.AcceptDeclaration)), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        await _sut.Handle(new SetEntrantStatus(eventId, entrantId, Domain.Enums.EntrantStatus.Withdrawn), TestContext.Current.CancellationToken);
        _mr.VerifyAll();
    }
}
