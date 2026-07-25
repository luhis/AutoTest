using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class DeleteClubShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IClubsRepository> _clubs;
    private readonly IRequestHandler<DeleteClub> _sut;

    public DeleteClubShould()
    {
        _clubs = _mr.Create<IClubsRepository>();
        _sut = new DeleteClubHandler(_clubs.Object);
    }

    [Fact]
    public async Task DeleteClub()
    {
        var clubId = 1ul;
        _clubs.Setup(a => a.Delete(clubId, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        await _sut.Handle(new(clubId), TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }
}
