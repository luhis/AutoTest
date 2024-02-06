using System.Net.Http;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using FluentAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers
{
    public class ProfileControllerShould : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<AuthdCustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient unAuthorisedClient;
        private readonly HttpClient authorisedClient;

        public ProfileControllerShould(CustomWebApplicationFactory<Startup> fixture, AuthdCustomWebApplicationFactory<Startup> fixture2)
        {
            this.unAuthorisedClient = fixture.GetUnAuthorisedClient();
            this.authorisedClient = fixture2.GetAuthorisedClient();
        }

        [Fact]
        public async Task GetProfileUnauthorized()
        {
            var res = await unAuthorisedClient.GetAsync("/api/profile");
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetProfile()
        {
            var res = await authorisedClient.GetAsync("/api/profile");
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var content = await res.DeserialiseAsync<Profile>();
            content.Should().BeEquivalentTo(new Profile("user@test.com", "", "", Domain.Enums.Age.Senior, false));
        }
    }
}
