using System.Collections.Generic;
using System.Linq;
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
    public class GetEntrantsHandlerShould
    {
        private readonly MockRepository mr;
        private readonly IRequestHandler<GetEntrants, IEnumerable<Entrant>> sut;
        private readonly Mock<IEntrantsRepository> profileRepository;

        public GetEntrantsHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            profileRepository = mr.Create<IEntrantsRepository>();
            sut = new GetEntrantsHandler(profileRepository.Object);
        }

        [Fact]
        public async Task GetMarshals()
        {
            var eventId = 1ul;
            var marshals = new[] {
                new Entrant(1, 22, "Joe", "Bloggs", "a@a.com", Domain.Enums.EventType.AutoTest, "A", 99, "BRMC", "123456", Domain.Enums.Age.Senior, false),
                new Entrant(2, 22, "Joe", "Bloggs", "a@a.com", Domain.Enums.EventType.AutoTest, "A", 99, "BRMC", "123456", Domain.Enums.Age.Senior, false)
            };
            profileRepository.Setup(a => a.GetAll(eventId, CancellationToken.None)).ReturnsAsync(marshals);

            var res = await sut.Handle(new(eventId), CancellationToken.None);

            res.Should().BeEquivalentTo(marshals.OrderBy(a => a.FamilyName), o => o.WithStrictOrdering());
            mr.VerifyAll();
        }
    }
}
