using System;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
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
        var eventId = 2ul;
        var evt = new Event(eventId, 1, "location", DateTime.UtcNow, 2, 2, [], TimingSystem.StopWatch, DateTime.UtcNow, DateTime.UtcNow, 22, DateTime.UtcNow);

        _eventsRepository.Setup(a => a.Upsert(evt, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);
        _fileRepository.Setup(a => a.SaveMaps(eventId, "maps", TestContext.Current.CancellationToken)).ReturnsAsync("").Verifiable(Times.Once);
        _fileRepository.Setup(a => a.SaveRegs(eventId, "regs", TestContext.Current.CancellationToken)).ReturnsAsync("").Verifiable(Times.Once);

        var se = new SaveEvent(evt, "maps", "regs");
        var res = await _sut.Handle(se, TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }
}
