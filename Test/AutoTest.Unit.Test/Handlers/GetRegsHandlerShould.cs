using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AwesomeAssertions;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class GetRegsHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IFileRepository> _fs;
    private readonly IRequestHandler<GetRegs, string> _sut;

    public GetRegsHandlerShould()
    {
        _fs = _mr.Create<IFileRepository>();
        _sut = new GetRegsHandler(_fs.Object);
    }

    [Fact]
    public async Task Get()
    {
        _fs.Setup(a => a.GetRegs(11, TestContext.Current.CancellationToken)).ReturnsAsync("data").Verifiable(Times.Once);

        var regs = await _sut.Handle(new GetRegs(11), TestContext.Current.CancellationToken);

        regs.Should().Be("data");
        _mr.VerifyAll();
    }
}
