using System.Net.Http;
using System.Threading.Tasks;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Web;
using FluentAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers
{
    public class ClubsControllerShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient unAuthorisedClient;

        public ClubsControllerShould(CustomWebApplicationFactory<Startup> fixture)
        {
            this.unAuthorisedClient = fixture.GetUnAuthorisedClient();
        }

        [Fact]
        public async Task GetResults()
        {
            var res = await unAuthorisedClient.GetAsync("/api/clubs/");
            var content = await res.Content.ReadAsStringAsync();
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
