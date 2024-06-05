using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
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
            fs.Setup(a => a.GetRegs(11, CancellationToken.None)).ReturnsAsync("data");

            var regs = await sut.Handle(new GetRegs(11), CancellationToken.None);

            regs.Should().BeEquivalentTo("data");
            mr.VerifyAll();
        }
    }
}
