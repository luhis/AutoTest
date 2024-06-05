using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Service.Models;
using AutoTest.Web;
using FluentAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers
{
    public class ResultsControllerShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient unAuthorisedClient;

        public ResultsControllerShould(CustomWebApplicationFactory<Startup> fixture)
        {
            this.unAuthorisedClient = fixture.GetUnAuthorisedClient();
        }

        [Fact]
        public async Task GetResults()
        {
            var res = await unAuthorisedClient.GetAsync("/api/results/22");
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var content = await res.DeserialiseAsync<IEnumerable<Result>>();
            content.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetAwards()
        {
            var res = await unAuthorisedClient.GetAsync("/api/results/22/awards");
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var content = await res.DeserialiseAsync<Awards>();
            content.Should().NotBeNull();
        }
    }
}
