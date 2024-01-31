using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class GetEntrantHandlerShould
    {
        private readonly MockRepository mr;
        private readonly IRequestHandler<GetEntrant, Entrant?> sut;
        private readonly Mock<IEntrantsRepository> profileRepository;

        public GetEntrantHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            profileRepository = mr.Create<IEntrantsRepository>();
            sut = new GetEntrantHandler(profileRepository.Object);
        }

        [Fact]
        public async Task GetMarshals()
        {
            var eventId = 1ul;
            var entrantId = (ushort)2u;
            var entrant = new Entrant(1, entrantId, "Joe", "Bloggs", "a@a.com", Domain.Enums.EventType.AutoTest, "A", 99, "BRMC", "123456", Domain.Enums.Age.Senior, false);
            profileRepository.Setup(a => a.GetById(eventId, entrantId, CancellationToken.None)).ReturnsAsync(entrant);

            var res = await sut.Handle(new(eventId, entrantId), CancellationToken.None);

            res.Should().BeEquivalentTo(entrant);
            mr.VerifyAll();
        }
    }
}
