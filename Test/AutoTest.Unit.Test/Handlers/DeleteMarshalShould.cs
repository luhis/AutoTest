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
    private readonly IRequestHandler<DeleteMarshal> sut;
    private readonly MockRepository mr;
    private readonly Mock<IMarshalsRepository> marshalsRepository;
    private readonly Mock<IAuthorisationNotifier> signalRNotifier;

    public DeleteMarshalShould()
    {
        mr = new MockRepository(MockBehavior.Strict);
        marshalsRepository = mr.Create<IMarshalsRepository>();
        signalRNotifier = mr.Create<IAuthorisationNotifier>();
        sut = new DeleteMarshalHandler(marshalsRepository.Object, signalRNotifier.Object);
    }

    [Fact]
    public async Task DeleteMarshal()
    {
        var eventId = 1ul;
        var marshalId = 2ul;
        var marshal = new Domain.StorageModels.Marshal(marshalId, "joe", "bloggs", "joe@bloggs.com", eventId, 1234, "");
        marshalsRepository.Setup(a => a.GetById(eventId, marshalId, TestContext.Current.CancellationToken)).ReturnsAsync(marshal);
        marshalsRepository.Setup(a => a.Remove(marshal, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        signalRNotifier.Setup(a => a.RemoveEventMarshal(marshalId, Its.EquivalentTo(new[] { "joe@bloggs.com" }), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        await sut.Handle(new(eventId, marshalId), TestContext.Current.CancellationToken);

        mr.VerifyAll();
    }
}
