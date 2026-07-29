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
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IFileRepository> _fs;
    private readonly IRequestHandler<GetMaps, string> _sut;

    public GetMapsHandlerShould()
    {
        _fs = _mr.Create<IFileRepository>();
        _sut = new GetMapsHandler(_fs.Object);
    }

    [Fact]
    public async Task Get()
    {
        _fs.Setup(a => a.GetMaps(11, TestContext.Current.CancellationToken)).ReturnsAsync("data").Verifiable(Times.Once);

        var maps = await _sut.Handle(new GetMaps(11), TestContext.Current.CancellationToken);

        maps.Should().Be("data");
        _mr.VerifyAll();
    }
}
