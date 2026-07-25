using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using AwesomeAssertions.ArgumentMatchers.Moq;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class DeleteMarshalShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IMarshalsRepository> _marshalsRepository;
    private readonly Mock<IAuthorisationNotifier> _signalRNotifier;
    private readonly IRequestHandler<DeleteMarshal> _sut;

    public DeleteMarshalShould()
    {
        _marshalsRepository = _mr.Create<IMarshalsRepository>();
        _signalRNotifier = _mr.Create<IAuthorisationNotifier>();
        _sut = new DeleteMarshalHandler(_marshalsRepository.Object, _signalRNotifier.Object);
    }

    [Fact]
    public async Task DeleteMarshal()
    {
        var eventId = 1ul;
        var marshalId = 2ul;
        var marshal = new Domain.StorageModels.Marshal(marshalId, "joe", "bloggs", "joe@bloggs.com", eventId, 1234, "");
        _marshalsRepository.Setup(a => a.GetById(eventId, marshalId, TestContext.Current.CancellationToken)).ReturnsAsync(marshal);
        _marshalsRepository.Setup(a => a.Remove(marshal, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        _signalRNotifier.Setup(a => a.RemoveEventMarshal(marshalId, Its.EquivalentTo(new[] { "joe@bloggs.com" }), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        await _sut.Handle(new(eventId, marshalId), TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }
}
