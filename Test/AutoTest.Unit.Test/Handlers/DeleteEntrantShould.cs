using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class DeleteEntrantShould
{
    private readonly IRequestHandler<DeleteEntrant> _sut;
    private readonly MockRepository _mr;
    private readonly Mock<IEntrantsRepository> _entrants;

    public DeleteEntrantShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _entrants = _mr.Create<IEntrantsRepository>();
        _sut = new DeleteEntrantHandler(_entrants.Object);
    }

    [Fact]
    public async Task DeleteEntrant()
    {
        var eventId = 1ul;
        var entrantId = 2ul;
        var entrant = Models.GetEntrant(entrantId, eventId);
        _entrants.Setup(a => a.GetById(eventId, entrantId, TestContext.Current.CancellationToken)).ReturnsAsync(entrant);
        _entrants.Setup(a => a.Delete(entrant, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        await _sut.Handle(new(eventId, entrantId), TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }
}
