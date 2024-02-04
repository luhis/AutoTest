using System.Net.Http;
using System.Threading.Tasks;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Web;
using FluentAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers
{
    public class MarshalsControllerShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient unAuthorisedClient;

        public MarshalsControllerShould(CustomWebApplicationFactory<Startup> fixture)
        {
            this.unAuthorisedClient = fixture.GetUnAuthorisedClient();
        }

        [Fact]
        public async Task GetResults()
        {
            var res = await unAuthorisedClient.GetAsync("/api/marshals/1");
            var content = await res.Content.ReadAsStringAsync();
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
