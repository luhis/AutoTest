using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using FluentAssertions.ArgumentMatchers.Moq;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class SetEntrantStatusShould
    {
        private readonly IRequestHandler<SetEntrantStatus> sut;
        private readonly MockRepository mr;
        private readonly Mock<IEntrantsRepository> entrants;

        public SetEntrantStatusShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            entrants = mr.Create<IEntrantsRepository>();
            sut = new SetEntrantStatusHandler(entrants.Object);
        }

        [Fact]
        public async Task SetStatus()
        {
            var eventId = 11ul;
            var entrantId = 11ul;
            entrants.Setup(a => a.GetById(eventId, entrantId, CancellationToken.None)).ReturnsAsync(Models.GetEntrant(entrantId, eventId));
            var toSave = Models.GetEntrant(entrantId, eventId);
            toSave.SetEntrantStatus(Domain.Enums.EntrantStatus.Withdrawn);
            entrants.Setup(a => a.Upsert(Its.EquivalentTo(toSave, o => o.Excluding(a => a.EmergencyContact).Excluding(a => a.MsaMembership).Excluding(a => a.AcceptDeclaration)), CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new SetEntrantStatus(eventId, entrantId, Domain.Enums.EntrantStatus.Withdrawn), CancellationToken.None);
            mr.VerifyAll();
        }
    }
}
