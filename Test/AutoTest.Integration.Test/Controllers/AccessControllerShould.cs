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
    public class AccessControllerShould : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<AuthdCustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient unAuthorisedClient;
        private readonly HttpClient authorisedClient;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public AccessControllerShould(CustomWebApplicationFactory<Startup> fixture, AuthdCustomWebApplicationFactory<Startup> fixture2)
        {
            this.unAuthorisedClient = fixture.GetUnAuthorisedClient();
            this.authorisedClient = fixture2.CreateClient();
        }

        [Fact]
        public async Task GetUnauthorised()
        {
            var res = await unAuthorisedClient.GetAsync("/api/access/");
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var content = await res.Content.ReadAsStringAsync();
            var accessModel = JsonSerializer.Deserialize<AccessModel>(content, options);
            accessModel.Should().NotBeNull();
            accessModel.Should().BeEquivalentTo(new AccessModel(false, false, false, false, [], [], [], []));
        }

        [Fact]
        public async Task GetAuthorised()
        {
            var res = await authorisedClient.GetAsync("/api/access/");
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var content = await res.Content.ReadAsStringAsync();
            var accessModel = JsonSerializer.Deserialize<AccessModel>(content, options);
            accessModel.Should().NotBeNull();
            accessModel.Should().BeEquivalentTo(new AccessModel(false, true, true, true, [], [], [], []));
        }
    }
}
