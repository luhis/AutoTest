using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Web;
using AutoTest.Web.Models;
using FluentAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers
{
    public class AccessControllerShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient unAuthorisedClient;

        public AccessControllerShould(CustomWebApplicationFactory<Startup> fixture)
        {
            this.unAuthorisedClient = fixture.GetUnAuthorisedClient();
        }

        [Fact]
        public async Task GetUnauthorised()
        {
            var res = await unAuthorisedClient.GetAsync("/api/access/");
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var content = await res.Content.ReadAsStringAsync();
            var accessModel = JsonSerializer.Deserialize<AccessModel>(content, new JsonSerializerOptions() { });
            accessModel.Should().NotBeNull();
            accessModel.Should().BeEquivalentTo(new AccessModel(false, false, false, false, null!, null!, null!, null!));
        }
    }
}
