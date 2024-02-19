using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using AutoTest.Web.Models.Display;
using FluentAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers
{
    public class EntrantsControllerShould(CustomWebApplicationFactory<Startup> fixture, AuthdCustomWebApplicationFactory<Startup> fixture2) : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<AuthdCustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient unAuthorisedClient = fixture.GetUnAuthorisedClient();
        private readonly HttpClient authorisedClient = fixture2.GetAuthorisedClient();

        [Fact]
        public async Task GetResults()
        {
            var res = await unAuthorisedClient.GetAsync("/api/entrants/22");
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var content = await res.DeserialiseAsync<IEnumerable<PublicEntrantModel>>();
            content.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetSingle()
        {
            var res = await authorisedClient.GetAsync("/api/entrants/22/1");
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var content = await res.DeserialiseAsync<Entrant>();
            content.Should().NotBeNull();
        }

        [Fact]
        public async Task NotFound()
        {
            var res = await authorisedClient.GetAsync("/api/entrants/22/9999");
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
