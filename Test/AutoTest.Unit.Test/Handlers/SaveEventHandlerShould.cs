using System;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class SaveEventHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IEventsRepository> _eventsRepository;
    private readonly Mock<IFileRepository> _fileRepository;
    private readonly IRequestHandler<SaveEvent, ulong> _sut;

    public SaveEventHandlerShould()
    {
        _eventsRepository = _mr.Create<IEventsRepository>();
        _fileRepository = _mr.Create<IFileRepository>();
        _sut = new SaveEventHandler(_eventsRepository.Object, _fileRepository.Object);
    }

    [Fact]
    public async Task Save()
    {
        var entrantId = 1ul;
        var eventId = 2ul;
        var entrant = Models.GetEntrant(entrantId, eventId);
        entrant.SetPayment(new Payment());
        var evt = new Event(eventId, 1, "location", DateTime.UtcNow, 2, 2, "regs", [], "", TimingSystem.StopWatch, DateTime.UtcNow, DateTime.UtcNow, 22, DateTime.UtcNow);

        var entrantFromDb = Models.GetEntrant(entrantId, eventId);
        _eventsRepository.Setup(a => a.Upsert(evt, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        _fileRepository.Setup(a => a.SaveMaps(eventId, "", TestContext.Current.CancellationToken)).ReturnsAsync("");
        _fileRepository.Setup(a => a.SaveRegs(eventId, "regs", TestContext.Current.CancellationToken)).ReturnsAsync("");

        var se = new SaveEvent(evt);
        var res = await _sut.Handle(se, TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }
}
