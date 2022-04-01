using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.Fixtures;
using FluentAssertions;
using MediatR;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{

    public class GetProfileShould
    {
        private readonly IRequestHandler<GetProfile, Profile> sut;
        private readonly AutoTestContext context;

        public GetProfileShould()
        {
            context = InMemDbFixture.GetDbContext();
            sut = new GetProfileHandler(context);
        }

        [Fact]
        public async Task ReturnBlankProfileIfNone()
        {
            context.Users!.Should().BeEmpty();
            var email = "a@a.com";
            var res = await sut.Handle(new(email), CancellationToken.None);

            res.EmailAddress.Should().BeEquivalentTo(email);
            context.Users!.Should().BeEmpty();
        }
    }
}
