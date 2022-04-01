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
    public delegate void CallbackDelegate(string p1, out IEnumerable<(ulong ClubId, IEnumerable<AuthorisationEmail> AdminEmails)> p2);
    public class GetAdminCLubsHandlerShould
    {
        private readonly IRequestHandler<GetAdminClubs, IEnumerable<ulong>> sut;
        private readonly MockRepository mr;
        private readonly Mock<IClubsRepository> clubsRepository;
        private readonly Mock<IMemoryCache> memoryCache;

        public GetAdminCLubsHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            clubsRepository = mr.Create<IClubsRepository>();
            memoryCache = mr.Create<IMemoryCache>();
            sut = new GetAdminClubsHandler(clubsRepository.Object, memoryCache.Object);
        }

        [Fact(Skip = "skip")]
        public async Task ShouldSkipIfInCache()
        {
            IEnumerable<(ulong ClubId, IEnumerable<AuthorisationEmail> AdminEmails)> outObj;
            memoryCache
                .Setup(a => a.TryGetValue(nameof(GetAdminClubsHandler), out outObj))
                .Callback(new CallbackDelegate((string word, out IEnumerable<(ulong ClubId, IEnumerable<AuthorisationEmail> AdminEmails)> substitution) =>
                substitution = new[] { (ClubId: 1ul, AdminEmails: System.Array.Empty<AuthorisationEmail>().AsEnumerable()) }.AsEnumerable())).Returns(true);
            var email = "a@a.com";
            var res = await sut.Handle(new(email), CancellationToken.None);

            res.Should().BeEquivalentTo(Enumerable.Empty<ulong>());
            mr.VerifyAll();
        }
    }
}
