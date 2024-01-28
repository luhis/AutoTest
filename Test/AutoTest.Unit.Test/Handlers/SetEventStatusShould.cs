using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using FluentAssertions.ArgumentMatchers.Moq;
using MediatR;
using Moq;
using Xunit;
using System;

namespace AutoTest.Unit.Test.Handlers
{
    public class SetEventStatusShould
    {
        private readonly IRequestHandler<SetEventStatus> sut;
        private readonly MockRepository mr;
        private readonly Mock<IEventsRepository> events;

        public SetEventStatusShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            events = mr.Create<IEventsRepository>();
            sut = new SetEventStatusHandler(events.Object);
        }

        [Fact]
        public async Task SetStatus()
        {
            var eventId = 11ul;
            var clubId = 2ul;
            events.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(new Event(eventId, clubId, "location", new DateTime(2000, 1, 1), 2, 3, "regs", new[] { Domain.Enums.EventType.AutoSolo }, "maps", Domain.Enums.TimingSystem.App, new DateTime(), new DateTime(), 10));
            var toSave = new Event(eventId, clubId, "location", new DateTime(2000, 1, 1), 2, 3, "regs", new[] { Domain.Enums.EventType.AutoSolo }, "maps", Domain.Enums.TimingSystem.App, new DateTime(), new DateTime(), 10);
            toSave.SetEventStatus(Domain.Enums.EventStatus.Open);
            events.Setup(a => a.Upsert(Its.EquivalentTo(toSave), CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new SetEventStatus(eventId, Domain.Enums.EventStatus.Open), CancellationToken.None);
            mr.VerifyAll();
        }
    }
}
