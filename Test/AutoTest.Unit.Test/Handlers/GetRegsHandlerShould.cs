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
    private readonly MockRepository mr;
    private readonly IRequestHandler<GetRegs, string> sut;
    private readonly Mock<IFileRepository> fs;

    public GetRegsHandlerShould()
    {
        mr = new MockRepository(MockBehavior.Strict);
        fs = mr.Create<IFileRepository>();
        sut = new GetRegsHandler(fs.Object);
    }

    [Fact]
    public async Task Get()
    {
        fs.Setup(a => a.GetRegs(11, TestContext.Current.CancellationToken)).ReturnsAsync("data");

        var regs = await sut.Handle(new GetRegs(11), TestContext.Current.CancellationToken);

        regs.Should().Be("data");
        mr.VerifyAll();
    }
}
