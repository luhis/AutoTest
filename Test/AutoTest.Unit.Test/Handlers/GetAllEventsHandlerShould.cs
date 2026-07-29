using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using AwesomeAssertions;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class GetAllEventsHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IEventsRepository> _eventsRepository;
    private readonly Mock<IFileRepository> _fileRepository;
    private readonly IRequestHandler<GetAllEvents, IEnumerable<EventViewModel>> _sut;

    public GetAllEventsHandlerShould()
    {
        _eventsRepository = _mr.Create<IEventsRepository>();
        _fileRepository = _mr.Create<IFileRepository>();
        _sut = new GetAllEventsHandler(_eventsRepository.Object, _fileRepository.Object);
    }

    [Fact]
    public async Task ReturnEvents()
    {
        var events = new[] { Models.GetEvent(1), Models.GetEvent(2) };
        _eventsRepository.Setup(a => a.GetAll(TestContext.Current.CancellationToken)).ReturnsAsync(events).Verifiable(Times.Once);
        _fileRepository.Setup(a => a.HasRegs(1UL, TestContext.Current.CancellationToken)).ReturnsAsync(false).Verifiable(Times.Once);
        _fileRepository.Setup(a => a.HasMaps(1UL, TestContext.Current.CancellationToken)).ReturnsAsync(true).Verifiable(Times.Once);
        _fileRepository.Setup(a => a.HasRegs(2UL, TestContext.Current.CancellationToken)).ReturnsAsync(true).Verifiable(Times.Once);
        _fileRepository.Setup(a => a.HasMaps(2UL, TestContext.Current.CancellationToken)).ReturnsAsync(false).Verifiable(Times.Once);

        var res = (await _sut.Handle(new(), TestContext.Current.CancellationToken)).ToArray();

        res.Should().BeEquivalentTo(new[]
        {
            new { EventId = 1UL, HasRegulations = false, HasMaps = true },
            new { EventId = 2UL, HasRegulations = true, HasMaps = false },
        });
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ReturnEmptyWhenNoEvents()
    {
        _eventsRepository.Setup(a => a.GetAll(TestContext.Current.CancellationToken)).ReturnsAsync(Enumerable.Empty<Event>()).Verifiable(Times.Once);

        var res = (await _sut.Handle(new(), TestContext.Current.CancellationToken)).ToArray();

        res.Should().BeEmpty();
        _mr.VerifyAll();
    }
}
