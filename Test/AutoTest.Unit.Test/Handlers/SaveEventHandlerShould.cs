using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class SaveEventHandlerShould
    {
        private readonly MockRepository mr;
        private readonly Mock<IEventsRepository> eventsRepository;
        //private readonly Mock<IFileRepository> fileRepository;
        private readonly IRequestHandler<SaveEvent, ulong> sut;
        public SaveEventHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            eventsRepository = mr.Create<IEventsRepository>();
            //fileRepository = mr.Create<IFileRepository>();
            sut = new SaveEventHandler(eventsRepository.Object);//, fileRepository.Object);
        }

        [Fact]
        public async Task Save()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", eventId, Age.Senior, false);
            entrant.SetPayment(new Payment());
            var evt = new Event(eventId, 1, "location", DateTime.UtcNow, 2, 2, "regs", [], "", TimingSystem.StopWatch, DateTime.UtcNow, DateTime.UtcNow, 22, DateTime.UtcNow);

            var entrantFromDb = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", eventId, Age.Senior, false);
            eventsRepository.Setup(a => a.Upsert(evt, CancellationToken.None)).Returns(Task.CompletedTask);
            //fileRepository.Setup(a => a.SaveMaps(2, "", CancellationToken.None)).ReturnsAsync("");

            var se = new SaveEvent(evt);
            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
