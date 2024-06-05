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
    public class GetMapsHandlerShould
    {
        private readonly MockRepository mr;
        private readonly IRequestHandler<GetMaps, string> sut;
        private readonly Mock<IFileRepository> fs;

        public GetMapsHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            fs = mr.Create<IFileRepository>();
            sut = new GetMapsHandler(fs.Object);
        }

        [Fact]
        public async Task Get()
        {
            fs.Setup(a => a.GetMaps(11, CancellationToken.None)).ReturnsAsync("data");

            var maps = await sut.Handle(new GetMaps(11), CancellationToken.None);

            maps.Should().BeEquivalentTo("data");
            mr.VerifyAll();
        }
    }
}
