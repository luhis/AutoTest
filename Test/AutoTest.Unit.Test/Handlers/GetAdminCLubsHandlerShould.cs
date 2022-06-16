using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public delegate void CallbackDelegate(object p1, out object p2);
    public class GetAdminClubsHandlerShould
    {
        private readonly IRequestHandler<GetAdminClubs, IEnumerable<ulong>> sut;
        private readonly MockRepository mr;
        private readonly Mock<IClubsRepository> clubsRepository;
        private readonly Mock<IMemoryCache> memoryCache;

        public GetAdminClubsHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            clubsRepository = mr.Create<IClubsRepository>();
            memoryCache = mr.Create<IMemoryCache>();
            sut = new GetAdminClubsHandler(clubsRepository.Object, memoryCache.Object);
        }

        [Fact(Skip = "skip")]
        public async Task ShouldSkipIfInCache()
        {
            object outObj;
            memoryCache
                .Setup(a => a.TryGetValue(nameof(GetAdminClubsHandler), out outObj)).Returns(true)
                .Callback(new CallbackDelegate((object word, out object substitution) =>
                substitution = new[] { (object)(ClubId: 1ul, AdminEmails: Enumerable.Empty<AuthorisationEmail>()) }.AsEnumerable()))
                ;
            var email = "a@a.com";
            var res = await sut.Handle(new(email), CancellationToken.None);

            res.Should().BeEquivalentTo(Enumerable.Empty<ulong>());
            mr.VerifyAll();
        }

        [Fact]
        public async Task ShouldCreateIfNotInCache()
        {
            object outObj;
            memoryCache
                .Setup(a => a.TryGetValue(nameof(GetAdminClubsHandler), out outObj))
                .Callback(new CallbackDelegate((object word, out object substitution) =>
                substitution = new[] { (object)(ClubId: 1ul, AdminEmails: Enumerable.Empty<AuthorisationEmail>()) }.AsEnumerable()))
                .Returns(false);
            var ce = mr.Create<ICacheEntry>();
            ce.Setup(a => a.Dispose());
            ce.SetupSet(a => a.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30));
            ce.SetupSet(a => a.Value = Enumerable.Empty<(ulong ClubId, IEnumerable<AuthorisationEmail> AdminEmails)>());
            memoryCache.Setup(a => a.CreateEntry(nameof(GetAdminClubsHandler))).Returns(ce.Object);
            clubsRepository.Setup(a => a.GetAll(CancellationToken.None)).ReturnsAsync(Enumerable.Empty<Club>());
            var email = "a@a.com";
            var res = await sut.Handle(new(email), CancellationToken.None);

            res.Should().BeEquivalentTo(Enumerable.Empty<ulong>());
            mr.VerifyAll();
        }
    }
}
