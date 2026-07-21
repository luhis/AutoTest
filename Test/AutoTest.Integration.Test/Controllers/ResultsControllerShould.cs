using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Service.Models;
using AutoTest.Web;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class ResultsControllerShould(TestWebApplicationFactory<Startup> factory) : IClassFixture<TestWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client = factory.GetUnAuthorisedClient();

    [Fact]
    public async Task GetResults()
    {
        var res = await _client.GetAsync($"/api/results/{TestIds.EventId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<Result>>();
        content.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAwards()
    {
        var res = await _client.GetAsync($"/api/results/{TestIds.EventId}/awards", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<Awards>();
        content.Should().NotBeNull();
        content!.Ftd.Should().NotBeNull();
        content.ClassAwards.Should().NotBeNull();
    }
}
