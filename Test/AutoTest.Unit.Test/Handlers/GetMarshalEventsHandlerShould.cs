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

public class GetMarshalEventsHandlerShould
{
    private readonly MockRepository _mr;
    private readonly IRequestHandler<GetMarshalEvents, IEnumerable<ulong>> _sut;
    private readonly Mock<IMarshalsRepository> _marshalsRepository;

    public GetMarshalEventsHandlerShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _marshalsRepository = _mr.Create<IMarshalsRepository>();
        _sut = new GetMarshalEventsHandler(_marshalsRepository.Object);
    }

    [Fact]
    public async Task GetMarshalEvents()
    {
        var eventId = 1ul;
        var marshals = new[] {
            new Marshal(1, "b", "b", "a@a.com", eventId, 212312, ""),
            new Marshal(2, "a", "a", "a@a.com", eventId, 212312, "")
        };
        var mock = marshals.BuildMock();
        _marshalsRepository.Setup(a => a.GetByEmail("test@test.com")).Returns(mock);

        var res = await _sut.Handle(new("test@test.com"), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(marshals.OrderBy(a => a.FamilyName).Select(a => a.EventId).Distinct(), o => o.WithStrictOrdering());
        _mr.VerifyAll();
    }
}
