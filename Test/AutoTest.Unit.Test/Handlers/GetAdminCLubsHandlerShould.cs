using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AwesomeAssertions;
using Mediator;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class GetAdminClubsHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IClubsRepository> _clubsRepository;
    private readonly Mock<IMemoryCache> _memoryCache;
    private readonly IRequestHandler<GetAdminClubs, IEnumerable<ulong>> _sut;

    public GetAdminClubsHandlerShould()
    {
        _clubsRepository = _mr.Create<IClubsRepository>();
        _memoryCache = _mr.Create<IMemoryCache>();
        _sut = new GetAdminClubsHandler(_clubsRepository.Object, _memoryCache.Object);
    }

    [Fact]
    public async Task ShouldSkipIfInCache()
    {
        object? outObj;
        _memoryCache
            .Setup(a => a.TryGetValue(nameof(GetAdminClubsHandler), out outObj)).Returns((string _, out IEnumerable<(ulong ClubId, IEnumerable<AuthorisationEmail> AdminEmails)> outObj) =>
            {
                outObj = new[] { (ClubId: 1ul, AdminEmails: Enumerable.Empty<AuthorisationEmail>()) }.AsEnumerable();
                return true;
            });
        var email = "a@a.com";
        var res = await _sut.Handle(new(email), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(Enumerable.Empty<ulong>());
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ShouldCreateIfNotInCache()
    {
        object? outObj;
        _memoryCache
            .Setup(a => a.TryGetValue(nameof(GetAdminClubsHandler), out outObj))
            .Returns(false);
        var ce = _mr.Create<ICacheEntry>();
        ce.Setup(a => a.Dispose());
        ce.SetupSet(a => a.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30));
        ce.SetupSet(a => a.Value = Enumerable.Empty<(ulong ClubId, IEnumerable<AuthorisationEmail> AdminEmails)>());
        _memoryCache.Setup(a => a.CreateEntry(nameof(GetAdminClubsHandler))).Returns(ce.Object);
        _clubsRepository.Setup(a => a.GetAll(TestContext.Current.CancellationToken)).ReturnsAsync(Enumerable.Empty<Club>());
        var email = "a@a.com";
        var res = await _sut.Handle(new(email), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(Enumerable.Empty<ulong>());
        _mr.VerifyAll();
    }
}
