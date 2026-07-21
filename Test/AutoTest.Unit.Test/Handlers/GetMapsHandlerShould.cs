using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AwesomeAssertions;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class GetMapsHandlerShould
{
    private readonly MockRepository _mr;
    private readonly IRequestHandler<GetMaps, string> _sut;
    private readonly Mock<IFileRepository> _fs;

    public GetMapsHandlerShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _fs = _mr.Create<IFileRepository>();
        _sut = new GetMapsHandler(_fs.Object);
    }

    [Fact]
    public async Task Get()
    {
        _fs.Setup(a => a.GetMaps(11, TestContext.Current.CancellationToken)).ReturnsAsync("data");

        var maps = await _sut.Handle(new GetMaps(11), TestContext.Current.CancellationToken);

        maps.Should().Be("data");
        _mr.VerifyAll();
    }
}
