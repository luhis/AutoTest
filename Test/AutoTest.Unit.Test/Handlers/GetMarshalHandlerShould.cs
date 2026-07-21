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

public class GetMarshalHandlerShould
{
    private readonly MockRepository _mr;
    private readonly IRequestHandler<GetMarshal, Marshal?> _sut;
    private readonly Mock<IMarshalsRepository> _marshalsRepository;

    public GetMarshalHandlerShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _marshalsRepository = _mr.Create<IMarshalsRepository>();
        _sut = new GetMarshalHandler(_marshalsRepository.Object);
    }

    [Fact]
    public async Task ReturnNullWhenNotFound()
    {
        var eventId = 1ul;
        var marshalId = 2ul;
        _marshalsRepository.Setup(a => a.GetById(eventId, marshalId, TestContext.Current.CancellationToken)).ReturnsAsync((Marshal?)null);

        var res = await _sut.Handle(new(eventId, marshalId), TestContext.Current.CancellationToken);

        res.Should().BeNull();
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ReturnMarshal()
    {
        var eventId = 1ul;
        var marshalId = 2ul;
        var marshal = Models.GetMarshal(marshalId, eventId);
        _marshalsRepository.Setup(a => a.GetById(eventId, marshalId, TestContext.Current.CancellationToken)).ReturnsAsync(marshal);

        var res = await _sut.Handle(new(eventId, marshalId), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(marshal);
        _mr.VerifyAll();
    }
}
