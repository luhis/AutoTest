using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AwesomeAssertions;
using Mediator;
using MockQueryable;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class GetMarshalsHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IMarshalsRepository> _marshalsRepository;
    private readonly IRequestHandler<GetMarshals, IEnumerable<Marshal>> _sut;

    public GetMarshalsHandlerShould()
    {
        _marshalsRepository = _mr.Create<IMarshalsRepository>();
        _sut = new GetMarshalsHandler(_marshalsRepository.Object);
    }

    [Fact]
    public async Task GetMarshals()
    {
        var eventId = 1ul;
        var marshals = new[] {
            new Marshal(1, "b", "b", "a@a.com", eventId, 212312, ""),
            new Marshal(2, "a", "a", "a@a.com", eventId, 212312, "")
        };
        var mock = marshals.BuildMock();
        _marshalsRepository.Setup(a => a.GetByEventId(eventId)).Returns(mock);

        var res = await _sut.Handle(new(eventId), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(marshals.OrderBy(a => a.FamilyName), o => o.WithStrictOrdering());
        _mr.VerifyAll();
    }
}
